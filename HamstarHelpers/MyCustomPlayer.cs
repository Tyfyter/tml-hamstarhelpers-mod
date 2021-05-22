﻿using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.PlayerData;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.TModLoader;


namespace HamstarHelpers {
	/// @private
	class ModHelpersCustomPlayer : CustomPlayerData {
		//public PlayerLogic Logic { get; private set; }



		////////////////
		
		protected ModHelpersCustomPlayer() { }


		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( Main.netMode != NetmodeID.Server ) {
				if( !isCurrentPlayer ) {
					return;
				}
			}

			if( Main.netMode == NetmodeID.SinglePlayer ) {
				var myplayer = TmlLibraries.SafelyGetModPlayer<ModHelpersPlayer>( this.Player );
				myplayer.Logic.OnSingleEnterWorld( this.Player );
			} else if( Main.netMode == NetmodeID.MultiplayerClient ) {
				var myplayer = TmlLibraries.SafelyGetModPlayer<ModHelpersPlayer>( this.Player );
				myplayer.Logic.OnCurrentClientEnterWorld( this.Player );
			} else if( Main.netMode == NetmodeID.Server ) {
			}
		}
	}
}
