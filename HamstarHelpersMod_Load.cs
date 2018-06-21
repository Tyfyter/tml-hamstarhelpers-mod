﻿using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Internals.ControlPanel.Inbox;
using HamstarHelpers.MiscHelpers;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.EntityGroups;
using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Timers;
using HamstarHelpers.Internals.WebRequests;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Internals.ControlPanel;
using HamstarHelpers.TmlHelpers.LoadHelpers;

namespace HamstarHelpers {
	partial class HamstarHelpersMod : Mod {
		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			DebugHelpers.LogHelpers.Log( "UNHANDLED crash? "+e.IsTerminating+" \nSender: "+sender.ToString()+" \nMessage: "+e.ExceptionObject.ToString() );
		}



		////////////////

		internal JsonConfig<HamstarHelpersConfigData> JsonConfig;
		public HamstarHelpersConfigData Config { get { return JsonConfig.Data; } }

		internal IDictionary<int, Type> OldPacketProtocols = new Dictionary<int, Type>();
		internal IDictionary<int, Type> PacketProtocols = new Dictionary<int, Type>();

		internal HamstarExceptionManager ExceptionMngr;
		internal Timers Timers;
		internal DebugHelpers.LogHelpers LogHelpers;
		internal TmlHelpers.ModMetaDataManager ModMetaDataManager;
		internal NetHelpers.NetHelpers NetHelpers;
		internal BuffHelpers.BuffHelpers BuffHelpers;
		internal ItemHelpers.ItemIdentityHelpers ItemIdentityHelpers;
		internal NPCHelpers.NPCIdentityHelpers NPCIdentityHelpers;
		internal ProjectileHelpers.ProjectileIdentityHelpers ProjectileIdentityHelpers;
		internal BuffHelpers.BuffIdentityHelpers BuffIdentityHelpers;
		internal NPCHelpers.NPCBannerHelpers NPCBannerHelpers;
		internal RecipeHelpers.RecipeHelpers RecipeHelpers;
		internal LoadHelpers LoadHelpers;
		internal TmlHelpers.TmlPlayerHelpers TmlPlayerHelpers;
		internal WorldHelpers.WorldHelpers WorldHelpers;
		internal TmlHelpers.ModHelpers.ModLockHelpers ModLockHelpers;
		internal EntityGroups EntityGroups;
		internal AnimatedColorsManager AnimatedColors;
		internal PlayerMessages PlayerMessages;
		internal InboxControl Inbox;
		internal ModVersionGet ModVersionGet;
		internal ServerBrowserReporter ServerBrowser;
		internal MenuItemManager MenuItemMngr;
		internal Utilities.Menu.OldMenuItemManager OldMenuItemMngr;
		internal MusicHelpers MusicHelpers;

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }

		public ModHotKey ControlPanelHotkey = null;

		public UIControlPanel ControlPanel = null;
		 private int LastSeenScreenWidth = -1;
		 private int LastSeenScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public HamstarHelpersMod() {
			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;
			
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ExceptionMngr = new HamstarExceptionManager();
			this.AnimatedColors = new AnimatedColorsManager();
			this.JsonConfig = new JsonConfig<HamstarHelpersConfigData>( HamstarHelpersConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new HamstarHelpersConfigData() );
		}

		public override void Load() {
			HamstarHelpersMod.Instance = this;

			this.LoadConfigs();

			if( !this.HasUnhandledExceptionLogger && this.Config.DebugModeUnhandledExceptionLogging ) {
				this.HasUnhandledExceptionLogger = true;
				AppDomain.CurrentDomain.UnhandledException += HamstarHelpersMod.UnhandledLogger;
			}

			this.LoadHelpers = new LoadHelpers();

			this.Timers = new Timers();
			this.LogHelpers = new DebugHelpers.LogHelpers();
			this.ModMetaDataManager = new TmlHelpers.ModMetaDataManager();
			this.BuffHelpers = new BuffHelpers.BuffHelpers();
			this.NetHelpers = new NetHelpers.NetHelpers();
			this.ItemIdentityHelpers = new ItemHelpers.ItemIdentityHelpers();
			this.NPCIdentityHelpers = new NPCHelpers.NPCIdentityHelpers();
			this.ProjectileIdentityHelpers = new ProjectileHelpers.ProjectileIdentityHelpers();
			this.BuffIdentityHelpers = new BuffHelpers.BuffIdentityHelpers();
			this.NPCBannerHelpers = new NPCHelpers.NPCBannerHelpers();
			this.RecipeHelpers = new RecipeHelpers.RecipeHelpers();
			this.TmlPlayerHelpers = new TmlHelpers.TmlPlayerHelpers();
			this.WorldHelpers = new WorldHelpers.WorldHelpers();
			this.ControlPanel = new UIControlPanel();
			this.ModLockHelpers = new TmlHelpers.ModHelpers.ModLockHelpers();
			this.EntityGroups = new EntityGroups();
			this.PlayerMessages = new PlayerMessages();
			this.Inbox = new InboxControl();
			this.ModVersionGet = new ModVersionGet();
			this.ServerBrowser = new ServerBrowserReporter();
			this.MenuItemMngr = new MenuItemManager();
			this.OldMenuItemMngr = new Utilities.Menu.OldMenuItemManager();
			this.MusicHelpers = new MusicHelpers();

#pragma warning disable 612, 618
			TmlHelpers.AltNPCInfo.DataInitialize();
			TmlHelpers.AltProjectileInfo.DataInitialize();
#pragma warning restore 612, 618

			if( !this.Config.DisableControlPanelHotkey ) {
				this.ControlPanelHotkey = this.RegisterHotKey( "Mod Helpers Control Panel", "O" );
			}

			this.LoadModData();
		}


		private void LoadConfigs() {
			if( !this.JsonConfig.LoadFile() ) {
				this.JsonConfig.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Mod Helpers updated to " + HamstarHelpersConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
			}
		}

		public override void Unload() {
			this.UnloadModData();

			this.LoadHelpers.FulfillModUnloadPromises();

			try {
				if( this.HasUnhandledExceptionLogger ) {
					this.HasUnhandledExceptionLogger = false;
					AppDomain.CurrentDomain.UnhandledException -= HamstarHelpersMod.UnhandledLogger;
				}
			} catch { }

			this.ExceptionMngr = null;
			this.Timers = null;
			this.JsonConfig = null;
			this.PacketProtocols = null;
			this.LogHelpers = null;
			this.ModMetaDataManager = null;
			this.BuffHelpers = null;
			this.NetHelpers = null;
			this.ItemIdentityHelpers = null;
			this.NPCIdentityHelpers = null;
			this.ProjectileIdentityHelpers = null;
			this.BuffIdentityHelpers = null;
			this.NPCBannerHelpers = null;
			this.RecipeHelpers = null;
			this.TmlPlayerHelpers = null;
			this.LoadHelpers = null;
			this.ModVersionGet = null;
			this.WorldHelpers = null;
			this.ModLockHelpers = null;
			this.EntityGroups = null;
			this.AnimatedColors = null;
			this.PlayerMessages = null;
			this.Inbox = null;
			this.ControlPanelHotkey = null;
			this.ControlPanel = null;
			this.ServerBrowser = null;
			this.MenuItemMngr = null;
			this.OldMenuItemMngr = null;
			this.MusicHelpers = null;

			HamstarHelpersMod.Instance = null;
		}

		////////////////

		public override void PostSetupContent() {
			this.OldPacketProtocols = Utilities.Network.OldPacketProtocol.GetProtocols();
			this.PacketProtocols = PacketProtocol.GetProtocols();

			this.LoadHelpers.OnPostSetupContent();
			this.ModMetaDataManager.OnPostSetupContent();

			if( !Main.dedServ ) {
				UIControlPanel.OnPostSetupContent( this );
			}

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			if( this.Config.AddCrimsonLeatherRecipe ) {
				var vertebrae_to_leather = new ModRecipe( this );

				vertebrae_to_leather.AddIngredient( ItemID.Vertebrae, 5 );
				vertebrae_to_leather.SetResult( ItemID.Leather );
				vertebrae_to_leather.AddRecipe();
			}
		}

		public override void AddRecipeGroups() {
			NPCBannerHelpers.InitializeBanners();

			foreach( var kv in HamstarHelpers.RecipeHelpers.RecipeHelpers.Groups ) {
				RecipeGroup.RegisterGroup( kv.Key, kv.Value );
			}

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.ItemIdentityHelpers.PopulateNames();
			this.NPCIdentityHelpers.PopulateNames();
			this.ProjectileIdentityHelpers.PopulateNames();
			this.BuffIdentityHelpers.PopulateNames();
			
			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			this.LoadHelpers.AddWorldUnloadEachPromise( () => {
				this.OnWorldExit();
			} );

			this.LoadHelpers.FulfillPostModLoadPromises();
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.LoadHelpers.PreSaveAndExit();
		}


		////////////////
		
		private void OnWorldExit() {
			var myworld = this.GetModWorld<HamstarHelpersWorld>();
			myworld.OnWorldExit();
		}
	}
}
