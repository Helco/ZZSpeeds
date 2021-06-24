using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    class Inventory : ITrigger
    {
        public readonly struct Item
        {
            public readonly CardId cardId;
            public readonly int amount; // is always 1 for fairies/spells

            public Item(CardId cardId, int amount)
            {
                this.cardId = cardId;
                this.amount = amount;
            }
        }

        private readonly GameState state;
        private readonly IntPtr indexMapPtr;
        private readonly IntPtr dataPtr;
        private readonly int capacity;
        private readonly MemoryWatcher<int> count;
        private readonly List<Item> items;

        public IReadOnlyList<Item> Items => items;

        public Inventory(GameState state)
        {
            this.state = state;

            var ptrInventoryList = state.PlayerPtr +
                state.Version.OffPlayerToInventory +
                state.Version.OffInventoryToCardsList;
            if (!state.Process.ReadValue(ptrInventoryList + state.Version.OffListToCapacity, out capacity) ||
                !state.Process.ReadPointer(ptrInventoryList + state.Version.OffListToIndexMap, out indexMapPtr) ||
                !state.Process.ReadPointer(ptrInventoryList + state.Version.OffListToData, out dataPtr))
                throw new ApplicationException("Could not read inventory variables");
            count = new MemoryWatcher<int>(ptrInventoryList + state.Version.OffListToCount);

            items = new List<Item>(capacity);
        }

        public void Dispose() { }

        public IEnumerable<MemoryWatcher> MemoryWatchers => new[] { count };

        public void Update()
        {
            if (!state.Process.ReadBytes(indexMapPtr, 4 * count.Current, out var indexMapBytes))
            {
                Debug.WriteLine("Could not read inventory indices");
                return;
            }
            var indices = Enumerable
                .Range(0, count.Current)
                .Select(i => BitConverter.ToInt32(indexMapBytes, i * 4));
            items.Clear();
            if (!indices.Any())
                return;

            var maxIndex = indices.Max();
            if (!state.Process.ReadBytes(dataPtr, 4 * (maxIndex + 1), out var dataBytes))
            {
                Debug.WriteLine("Could not read inventory data");
                return;
            }
            var dataPtrs = indices
                .Select(i => new IntPtr(BitConverter.ToInt32(dataBytes, i * 4)));

            foreach (var slotPtr in dataPtrs)
            {
                var cardIdPtr = slotPtr + state.Version.OffInventorySlotToCardId;
                var amountPtr = slotPtr + state.Version.OffInventorySlotToAmount;
                if (!state.Process.ReadValue(cardIdPtr, out uint cardIdNum))
                {
                    Debug.WriteLine("Could not read inventory card ID");
                    continue;
                }
                var cardId = new CardId(cardIdNum);
                int amount = 1;
                if (cardId.type == CardType.Item && !state.Process.ReadValue(amountPtr, out amount))
                {
                    Debug.WriteLine("Could not read inventory item amount");
                    return;
                }

                items.Add(new Item(cardId, amount));
            }
        }
    }
}
