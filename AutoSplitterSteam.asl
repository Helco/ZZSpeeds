/*
 * AutoSplitter for Zanzarah Any% (but for the !wrong! version Steam 1.010) by Helco
 * TODO: Add explaining overview
 */

state("zanthp")
{
}

init
{
	print("Found some Zanzarah game process");
	vars.foundGamePointer = false;
	vars.memWatchers = new MemoryWatcherList();

	vars.sigGame = new SigScanTarget(0,
		"3C", "78", "5A", "00", // Virtual-table pointer
		"00", "00", "??", "00" // some buffer size either 0x00010000 or 0x00020000 (related to audio settings? frequency?)
	);
	vars.offGameToPlayer = 0x7320;
	vars.offPlayerToUIManager = 0x188;
	vars.offUIManagerToSavegameScreenPtr = 0x84;
	vars.offSavegameScreenToInExitingAnimation = 0xF8;
	vars.offPlayerToInventory = 0x7C;
	vars.offInventoryToCardsList = 0x00;
	vars.offListToNextFreeIndex = 0x8;
	vars.offListToData = 0x10;
	vars.offListToIndexMap = 0x14;
	vars.offInventorySlotToCardId = 0x04;

	Func<int, int, int> cardId = (id,type) => (id << 16) | (type << 8);
	vars.splittingItems = new HashSet<int>(new int[] {
		cardId(44, 0), // Nature card
		cardId(53, 0), // Nature key
		cardId(57, 0), // Earth key
		cardId(55, 0) // Air key
	});
	vars.gotPsyFairy = false;
	vars.psyFairies = new HashSet<int>(new int[] {
		cardId(27, 2), // Mencre
		cardId(28, 2), // Mensec
		cardId(46, 2), // Beltaur
		cardId(47, 2), // Mentaur
		cardId(48, 2), // Clum
		cardId(49, 2), // Clumaur
	});

	vars.memShouldStart = new MemoryWatcher<byte>(IntPtr.Zero);
}

exit
{
	vars.foundGamePointer = false;
	vars.ptrGame = IntPtr.Zero;
	vars.gotPsyFairy = false;
}

update
{
	if (vars.foundGamePointer)
	{
		vars.memWatchers.UpdateAll(game);
		return true;
	}

	/* FIND THE GAME POINTER
	 * *********************
	 */
	foreach (var page in game.MemoryPages())
	{
		var scanner = new SignatureScanner(game, page.BaseAddress, (int)page.RegionSize);
		vars.ptrGame = scanner.Scan(vars.sigGame);
		if (vars.ptrGame != IntPtr.Zero)
			break;
	}
	if (vars.ptrGame == IntPtr.Zero)
		return false; // didn't found it yet :(
	print("Found the game pointer at " + vars.ptrGame.ToString("X"));
	vars.foundGamePointer = true;

	/* START TRIGGER
	 * *************
	 * According to the rules the timer starts when clicking on the confirm button in the savegame screen.
	 * Internally there are two different and I hope the first one is always the correct for this usage.
	 * What happens is that there is an animation playing when clicking on the button and this is
	 * documented in the code with a boolean variable. Let's watch it!
	 */
	IntPtr ptrUIManager = new IntPtr(vars.ptrGame.ToInt64() + vars.offGameToPlayer + vars.offPlayerToUIManager);
	IntPtr ptrSavegameScreen = IntPtr.Zero;
	ExtensionMethods.ReadPointer(game, ptrUIManager + vars.offUIManagerToSavegameScreenPtr, out ptrSavegameScreen);
	vars.memShouldStart = new MemoryWatcher<byte>(ptrSavegameScreen + vars.offSavegameScreenToInExitingAnimation);

	/* INVENTORY TRIGGER
	 * *****************
	 * Yes, this will be much more complicated, but the gist is we watch for changes in the players
	 * inventory, then look what items got added. But this means we have to traverse a Zanzarah-style List.
	 * This works as follows:
	 *   - there are `nextFreeIndex` elements in the list
	 *   - for every 0 .. `nextFreeIndex` (exclusive) get the data index out of the `indexMap`
	 *   - get the element at that data index out of `data`
	 *   - in this case this is a `InventorySlot` which has a common member with the card id
	 */
	 vars.ptrInventoryList = new IntPtr(vars.ptrGame.ToInt64() + vars.offGameToPlayer + vars.offPlayerToInventory);
	 vars.memItemCount = new MemoryWatcher<int>(vars.ptrInventoryList + vars.offListToNextFreeIndex);

	/* MEMORY WATCHER LIST
	 * *******************
	 */
	vars.memWatchers.AddRange(new MemoryWatcher[]
	{
		vars.memShouldStart,
		vars.memItemCount
	});
	return true;
}

start
{
	return vars.memShouldStart.Current != 0;
}

split
{
	bool shouldSplit = false;

	// was there a new item added?
	if (vars.memItemCount.Old+1 == vars.memItemCount.Current) // are there actually new items
	{
		// checking the last one should be fine
		IntPtr ptrInventoryData;
		ExtensionMethods.ReadPointer(game, vars.ptrInventoryList + vars.offListToData, out ptrInventoryData);
		IntPtr ptrInventoryIndexMap;
		ExtensionMethods.ReadPointer(game, vars.ptrInventoryList + vars.offListToIndexMap, out ptrInventoryIndexMap);
		int lastItemIndex;
		ExtensionMethods.ReadValue<int>(game, ptrInventoryIndexMap + 4 * (vars.memItemCount.Current - 1), out lastItemIndex);
		IntPtr ptrLastItem;
		ExtensionMethods.ReadPointer(game, ptrInventoryData + 4 * lastItemIndex, out ptrLastItem);
		int lastItemCardId;
		ExtensionMethods.ReadValue<int>(game, ptrLastItem + vars.offInventorySlotToCardId, out lastItemCardId);

		print("You got mail eh card: " + lastItemCardId);

		shouldSplit = shouldSplit || vars.splittingItems.Contains(lastItemCardId);
		if (vars.psyFairies.Contains(lastItemCardId) && !vars.gotPsyFairy)
		{
			vars.gotPsyFairy = true;
			shouldSplit = true;
		}
	}

	return shouldSplit;
}
