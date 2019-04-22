﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Config;
using System;
using Terraria;
using HamstarHelpers.Helpers.TmlHelpers;


namespace HamstarHelpers {
	public class HamstarHelpersConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Mod Helpers Config.json";


		////////////////

		public string VersionSinceUpdate = new Version(0,0,0,0).ToString();

		public bool DebugModeHelpersInfo = false;
		public bool DebugModeNetInfo = false;
		public bool DebugModeUnhandledExceptionLogging = true;
		public bool DebugModeDumpAlsoServer = false;
		public bool DebugModeResetCustomEntities = false;
		public bool DebugModeCustomEntityInfo = false;
		public bool DebugModeDisableSilentLogging = false;
		public bool DebugModePacketInfo = false;

		public bool UseCustomLogging = false;
		public bool UseCustomLoggingPerNetMode = false;
		public bool UseAlsoNormalLogging = false;

		public bool DisableControlPanel = false;
		public bool DisableControlPanelHotkey = false;
		public int ControlPanelIconX = 0;
		public int ControlPanelIconY = 0;

		public bool AddCrimsonLeatherRecipe = true;

		public bool WorldModLockEnable = true;
		public bool WorldModLockMinimumOnly = true;

		public bool ModCallCommandEnabled = true;

		public int ModIssueReportErrorLogMaxLines = 75;

		public bool IsServerHiddenFromBrowser = false;
		public bool IsServerHiddenFromBrowserUnlessPortForwardedViaUPNP = true;
		public bool IsServerPromptingUsersBeforeListingOnBrowser = true;
		//public int ServerBrowserCustomPort = -1;

		public int PacketRequestRetryDuration = 60 * 4;	// 5 seconds

		public int InboxIconPosX = 2;
		public int InboxIconPosY = 80;

		public int PingUpdateDelay = 60 * 15;	// 15 seconds

		public bool IsServerGaugingAveragePing = true;
		public bool IsCheckingModVersions = true;
		public bool IsCheckingModTags = true;

		public bool DisableJudgmentalTags = true;

		public bool DisableModTags = false;
		public bool DisableModRecommendations = false;
		public bool DisableModMenuUpdates = false;
		public bool DisableSupportLinks = false;

		public bool MagiTechScrapMechBossDropsEnabled = false;
		public bool CoalAsTile = true;

		public string PrivilegedUserId = "";



		////////////////

		internal bool UpdateToLatestVersion() {
			var mymod = ModHelpersMod.Instance;
			var newConfig = new HamstarHelpersConfigData();
			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();
			
			if( versSince >= mymod.Version ) {
				return false;
			}
			
			if( versSince < new Version(4,2,0,1) ) {
				if( this.ModIssueReportErrorLogMaxLines == 35 ) {
					this.ModIssueReportErrorLogMaxLines = newConfig.ModIssueReportErrorLogMaxLines;
				}
			}
			if( versSince < new Version(4,3,0) ) {
				this.DebugModeHelpersInfo = false;
			}
			if( versSince < new Version(4,3,1) ) {
				if( this.ModIssueReportErrorLogMaxLines == 50 ) {
					this.ModIssueReportErrorLogMaxLines = newConfig.ModIssueReportErrorLogMaxLines;
				}
			}
			
			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}


		////////////////
		
		internal void LoadFromNetwork( HamstarHelpersConfigData config ) {
			var mymod = ModHelpersMod.Instance;
			var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			mymod.ConfigJson.SetData( config );

			myplayer.Logic.FinishModSettingsSyncOnClient();
		}
	}
}
