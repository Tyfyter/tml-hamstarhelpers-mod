﻿using System;
using Terraria.ID;

using NPCMatcher = System.Func<Terraria.NPC, System.Collections.Generic.IDictionary<string, System.Collections.Generic.ISet<int>>, bool>;


namespace HamstarHelpers.Services.EntityGroups {
	public partial class EntityGroups {
		private void DefineNPCGroups1( Action<string, string[], NPCMatcher> add_def ) {
			// General

			add_def( "Any Friendly NPC", null, ( npc, grp ) => {
				return npc.friendly;
			} );
			add_def( "Any Hostile NPC", null, ( npc, grp ) => {
				return !npc.friendly;
			} );
			add_def( "Any Town NPC", null, ( npc, grp ) => {
				return npc.townNPC;
			} );

			// Monsters

			add_def( "Any Slime", null, ( npc, grp ) => {
				if( npc.aiStyle == 1 ) {
					switch( npc.netID ) {
					case NPCID.HoppinJack:	//?
					case NPCID.Grasshopper:
					case NPCID.GoldGrasshopper:
						return false;
					}
					return true;
				} else {
					switch( npc.netID ) {
					case NPCID.Slimer:
					case NPCID.Slimer2:
					case NPCID.Gastropod:
						return true;
					}
				}
				return false;
			} );
		}
	}
}
