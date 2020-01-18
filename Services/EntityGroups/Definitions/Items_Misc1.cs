﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items.Attributes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ItemGroupIDs {
		/// <summary></summary>
		public const string AnyItem = "Any Item";
		//"Any Rainbow 2 Tier", null,
		//"Any Rainbow Tier", null,
		//"Any Amber Tier", null,
		//"Any Grey Tier", null,
		//"Any White Tier", null,
		//"Any Blue Tier", null,
		//"Any Green Tier", null,
		//"Any Orange Tier", null,
		//"Any Light Red Tier", null,
		//"Any Pink Tier", null,
		//"Any Light Purple Tier", null,
		//"Any Lime Tier", null,
		//"Any Yellow Tier", null,
		//"Any Cyan Tier", null,
		//"Any Red Tier", null,
		//"Any Purple Tier", null,
		//"Any Dye", null,
		//"Any Food", null,
	}




	partial class EntityGroupDefs {
		internal static void DefineItemMiscGroups1( IList<EntityGroupMatcherDefinition<Item>> defs ) {
			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: ItemGroupIDs.AnyItem,
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return true;
				} )
			) );

			for( int i = -12; i <= ItemRarityAttributeHelpers.HighestVanillaRarity; i++ ) {
				if( i >= -10 && i <= -3 ) { i = -2; }

				int tier = i;
				defs.Add( new EntityGroupMatcherDefinition<Item>(
					grpName: "Any " + ItemRarityAttributeHelpers.RarityColorText[i] + " Tier",
					grpDeps: null,
					matcher: new ItemGroupMatcher( ( item, grps ) => {
						return item.rare == tier;
					} )
				) );
			}

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Dye",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.dye != 0 || item.hairDye != -1;
				} )
			) );

			defs.Add( new EntityGroupMatcherDefinition<Item>(
				grpName: "Any Food",
				grpDeps: null,
				matcher: new ItemGroupMatcher( ( item, grps ) => {
					return item.buffType == BuffID.WellFed;
				} )
			) );
		}
	}
}
