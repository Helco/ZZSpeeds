using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter
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
                !state.Process.ReadValue(ptrInventoryList + state.Version.OffListToIndexMap, out int indexMapPtrI) ||
                !state.Process.ReadValue(ptrInventoryList + state.Version.OffListToData, out int dataPtrI))
                throw new ApplicationException("Could not read inventory variables");
            count = new MemoryWatcher<int>(ptrInventoryList + state.Version.OffListToCount);
            indexMapPtr = new IntPtr(indexMapPtrI);
            dataPtr = new IntPtr(dataPtrI);

            items = new List<Item>(capacity);
        }

        public void Dispose() { }

        public IEnumerable<MemoryWatcher> MemoryWatchers => new[] { count };

        public void Update()
        {
            if (!state.Process.ReadBytes(indexMapPtr, 4 * count.Current, out var indexMapBytes))
                throw new ApplicationException("Could not read inventory indices");
            var indices = Enumerable
                .Range(0, count.Current)
                .Select(i => BitConverter.ToInt32(indexMapBytes, i * 4));

            var maxIndex = indices.Max();
            if (!state.Process.ReadBytes(dataPtr, 4 * maxIndex, out var dataBytes))
                throw new ApplicationException("Could not read inventory data");
            var dataPtrs = indices
                .Select(i => new IntPtr(BitConverter.ToInt32(dataBytes, i * 4)));

            items.Clear();
            foreach (var slotPtr in dataPtrs)
            {
                var cardIdPtr = slotPtr + state.Version.OffInventorySlotToCardId;
                var amountPtr = slotPtr + state.Version.OffInventorySlotToAmount;
                if (!state.Process.ReadValue(cardIdPtr, out uint cardIdNum))
                    throw new ApplicationException("Could not read inventory slot cardId");
                var cardId = new CardId(cardIdNum);
                int amount = 1;
                if (cardId.type == CardType.Item && !state.Process.ReadValue(amountPtr, out amount))
                    throw new ApplicationException("Could not read inventory slot amount");

                items.Add(new Item(cardId, amount));
            }
        }
    }
}
