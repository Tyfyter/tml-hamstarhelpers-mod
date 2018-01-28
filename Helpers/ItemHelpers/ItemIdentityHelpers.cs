﻿using System.Collections.Generic;
using Terraria;
using Terraria.ID;


namespace HamstarHelpers.ItemHelpers {
	public partial class ItemIdentityHelpers {
		public const int HighestVanillaRarity = 11;
		public const int JunkRarity = -1;
		public const int QuestItemRarity = -11;


		////////////////

		private static IDictionary<int, int> ProjPene = new Dictionary<int, int>();

		public static bool IsPenetrator( Item item ) {
			if( item.shoot <= 0 ) { return false; }

			if( !ItemIdentityHelpers.ProjPene.Keys.Contains( item.shoot ) ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				ItemIdentityHelpers.ProjPene[item.shoot] = proj.penetrate;
			}

			return ItemIdentityHelpers.ProjPene[item.shoot] == -1 || ItemIdentityHelpers.ProjPene[item.shoot] >= 3;   // 3 seems fair?
		}


		public static bool IsTool( Item item ) {
			return (item.useStyle > 0 ||
					item.damage > 0 ||
					item.crit > 0 ||
					item.knockBack > 0 ||
					item.melee ||
					item.magic ||
					item.ranged ||
					item.thrown ||
					item.summon ||
					item.pick > 0 ||
					item.hammer > 0 ||
					item.axe > 0) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity &&
				item.type != 849;   // Actuators are not consumable, apparently
		}


		public static bool IsArmor( Item item ) {
			return (item.headSlot != -1 ||
					item.bodySlot != -1 ||
					item.legSlot != -1) &&
				!item.accessory &&
				!item.potion &&
				!item.consumable &&
				!item.vanity;
		}


		public static bool IsGrapple( Item item ) {
			return Main.projHook[item.shoot];
		}


		public static bool IsYoyo( Item item ) {
			if( item.shoot > 0 && item.useStyle == 5 && item.melee && item.channel ) {
				var proj = new Projectile();
				proj.SetDefaults( item.shoot );

				return proj.aiStyle == 99;
			}
			return false;
		}

		
		public static bool IsGameplayRelevant( Item item, bool toys_relevant=false, bool junk_relevant=false ) {
			if( !toys_relevant ) {
				switch( item.type ) {
				case ItemID.WaterGun:
				case ItemID.SlimeGun:
				case ItemID.BeachBall:
					return false;
				}
			}
			if( junk_relevant && item.rare < 0 ) { return false; }
			return !item.vanity && item.dye <= 0 && item.hairDye <= 0 && item.paint > 0 && !Main.vanityPet[ item.buffType ];
		}


		public static float LooselyAppraise( Item item ) {
			float appraisal = item.rare;
			if( item.value > 0 ) {
				float value = (float)item.value / 8000f;
				appraisal = ((appraisal * 4f) + value) / 5f;
			}
			return appraisal;
		}
	}
}
