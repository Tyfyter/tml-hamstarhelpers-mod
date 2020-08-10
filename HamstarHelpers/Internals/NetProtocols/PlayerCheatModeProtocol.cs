﻿using System;
using Terraria;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Services.Cheats;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	[Serializable]
	class PlayerCheatModeProtocol : NetProtocolBroadcastPayload {
		public static void BroadcastFromClient( CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, Main.myPlayer );
			NetIO.Broadcast( protocol );
		}

		public static void BroadcastToClients( Player player, CheatModeType cheatFlags ) {
			var protocol = new PlayerCheatModeProtocol( cheatFlags, player.whoAmI );
			NetIO.SendToClients( protocol, player.whoAmI );
		}



		////////////////

		public int CheatFlags;
		public int PlayerWho;



		////////////////

		public PlayerCheatModeProtocol() { }

		private PlayerCheatModeProtocol( CheatModeType cheatFlags, int playerWho ) {
			this.CheatFlags = (int)cheatFlags;
			this.PlayerWho = playerWho;
		}


		////////////////

		public override bool ReceiveOnServerBeforeRebroadcast( int fromWho ) {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[fromWho] );
			myplayer.Logic.SetCheats( (CheatModeType)this.CheatFlags );
			return true;
		}

		public override void ReceiveBroadcastOnClient() {
			var myplayer = TmlHelpers.SafelyGetModPlayer<ModHelpersPlayer>( Main.player[this.PlayerWho] );
			myplayer.Logic.SetCheats( (CheatModeType)this.CheatFlags );
		}
	}
}
