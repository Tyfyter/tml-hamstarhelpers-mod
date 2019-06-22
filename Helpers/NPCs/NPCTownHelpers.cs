﻿using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;


namespace HamstarHelpers.Helpers.NPCs {
	/** <summary>Assorted static "helper" functions pertaining to NPC townsfolk.</summary> */
	public static class NPCTownHelpers {
		public static void Spawn( int townNpcType, int tileX, int tileY ) {
			int npcWho = NPC.NewNPC( tileX * 16, tileY * 16, townNpcType, 1, 0f, 0f, 0f, 0f, 255 );
			NPC npc = Main.npc[ npcWho ];

			Main.townNPCCanSpawn[ townNpcType ] = false;
			npc.homeTileX = tileX;
			npc.homeTileY = tileY;
			npc.homeless = true;

			if( tileX < WorldGen.bestX ) {
				npc.direction = 1;
			} else {
				npc.direction = -1;
			}

			npc.netUpdate = true;

			if( Main.netMode == 0 ) {
				Main.NewText( Language.GetTextValue( "Announcement.HasArrived", npc.FullName ), 50, 125, 255, false );
			} else if( Main.netMode == 2 ) {
				var msg = NetworkText.FromKey( "Announcement.HasArrived", new object[] { npc.GetFullNetName() } );
				NetMessage.BroadcastChatMessage( msg, new Color( 50, 125, 255 ), -1 );
			}

			//AchievementsHelper.NotifyProgressionEvent( 8 );
			//if( Main.npc[ npcWho ].type == 160 ) {
			//	AchievementsHelper.NotifyProgressionEvent( 18 );
			//}
		}


		////////////////

		public static void Leave( NPC npc, bool announce = true ) {
			if( Main.netMode == 1 ) {
				LogHelpers.Warn( "NPCTownHelpers.Leave() called on client." );
			}

			int whoami = npc.whoAmI;

			if( announce ) {
				string msg = Main.npc[whoami].GivenName + " the " + Main.npc[whoami].TypeName + " " + Lang.misc[35];

				if( Main.netMode == 0 ) {
					Main.NewText( msg, 50, 125, 255, false );
				} else if( Main.netMode == 1 ) {
					//NetMessage.SendChatMessageFromClient( new ChatMessage( msg ) );
				} else if( Main.netMode == 2 ) {
					NetMessage.BroadcastChatMessage( NetworkText.FromLiteral( msg ), new Color( 255, 50, 125 ) );
					//NetMessage.SendData( MessageID.ChatText, -1, -1, NetworkText.FromLiteral( msg ), 255, 50f, 125f, 255f, 0, 0, 0 );
				}
			}
			Main.npc[whoami].active = false;
			Main.npc[whoami].netSkip = -1;
			Main.npc[whoami].life = 0;
			NetMessage.SendData( 23, -1, -1, null, whoami, 0f, 0f, 0f, 0, 0, 0 );
		}


		////////////////

		public static Item[] GetCurrentShop() {
			if( Main.npcShop <= 0 || Main.npcShop > Main.instance.shop.Length ) {
				return null;
			}
			return Main.instance.shop[Main.npcShop].item;
		}


		public static Chest GetShop( int npcType ) {
			if( Main.instance == null ) {
				LogHelpers.Log( "No main instance." );
				return null;
			}

			switch( npcType ) {
			case NPCID.Merchant:
				return Main.instance.shop[1];
			case NPCID.ArmsDealer:
				return Main.instance.shop[2];
			case NPCID.Dryad:
				return Main.instance.shop[3];
			case NPCID.Demolitionist:
				return Main.instance.shop[4];
			case NPCID.Clothier:
				return Main.instance.shop[5];
			case NPCID.GoblinTinkerer:
				return Main.instance.shop[6];
			case NPCID.Wizard:
				return Main.instance.shop[7];
			case NPCID.Mechanic:
				return Main.instance.shop[8];
			case NPCID.SantaClaus:
				return Main.instance.shop[9];
			case NPCID.Truffle:
				return Main.instance.shop[10];
			case NPCID.Steampunker:
				return Main.instance.shop[11];
			case NPCID.DyeTrader:
				return Main.instance.shop[12];
			case NPCID.PartyGirl:
				return Main.instance.shop[13];
			case NPCID.Cyborg:
				return Main.instance.shop[14];
			case NPCID.Painter:
				return Main.instance.shop[15];
			case NPCID.WitchDoctor:
				return Main.instance.shop[16];
			case NPCID.Pirate:
				return Main.instance.shop[17];
			case NPCID.Stylist:
				return Main.instance.shop[18];
			case NPCID.TravellingMerchant:
				return Main.instance.shop[19];
			case NPCID.SkeletonMerchant:
				return Main.instance.shop[20];
			case NPCID.DD2Bartender:
				return Main.instance.shop[21];
			}

			return null;
		}


		////////////////

		public static ISet<int> GetFemaleTownNpcTypes() {
			return new HashSet<int>( new int[] {
				NPCID.Nurse,
				NPCID.PartyGirl,
				NPCID.Stylist,
				NPCID.Dryad,
				NPCID.Mechanic,
				NPCID.Steampunker
			} );
		}

		public static ISet<int> GetNonGenderedTownNpcTypes() {
			return new HashSet<int>( new int[] {
				NPCID.WitchDoctor,	// Indeterminable
				NPCID.Truffle,
				NPCID.Cyborg,	// Cyborgs still have fleshy bits?
				//NPCID.SkeletonMerchant	// Dialog suggests male in past life
			} );
		}
	}
}
