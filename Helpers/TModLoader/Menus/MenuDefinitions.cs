﻿using System;


namespace HamstarHelpers.Helpers.TModLoader.Menus {
	/*public enum VanillaMenuDefinitions {
		WorldEvilSelect = -71,
		WorldDifficultySelect = -7,
		Main = 0,
		1, (players loading?)
		CharacterCreate = 2,
		CharacterSelect = 5,
		6, (worlds loading?)
		WorldNameInput = 7,
		WorldSelect = 9,
		SinglePlayerBegin = 10, //?
		11, (some kind of menu to go back to?)
		Multiplayer = 12,
		14, (player disconnect?)
		15, (server disconnect?)
		WorldCreate = 16,
		PlayerHairPicker = 17,
		PlayerEyePicker = 18,
		PlayerSkinPicker = 19,
		20, ?
		PlayerShirtPicker = 21,
		PlayerUnderShirtPicker = 22,
		Pants = 23,
		Shoe = 24,
		MouseColor = 25,
		ServerPasswordInput = 31,
		Blank = 888,
		LanguageSelect = 1212,
		WorldSeedUI = 5000
	}*/




	public enum TModLoaderMenuDefinition {
		/*internal const int modsMenuID = 10000;
		internal const int modSourcesID = 10001;
		//set initial Main.menuMode to loadModsID
		internal const int loadModsProgressID = 10002;
		internal const int buildModProgressID = 10003;
		internal const int errorMessageID = 10005;
		internal const int reloadModsID = 10006;
		internal const int modBrowserID = 10007;
		internal const int modInfoID = 10008;
		//internal const int downloadModID = 10009;
		//internal const int modControlsID = 10010;
		internal const int managePublishedID = 10011;
		internal const int updateMessageID = 10012;
		internal const int infoMessageID = 10013;
		internal const int enterPassphraseMenuID = 10015;
		internal const int modPacksMenuID = 10016;
		internal const int tModLoaderSettingsID = 10017;
		internal const int enterSteamIDMenuID = 10018;
		internal const int extractModProgressID = 10019;
		internal const int downloadProgressID = 10020;
		internal const int uploadModProgressID = 10021;
		internal const int developerModeHelpID = 10022;
		internal const int progressID = 10023;
		internal const int modConfigID = 10024;
		internal const int createModID = 10025;
		internal const int exitID = 10026;*/

		Mods = 10000,
		ModSources = 10001,
		LoadModsProgress = 10002,
		BuildModProgress = 10003,
		ErrorMessage = 10005,
		ModBrowser = 10007,
		ModInfo = 10008,
		ManagePublished = 10011,
		UpdateMessage = 10012,
		InfoMessage = 10013,
		EnterPassphraseMenu = 10015,
		ModPacks = 10016,
		EnterSteamIDMenu = 10018,
		ExtractModProgress = 10019,
		DownloadProgress = 10020,
		UploadModProgress = 10021,
		DeveloperModeHelp = 10022,
		Progress = 10023,
		ModConfig = 10024,
		ModConfigList = -1,	//?
		CreateMod = 10025,
	}
}
