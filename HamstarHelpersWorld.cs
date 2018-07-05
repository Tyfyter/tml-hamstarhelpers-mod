﻿using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Internals.Logic;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersWorld : ModWorld {
		public string ObsoleteID2 { get; private set; }
		
		internal string ObsoleteID;
		public bool HasCorrectID { get; internal set; }  // Workaround for tml bug?

		internal WorldLogic WorldLogic { get; private set; }
		


		////////////////
		
		public override void Initialize() {
			var mymod = (HamstarHelpersMod)this.mod;

			this.ObsoleteID2 = WorldHelpers.GetUniqueId();
			this.ObsoleteID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;  // 'Load()' decides if no pre-existing one is found

			this.WorldLogic = new WorldLogic( mymod );

			if( String.IsNullOrEmpty(this.ObsoleteID2) ) {
				throw new Exception( "UID not defined." );
			}
		}


		internal void OnWorldExit() {
			this.HasCorrectID = false;
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoleteID = tags.GetString( "world_id" );
			}

			//mymod.UserHelpers.Load( mymod, tags );
			mymod.ModLockHelpers.Load( mymod, tags );
			this.WorldLogic.LoadForWorld( mymod, tags );

			mymod.ModLockHelpers.OnWorldLoad( mymod, this );
			//mymod.UserHelpers.OnWorldLoad( this );

			this.HasCorrectID = true;
		}

		public override TagCompound Save() {
			var mymod = (HamstarHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags.Set( "world_id", this.ObsoleteID );

			//mymod.UserHelpers.Save( mymod, tags );
			mymod.ModLockHelpers.Save( mymod, tags );
			this.WorldLogic.SaveForWorld( mymod, tags );

			return tags;
		}

		
		////////////////

		/*public override void NetSend( BinaryWriter writer ) {		<- TML's ModWorld.Net stuff is notoriously buggy!
			var mymod = (HamstarHelpersMod)this.mod;

			try {
				writer.Write( this.HasCorrectID );
				writer.Write( this.ObsoleteID );
			} catch( Exception e ) {
				LogHelpers.Log( e.ToString() );
			}
		}

		public override void NetReceive( BinaryReader reader ) {
			var mymod = (HamstarHelpersMod)this.mod;

			try {
				bool has_correct_id = reader.ReadBoolean();
				string id = reader.ReadString();

				if( has_correct_id ) {
					this.ObsoleteID = id;
					this.HasCorrectID = true;
				}
			} catch( Exception e ) {
				LogHelpers.Log( e.ToString() );
			}
		}*/

		////////////////

		public override void PreUpdate() {
			var mymod = (HamstarHelpersMod)this.mod;
			
			if( this.WorldLogic != null ) {
				if( Main.netMode == 0 ) { // Single
					this.WorldLogic.PreUpdateSingle( mymod );
				} else if( Main.netMode == 2 ) {
					this.WorldLogic.PreUpdateServer( mymod );
				}
			}
		}


		////////////////

		public override void PostDrawTiles() {
			Player player = Main.LocalPlayer;
			var mymod = (HamstarHelpersMod)this.mod;
			var myplayer = player.GetModPlayer<HamstarHelpersPlayer>( mymod );

			//Main.spriteBatch.Begin();
			RasterizerState rasterizer = Main.gameMenu ||
				(double)player.gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise :
					RasterizerState.CullClockwise;
			Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, (Effect)null, Main.GameViewMatrix.TransformationMatrix );

			try {
				mymod.CustomEntMngr.DrawAll( Main.spriteBatch );
			} catch( Exception e ) {
				ErrorLogger.Log( "HamstarHelpersWorld.PostDrawTiles - " + e.ToString() );
				throw e;
			}

			Main.spriteBatch.End();
		}
	}
}
