/*
 * AutoSplitter for Zanzarah Any% (but for the !wrong! version Steam 1.010) by Helco
 * TODO: Add explaining overview
 */

state("zanthp")
{
}

state("main")
{
}

startup
{
	// hook this to the timer as the process can be restarted and the gamesave even reloaded
	timer.OnStart += (s, e) =>
	{
		Func<int, int, int> cardId = (id,type) => (id << 16) | (type << 8);
		vars.splittingItems = new HashSet<int>(new int[]
		{
			cardId(44, 0), // Nature card
			cardId(53, 0), // Nature key
			cardId(57, 0), // Earth key
			cardId(55, 0) // Air key
		});
		vars.psyFairies = new HashSet<int>(new int[]
		{
			cardId(27, 2), // Mencre
			cardId(28, 2), // Mensec
			cardId(46, 2), // Beltaur
			cardId(47, 2), // Mentaur
			cardId(48, 2), // Clum
			cardId(49, 2), // Clumaur
		});
		vars.splittingEnemies = new HashSet<uint>(new uint[]
		{
			0xED367294u
		});
		vars.splittingScenes = new HashSet<int>(new int[]
		{
			2421
		});
	};
}

init
{
	print("Found some Zanzarah game process");
	vars.foundGamePointer = false;
	vars.memWatchers = new MemoryWatcherList();

	if (game.ProcessName == "zanthp")
	{
		vars.sigGame = new SigScanTarget(0,
			"3C", "78", "5A", "00", // Virtual-table pointer
			"00", "00", "??", "00" // some buffer size either 0x00010000 or 0x00020000 (related to audio settings? frequency?)
		);
		vars.offGameToPlayer = 0x7320;
		vars.offSceneToDataset = 0x4C0;
	}
	else if (game.ProcessName == "main")
	{
		vars.sigGame = new SigScanTarget(0,
			"0C", "68", "5A", "00",
			"00", "00", "??", "00"
		);
		vars.offGameToPlayer = 0x7250;
		vars.offSceneToDataset = 0x4B0;
	}

	vars.offPlayerToUIManager = 0x188;
	vars.offUIManagerToSavegameScreenPtr = 0x84;
	vars.offSavegameScreenToInExitingAnimation = 0xF8;
	vars.offPlayerToInventory = 0x7C;
	vars.offInventoryToCardsList = 0x00;
	vars.offListToNextFreeIndex = 0x8;
	vars.offListToData = 0x10;
	vars.offListToIndexMap = 0x14;
	vars.offInventorySlotToCardId = 0x04;
	vars.offUIManagerToCurrentScreenPtr = 0x38;
	vars.offUIManagerToDialogScreenPtr = 0x74; // I don't have a better name for it yet okay?!
	vars.offDialogScreenToCauseType = 0xB5C;
	vars.offPlayerToCurrentNPC = 0x294;
	vars.offNPCToDatabaseRow = 0x13C;
	vars.offDatabaseRowToUID = 0x14;
	vars.offGameToResMgr = 0x38;
	vars.offResMgrToScene = 0x8;
	vars.offDatasetToDatasetStruct = 0x24;
	vars.offDatasetStructToSceneId = 0x0;
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
	IntPtr ptrPlayer = new IntPtr(vars.ptrGame.ToInt64() + vars.offGameToPlayer);
	IntPtr ptrUIManager = new IntPtr(ptrPlayer.ToInt64() + vars.offPlayerToUIManager);
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
	vars.ptrInventoryList = new IntPtr(ptrPlayer.ToInt64() + vars.offPlayerToInventory);
	vars.memItemCount = new MemoryWatcher<int>(vars.ptrInventoryList + vars.offListToNextFreeIndex);

	/* NPC DEFEAT TRIGGER
	 * ******************
	 * You think the last one had a bit of pointer magic? Surprise, this will be worse!
	 * Luckily we do not have to search for every NPC to examine its state. Instead we look for the current
	 * UI screen and examine that. There is a biiiiiig screen class I called `DialogScreen` which handles
	 * a lot of the in-game... well dialogs like buying, gambling and the after-fight information.
	 * So we will look for that screen, check that the cause type (which sadly only applies to after-fight)
	 * has the right value (4 = player defeated NPC), check that there was an actual NPC to be defated
	 * go to his database row reference and then check the ID against the hardcoded list of splitting NPCs.
	 * Oh and because in C# 64Bit IntPtr has a 8-byte size, we have to cast always to uint... What a fun!
	 */
	IntPtr ptrDialogScreen;
	ExtensionMethods.ReadPointer(game, ptrUIManager + vars.offUIManagerToDialogScreenPtr, out ptrDialogScreen);
	vars.ptrDialogScreen = ptrDialogScreen;
	vars.memCurrentScreen = new MemoryWatcher<uint>(ptrUIManager + vars.offUIManagerToCurrentScreenPtr);
	vars.memCauseType = new MemoryWatcher<int>(vars.ptrDialogScreen + vars.offDialogScreenToCauseType);
	vars.memCurrentNPC = new MemoryWatcher<uint>(ptrPlayer + vars.offPlayerToCurrentNPC);
	
	/* SCENE CHANGE TRIGGER
	 * ********************
	 * And now the absolute hell! No just kidding, getting the scene changes is actually very ease
	 * we just have to jump a few static references to a variable that holds the scene id
	 */
	vars.memSceneId = new MemoryWatcher<int>(vars.ptrGame +
		vars.offGameToResMgr +
		vars.offResMgrToScene +
		vars.offSceneToDataset +
		vars.offDatasetToDatasetStruct +
		vars.offDatasetStructToSceneId);

	/* MEMORY WATCHER LIST
	 * *******************
	 */
	vars.memWatchers.AddRange(new MemoryWatcher[]
	{
		vars.memShouldStart,
		vars.memItemCount,
		vars.memCurrentScreen,
		vars.memCauseType,
		vars.memCurrentNPC,
		vars.memSceneId
	});
	return true;
}

start
{
	return vars.memShouldStart.Current != 0;
}

split
{
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

		if (vars.splittingItems.Contains(lastItemCardId))
		{
			vars.splittingItems.Remove(lastItemCardId);
			return true;
		}
		if (vars.psyFairies.Contains(lastItemCardId))
		{
			vars.psyFairies.Clear();
			return true;
		}
	}

	// Was a NPC defeated?
	if (vars.memCurrentScreen.Old != vars.memCurrentScreen.Current &&			 // Has something happened?
		vars.memCurrentScreen.Current == (uint)vars.ptrDialogScreen.ToInt64() && // The after-fight dialog is active
		vars.memCauseType.Current == 4 &&										 // The last fight ended with "Player defeated NPC"
		vars.memCurrentNPC.Current != 0u)										 // There is a NPC the player defeated
	{
		IntPtr databaseRow;
		ExtensionMethods.ReadPointer(game, new IntPtr((Int64)vars.memCurrentNPC.Current) + vars.offNPCToDatabaseRow, out databaseRow);
		uint defeatedUID;
		ExtensionMethods.ReadValue<uint>(game, databaseRow + vars.offDatabaseRowToUID, out defeatedUID);
		vars.lastDefeatedNPC = vars.memCurrentNPC.Current;
		print("You defeated " + defeatedUID);

		if (vars.splittingEnemies.Contains(defeatedUID))
		{
			vars.splittingEnemies.Remove(defeatedUID);
			return true;
		}
	}

	// Did we change the scene
	if (vars.memSceneId.Current != vars.memSceneId.Old)
	{
		print("You changed scenes to " + vars.memSceneId.Current);
		if (vars.splittingScenes.Contains(vars.memSceneId.Current))
		{
			vars.splittingScenes.Remove(vars.memSceneId.Current);
			return true;
		}
	}

	return false;
}
