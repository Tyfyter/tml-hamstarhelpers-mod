﻿using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.DataDumper;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		public static ModHelpersMod Instance;



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogHelpers.Log( "UNHANDLED crash? " + e.IsTerminating + " \nSender: " + sender.ToString() + " \nMessage: " + e.ExceptionObject.ToString() );
			
		}



		////////////////

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }



		////////////////

		public ModHelpersMod() {
			ModHelpersMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.InitializeInner();
		}


		public override void Load() {
			//ErrorLogger.Log( "Loading Mod Helpers. Ensure you have .NET Framework v4.6+ installed, if you're having problems." );
			if( Environment.Version < new Version( 4, 0, 30319, 42000 ) ) {
				SystemHelpers.OpenUrl( "https://dotnet.microsoft.com/download/dotnet-framework-runtime" );
				throw new FileNotFoundException( "Mod Helpers "+this.Version+" requires .NET Framework v4.6+ to work." );
			}

			this.LoadInner();
		}

		////

		public override void Unload() {
			this.UnloadInner();

			ModHelpersMod.Instance = null;
		}


		////////////////

		public override void PostSetupContent() {
			this.PostSetupContentInner();

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			this.AddRecipesInner();
		}

		public override void AddRecipeGroups() {
			this.AddRecipeGroupsInner();

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.PostAddRecipesInner();

			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			this.PostLoadAll();
/*DataDumper.SetDumpSource( "DEBUG", () => {
	var data = Services.DataStore.DataStore.GetAll();
	string str = "";

	foreach( var kv in data ) {
		string key = kv.Key as string;
		if( key == null ) { continue; }

		if( key[key.Length-1] != 'A' ) {
			continue;
		}

		string keyB = key.Substring( 0, key.Length - 1 ) + "B";
		if( !data.ContainsKey(keyB) ) { continue; }

		double valA = (double)kv.Value;
		double valB = (double)data[keyB];
		if( valA != valB ) {
			str += key + " " + valA + " vs " + valB+",\n";
		}
	}

	return str;
} );*/
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.Promises.PreSaveAndExit();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			try {
				int protocolCode = reader.ReadInt32();
				
				if( Main.netMode == 1 ) {
					PacketProtocol.HandlePacketOnClient( protocolCode, reader, playerWho );
				} else if( Main.netMode == 2 ) {
					PacketProtocol.HandlePacketOnServer( protocolCode, reader, playerWho );
				}
			} catch( Exception e ) {
				LogHelpers.Alert( e.ToString() );
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
