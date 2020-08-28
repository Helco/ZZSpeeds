/*
 * AutoSplitter for Zanzarah Any% by Helco
 *
 * Adds an autostart for zanzarah (triggered by the loading button)
 * and autosplit for:
 *   - getting important items / fairies
 *	 - getting certain number of fairies
 *   - getting any psy fairy (previously used in Any%)
 *   - defeating certain enemies
 *   - reaching certain scenes
 *   - playing the end-game video
 * also game time vs real time by eliminating load time
 *
 * Will most likely *only* work with:
 *   - 1.002 (Original release used for Any% speedruns)
 *   - 1.010 (Steam Version used for Card%)
 */

state("zanthp")
{
}

state("main")
{
}

startup
{
	settings.Add("reset_in_main_menu", false, "Reset run in main menu");
	settings.Add("auto_start_on_new_game", true, "Starts run when creating a new game");
	settings.Add("split_after", true, "Splits after");
	{
		settings.Add("getting", true, "getting:", "split_after");
		{
			settings.Add("get_nature_card", true, "Nature card", "getting");
			settings.Add("get_nature_key", true, "Nature key", "getting");
			settings.Add("get_earth_key", false, "Earth key", "getting");
			settings.Add("get_air_key", true, "Air key", "getting");
			settings.Add("get_clover", false, "Clover", "getting");
			settings.Add("get_viteria", false, "Viteria", "getting");
			settings.Add("get_suane", true, "Suane", "getting");
			settings.Add("get_segbuzz", false, "Segbuzz", "getting");
			settings.Add("get_any_psy", true, "Any Psy fairy", "getting");
		}
		settings.Add("getting_nf", true, "getting at least:", "split_after");
		{
			settings.Add("get_8_fairies", false, "8 Fairies", "getting_nf");
			settings.Add("get_10_fairies", false, "10 Fairies", "getting_nf");
			settings.Add("get_16_fairies", false, "16 Fairies", "getting_nf");
			settings.Add("get_19_fairies", false, "19 Fairies", "getting_nf");
			settings.Add("get_28_fairies", false, "28 Fairies", "getting_nf");
			settings.Add("get_29_fairies", false, "29 Fairies", "getting_nf");
			settings.Add("get_30_fairies", false, "30 Fairies", "getting_nf");
		}
		settings.Add("defeating", true, "defeating:", "split_after");
		{
			settings.Add("defeat_scarecrow", true, "Scarecrow", "defeating");
			settings.Add("defeat_joe", true, "Shadowelf - in the mountains (Joe)", "defeating");
			settings.Add("defeat_bob1", true, "Shadow Boss - in the mountains (Bob)", "defeating");
			settings.Add("defeat_bob2", true, "Shadow Boss - in the shadow realm (Bob)", "defeating");
			settings.Add("defeat_bbm", false, "Shadow General - in the shadow realm (Boyband Manager)", "defeating");
			settings.Add("defeat_druid1", false, "White Druid 1 - on base level in dark cathedral", "defeating");
			settings.Add("defeat_druid2", false, "White Druid 2 - on the upper level in dark cathedral", "defeating");
		}
		settings.Add("reaching", true, "reaching:", "split_after");
		{
			settings.Add("reach_tiralin", false, "Tiralin", "reaching");
			settings.Add("reach_dunmore", true, "Dunmore", "reaching");
			settings.Add("reach_catacombs", false, "Catacombs", "reaching");
			settings.Add("reach_mountain_catchpoint", false, "Mountain Fairy-catchpoint", "reaching");
			settings.Add("reach_shadow_realm", false, "Shadow Realm", "reaching");
			settings.Add("reach_bonekeyskip", false, "Shadow realm after the bone key skip", "reaching");
			settings.Add("reach_dark_cathedral", true, "Dark Cathedral", "reaching");
		}
		settings.Add("playing", true, "starting to play:", "split_after");
		{
			settings.Add("play_endgame", true, "Endgame cutscene", "playing");
		}
	}
}

init
{
	print("Found some Zanzarah game process");
	refreshRate = 30;
	vars.foundGamePointer = false;
	vars.memWatchers = new MemoryWatcherList();

	// hook this to the timer as the process can be restarted and the gamesave even reloaded
	// yes this would make more sense in startup but we need access to the right `settings` object -_-
	timer.OnStart += (s, e) =>
	{
		Func<int, int, int> cardId = (id,type) => (id << 16) | (type << 8);
		vars.splittingItems = new HashSet<int>();
		vars.splittingNFairies = new HashSet<int>();
		vars.psyFairies = new HashSet<int>();
		vars.splittingEnemies = new HashSet<uint>();
		vars.splittingScenes = new HashSet<int>();
		vars.splittingVideos = new HashSet<string>();

		if (settings["get_nature_card"])	vars.splittingItems.Add(cardId(47, 0));
		if (settings["get_nature_key"])		vars.splittingItems.Add(cardId(56, 0));
		if (settings["get_earth_key"])		vars.splittingItems.Add(cardId(57, 0));
		if (settings["get_air_key"])		vars.splittingItems.Add(cardId(55, 0));
		if (settings["get_clover"])			vars.splittingItems.Add(cardId(10, 0));
		if (settings["get_viteria"])		vars.splittingItems.Add(cardId(1, 2));
		if (settings["get_suane"])			vars.splittingItems.Add(cardId(74, 2));
		if (settings["get_segbuzz"])		vars.splittingItems.Add(cardId(68, 2));
		if (settings["get_any_psy"])
		{
			vars.psyFairies.Add(cardId(27, 2)); // Mencre
			vars.psyFairies.Add(cardId(28, 2)); // Mensec
			vars.psyFairies.Add(cardId(46, 2)); // Beltaur
			vars.psyFairies.Add(cardId(47, 2)); // Mentaur
			vars.psyFairies.Add(cardId(48, 2)); // Clum
			vars.psyFairies.Add(cardId(49, 2)); // Clumaur
		}

		if (settings["get_8_fairies"])		vars.splittingNFairies.Add(8);
		if (settings["get_10_fairies"])		vars.splittingNFairies.Add(10);
		if (settings["get_16_fairies"])		vars.splittingNFairies.Add(16);
		if (settings["get_19_fairies"])		vars.splittingNFairies.Add(19);
		if (settings["get_28_fairies"])		vars.splittingNFairies.Add(28);
		if (settings["get_29_fairies"])		vars.splittingNFairies.Add(29);
		if (settings["get_30_fairies"])		vars.splittingNFairies.Add(30);

		if (settings["defeat_scarecrow"])	vars.splittingEnemies.Add(0x86DB0D84u);
		if (settings["defeat_joe"])			vars.splittingEnemies.Add(0xF78C1A24u);
		if (settings["defeat_bob1"])		vars.splittingEnemies.Add(0x63D94324u);
		if (settings["defeat_bob2"])		vars.splittingEnemies.Add(0xB8F89614u);
		if (settings["defeat_bbm"])			vars.splittingEnemies.Add(0x11428094u);
		if (settings["defeat_druid1"])		vars.splittingEnemies.Add(0x1F795CB4u);
		if (settings["defeat_druid2"])		vars.splittingEnemies.Add(0xE277B534u);

		if (settings["reach_tiralin"])			vars.splittingScenes.Add(231);
		if (settings["reach_dunmore"])			vars.splittingScenes.Add(1243);
		if (settings["reach_catacombs"])		vars.splittingScenes.Add(3010);
		if (settings["reach_mountain_catchpoint"]) vars.splittingScenes.Add(1402);
		if (settings["reach_shadow_realm"])		vars.splittingScenes.Add(840);
		if (settings["reach_bonekeyskip"])		vars.splittingScenes.Add(621);
		if (settings["reach_dark_cathedral"])	vars.splittingScenes.Add(400);

		if (settings["play_endgame"]) vars.splittingVideos.Add("_v006");
	};

	// Version *dependent* offsets
	if (game.ProcessName == "zanthp")
	{
		vars.sigGame = new SigScanTarget(0,
			"3C", "78", "5A", "00", // Virtual-table pointer
			"00", "00", "??", "00" // some buffer size either 0x00010000 or 0x00020000 (related to audio settings? frequency?)
		);
		vars.offGameToPlayer = 0x7320;
		vars.offSceneToDataset = 0x4C0;
		vars.offResMgrToVideoMgr = 0x71E0;
		vars.offLoadScene_enter = 0x4473E0;
		vars.lenLoadScene_enter = 0x4473E8 - vars.offLoadScene_enter;
		vars.offLoadScene_exit = 0x447563;
		vars.lenLoadScene_exit = 0x447568 - vars.offLoadScene_exit;
	}
	else if (game.ProcessName == "main")
	{
		vars.sigGame = new SigScanTarget(0,
			"0C", "68", "5A", "00",
			"00", "00", "??", "00"
		);
		vars.offGameToPlayer = 0x7250;
		vars.offSceneToDataset = 0x4B0;
		vars.offResMgrToVideoMgr = 0x7110;
		vars.offLoadScene_enter = 0x446136;
		vars.lenLoadScene_enter = 0x44613E - vars.offLoadScene_enter;
		vars.offLoadScene_exit = 0x4462B9;
		vars.lenLoadScene_exit = 0x4462BE - vars.offLoadScene_exit;
	}

	// Version *independent* offsets
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
	vars.offUIManagerToMainmenuScreenPtr = 0x80;
	vars.offDialogScreenToCauseType = 0xB5C;
	vars.offPlayerToCurrentNPC = 0x294;
	vars.offNPCToDatabaseRow = 0x13C;
	vars.offDatabaseRowToUID = 0x14;
	vars.offGameToResMgr = 0x38;
	vars.offResMgrToScene = 0x8;
	vars.offDatasetToDatasetStruct = 0x24;
	vars.offDatasetStructToSceneId = 0x0;
	vars.offVideoMgrToFilename = 0x14;

	vars.lenVideoMgrFilename = 64;

	// Setup LoadScene injection
	var asmToggleIsPlaying = new byte[]
	{
		0x66, 0x83, 0x35,		// xor
		0xEF, 0xBE, 0xAD, 0xDE, // toggle variable ptr
		0x01,					// xor 1 is equivalent to flipping the first bit
		0xE9,					// jmp
		0xC5, 0xCD, 0xCD, 0xCD	// back to gate (mind the -4 offset, original first byte was C9)
	};
	vars.offIsLoadingScene = game.AllocateMemory(4);
	ExtensionMethods.WriteBytes(game, vars.offIsLoadingScene, new byte[4] { 0, 0, 0, 0 });
	var bytesOffIsLoadingScene = BitConverter.GetBytes((int)vars.offIsLoadingScene.ToInt64());
	var injectedProcs = game.AllocateMemory(asmToggleIsPlaying.Length * 2);
	var offInjectedLoadSceneEnter = injectedProcs;
	var offInjectedLoadSceneExit = injectedProcs + asmToggleIsPlaying.Length;

	for (int i = 0; i < 4; i++)
		asmToggleIsPlaying[3 + i] = bytesOffIsLoadingScene[i];
	Action<int, int, IntPtr> InjectToggleLoadScene = (srcStart, srcLen, dst) => 
	{
		var gate = game.WriteDetour(new IntPtr(srcStart), srcLen, dst);
		var bytesGate = BitConverter.GetBytes((int)(gate.ToInt64() - dst.ToInt64() - asmToggleIsPlaying.Length));
		for (int i = 0; i < 4; i++)
			asmToggleIsPlaying[9 + i] = bytesGate[i];
		game.WriteBytes(dst, asmToggleIsPlaying);
	};
	InjectToggleLoadScene(vars.offLoadScene_enter, vars.lenLoadScene_enter, offInjectedLoadSceneEnter);
	InjectToggleLoadScene(vars.offLoadScene_exit, vars.lenLoadScene_exit, offInjectedLoadSceneExit);
	vars.isLoadingScene = false;
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

	/* START & RESET TRIGGER
	 * *********************
	 * According to the rules the timer starts when clicking on the confirm button in the savegame screen.
	 * Internally there are two different and I hope the first one is always the correct for this usage.
	 * What happens is that there is an animation playing when clicking on the button and this is
	 * documented in the code with a boolean variable. Let's watch it!
	 * Also look that the screen is actually correct, also if you are in the main menu, reset (maybe)
	 */
	IntPtr ptrPlayer = new IntPtr(vars.ptrGame.ToInt64() + vars.offGameToPlayer);
	IntPtr ptrUIManager = new IntPtr(ptrPlayer.ToInt64() + vars.offPlayerToUIManager);
	IntPtr ptrSavegameScreen = IntPtr.Zero;
	ExtensionMethods.ReadPointer(game, ptrUIManager + vars.offUIManagerToSavegameScreenPtr, out ptrSavegameScreen);
	IntPtr ptrMainmenuScreen = IntPtr.Zero;
	ExtensionMethods.ReadPointer(game, ptrUIManager + vars.offUIManagerToMainmenuScreenPtr, out ptrMainmenuScreen);
	vars.memShouldStart = new MemoryWatcher<byte>(ptrSavegameScreen + vars.offSavegameScreenToInExitingAnimation);
	vars.ptrSavegameScreen = ptrSavegameScreen;
	vars.ptrMainmenuScreen = ptrMainmenuScreen;

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
	 * And now the absolute hell! No just kidding, getting the scene changes is actually very easy:
	 * we just have to jump a few static references to a variable that holds the scene id
	 */
	vars.memSceneId = new MemoryWatcher<int>(vars.ptrGame +
		vars.offGameToResMgr +
		vars.offResMgrToScene +
		vars.offSceneToDataset +
		vars.offDatasetToDatasetStruct +
		vars.offDatasetStructToSceneId);

	/* VIDEO CHANGE TRIGGER
	 * ********************
	 * Yes official rules state that the video triggers the timer end, so it does here as well.
	 * Jump another static reference chain to the filename and watch for the end game cutscene
	 */
	vars.memVideoFilename = new StringWatcher(
		vars.ptrGame +
		vars.offGameToResMgr +
		vars.offResMgrToVideoMgr +
		vars.offVideoMgrToFilename,
		ReadStringType.UTF8,
		vars.lenVideoMgrFilename);

	/* IS LOADING SCENE
	 * ****************
	 * It gets scary: we already injected to ResourceMgr_loadScene function to toggle a bit in
	 * an allocated memory region both on enter and on exit, therefore we can look at it to 
	 * determine if the game is *currently* loading anything
	 */
	vars.memIsLoadingScene = new MemoryWatcher<bool>(vars.offIsLoadingScene);

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
		vars.memSceneId,
		vars.memVideoFilename,
		vars.memIsLoadingScene
	});
	return true;
}

start
{
	return
		settings["auto_start_on_new_game"] &&
		vars.memCurrentScreen.Current == (uint)vars.ptrSavegameScreen.ToInt64() &&	// are we even in the savegames screen?
		vars.memShouldStart.Current != 0;											// is it transitioning?
}

reset
{
	return
		settings["reset_in_main_menu"] &&
		vars.memCurrentScreen.Current == (uint)vars.ptrMainmenuScreen.ToInt64();
}

isLoading
{
	return !vars.foundGamePointer || vars.memIsLoadingScene.Current;
}

split
{
	/* Image the following event chain:
	 *   1. User starts run, gets psy fairy
	 *   2. Run dies, User goes to main menu, restarts run 
	 *   3. The new run starts with the `start` event, but the inventory is not cleared yet
	 *   4. User immediately gets the psy fairy split
	 *   (applies to every item split)
	 * So what we do is we wait until we are not in the savegame screen anymore before allowing any splits
	 */
	if (vars.memCurrentScreen.Current == (uint)vars.ptrSavegameScreen.ToInt64())
	{
		return false; // no splits for you mister!
	}

	/* so another caveat... A fairy trade does not change the count in the inventory
	 * makes sense right? but it actually replaces the slot so the last slot after 
	 * the trade may not even be the new fairy.
	 * But we have performance? So just traverse the whole inventory all the time and
	 * check every item :(
	 */
	IntPtr ptrInventoryData;
	ExtensionMethods.ReadPointer(game, vars.ptrInventoryList + vars.offListToData, out ptrInventoryData);
	IntPtr ptrInventoryIndexMap;
	ExtensionMethods.ReadPointer(game, vars.ptrInventoryList + vars.offListToIndexMap, out ptrInventoryIndexMap);
	int fairyCount = 0;
	for (int i = 0; i < vars.memItemCount.Current; i++)
	{
		// checking the last one should be fine
		int dataIndex;
		ExtensionMethods.ReadValue<int>(game, ptrInventoryIndexMap + 4 * i, out dataIndex);
		IntPtr ptrItem;
		ExtensionMethods.ReadPointer(game, ptrInventoryData + 4 * dataIndex, out ptrItem);
		int itemCardId;
		ExtensionMethods.ReadValue<int>(game, ptrItem + vars.offInventorySlotToCardId, out itemCardId);

		if (vars.splittingItems.Contains(itemCardId))
		{
			print("Found a splitting item " + itemCardId);
			vars.splittingItems.Remove(itemCardId);
			return true;
		}
		if (vars.psyFairies.Contains(itemCardId))
		{
			print("Found a psy fairy");
			vars.psyFairies.Clear();
			return true;
		}
		if (((itemCardId >> 8) & 3) == 2)
		{
			fairyCount++;
		}
	}

	/* Did we get n fairies?
	 * Note: we can check for equality here as there is no point in the game
	 * where the player gets more than one fairy
	 */
	 if (vars.splittingNFairies.Contains(fairyCount))
	 {
		 print("We got " + fairyCount + " fairies");
		 vars.splittingNFairies.Remove(fairyCount);
		 return true;
	 }

	// Was a NPC defeated?
	if (vars.memCurrentScreen.Old != vars.memCurrentScreen.Current &&			 // Has something happened?
		vars.memCurrentScreen.Current == (uint)vars.ptrDialogScreen.ToInt64() && // The after-fight dialog is active
		vars.memCauseType.Old != vars.memCauseType.Current &&
		vars.memCauseType.Current == 4 &&										 // There just ended a fight with "Player defeated NPC"
		vars.memCurrentNPC.Current != 0u)										 // There is a NPC the player defeated
	{
		IntPtr databaseRow;
		ExtensionMethods.ReadPointer(game, new IntPtr((Int64)vars.memCurrentNPC.Current) + vars.offNPCToDatabaseRow, out databaseRow);
		uint defeatedUID;
		ExtensionMethods.ReadValue<uint>(game, databaseRow + vars.offDatabaseRowToUID, out defeatedUID);
		vars.lastDefeatedNPC = vars.memCurrentNPC.Current;
		print("You defeated " + defeatedUID);
		print("memCurOld" + vars.memCurrentScreen.Old.ToString());
		print("memCurCur" + vars.memCurrentScreen.Current.ToString());
		print("memCauseTypeOld" + vars.memCauseType.Old.ToString());
		print("memCauseTypeCur" + vars.memCauseType.Current.ToString());
		print("memCurrentNPC.Cur" + vars.memCurrentNPC.Current.ToString());
		print("databaseRow" + databaseRow);
		print("defeatedUID" + defeatedUID);

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

	// Is a video playing?
	if (vars.memVideoFilename.Old != vars.memVideoFilename.Current)
	{
		print("You now play the video " + vars.memVideoFilename.Current);
		if (vars.splittingVideos.Contains(vars.memVideoFilename.Current))
		{
			vars.splittingVideos.Remove(vars.memVideoFilename.Current);
			return true;
		}
	}

	return false;
}
