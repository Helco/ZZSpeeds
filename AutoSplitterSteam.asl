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
	vars.offListToSlotCount = 0x00;
	vars.offListToNextFreeIndex = 0x8;
	vars.offListToData = 0x10;
	vars.offListToIndexMap = 0x14;
	vars.offInventorySlotToCardId = 0x04;

	vars.memShouldStart = new MemoryWatcher<byte>(IntPtr.Zero);
}

exit
{
	vars.foundGamePointer = false;
	vars.ptrGame = IntPtr.Zero;
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

	/* MEMORY WATCHER LIST
	 * *******************
	 */
	vars.memWatchers.AddRange(new MemoryWatcher[]
	{
		vars.memShouldStart
	});
	return true;
}

start
{
	return vars.memShouldStart.Current != 0;
}
