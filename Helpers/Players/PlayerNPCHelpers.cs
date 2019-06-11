﻿using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace HamstarHelpers.Helpers.Players {
	public static class PlayerNPCHelpers {
		public static bool IsPlayerNearBoss( Player player ) {
			int x = ((int)player.Center.X - (Main.maxScreenW / 2)) / 16;
			int y = ((int)player.Center.Y - (Main.maxScreenH / 2)) / 16;

			Rectangle playerZone = new Rectangle( x, y, (Main.maxScreenH / 16), (Main.maxScreenH / 16) );
			int bossRadius = 5000;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC checkNpc = Main.npc[i];
				if( !checkNpc.active || !checkNpc.boss ) { continue; }

				int npcLeft = (int)(checkNpc.position.X + (float)checkNpc.width / 2f) - bossRadius;
				int npcTop = (int)(checkNpc.position.Y + (float)checkNpc.height / 2f) - bossRadius;
				Rectangle npcZone = new Rectangle( npcLeft, npcTop, bossRadius * 2, bossRadius * 2 );

				if( playerZone.Intersects( npcZone ) ) { return true; }
			}

			return false;
		}


		////////////////

		public static bool HasUsedNurse( Player player ) {
			return Main.npcChatText == Lang.dialog( 227, false ) ||
					Main.npcChatText == Lang.dialog( 228, false ) ||
					Main.npcChatText == Lang.dialog( 229, false ) ||
					Main.npcChatText == Lang.dialog( 230, false );
		}
	}
}