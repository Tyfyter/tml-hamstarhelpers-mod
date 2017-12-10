﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers.TmlHelpers.Events {
	/*public class ItemEvents {
		public delegate void AltFunctionUseEvt( Item item, Player player );
		public delegate void AnglerChatEvt( int type, ref string chat, ref string catchLocation );
		public delegate void ArmorArmGlowMaskEvt( int slot, Player drawPlayer, float shadow, ref int glowMask, ref Color color );
		public delegate void ArmorSetShadowsEvt( Player player, string set );
		public delegate void CanEquipAccessoryEvt( Item item, Player player, int slot );
		public delegate void CanHitNPCEvt( Item item, Player player, NPC target );
		public delegate void CanHitPvpEvt( Item item, Player player, Player target );
		public delegate void CanPickupEvt( Item item, Player player );
		public delegate void CanRightClickEvt( Item item );
		public delegate void CanUseItemEvt( Item item, Player player );
		public delegate void CaughtFishStackEvt( int type, ref int stack );
		public delegate void ConsumeAmmoEvt( Item item, Player player );
		public delegate void ConsumeItemEvt( Item item, Player player );
		public delegate void DrawArmorColorEvt( EquipType type, int slot, Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor );
		public delegate void DrawBodyEvt( int body );
		public delegate void DrawHairEvt( int head, ref bool drawHair, ref bool drawAltHair );
		public delegate void DrawHandsEvt( int body, ref bool drawHands, ref bool drawArms );
		public delegate void DrawHeadEvt( int head );
		public delegate void DrawLegsEvt( int legs, int shoes );
		public delegate void ExtractinatorUseEvt( int extractType, ref int resultType, ref int resultStack );
		public delegate void GetAlphaEvt( Item item, Color lightColor );
		public delegate void GetWeaponCritEvt( Item item, Player player, ref int crit );
		public delegate void GetWeaponDamageEvt( Item item, Player player, ref int damage );
		public delegate void GetWeaponKnockbackEvt( Item item, Player player, ref float knockback );
		public delegate void GrabRangeEvt( Item item, Player player, ref int grabRange );
		public delegate void GrabStyleEvt( Item item, Player player );
		public delegate void HoldItemEvt( Item item, Player player );
		public delegate void HoldItemFrameEvt( Item item, Player player );
		public delegate void HoldoutOffsetEvt( int type );
		public delegate void HoldoutOriginEvt( int type );
		public delegate void HoldStyleEvt( Item item, Player player );
		public delegate void HorizontalWingSpeedsEvt( Item item, Player player, ref float speed, ref float acceleration );
		public delegate void IsAnglerQuestAvailableEvt( int type );
		public delegate void IsArmorSetEvt( Item head, Item body, Item legs );
		public delegate void IsVanitySetEvt( int head, int body, int legs );
		public delegate void ItemSpaceEvt( Item item, Player player );
		public delegate void LoadEvt( Item item, TagCompound tag );
		public delegate void LoadLegacyEvt( Item item, BinaryReader reader );
		public delegate void MeleeEffectsEvt( Item item, Player player, Rectangle hitbox );
		public delegate void MeleeSpeedMultiplierEvt( Item item, Player player );
		public delegate void ModifyHitNPCEvt( Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit );
		public delegate void ModifyHitPvpEvt( Item item, Player player, Player target, ref int damage, ref bool crit );
		public delegate void ModifyTooltipsEvt( Item item, List<TooltipLine> tooltips );
		public delegate void NeedsSavingEvt( Item item );
		public delegate void NetReceiveEvt( Item item, BinaryReader reader );
		public delegate void NetSendEvt( Item item, BinaryWriter writer );
		public delegate void OnCraftEvt( Item item, Recipe recipe );
		public delegate void OnHitNPCEvt( Item item, Player player, NPC target, int damage, float knockBack, bool crit );
		public delegate void OnHitPvpEvt( Item item, Player player, Player target, int damage, bool crit );
		public delegate void OnPickupEvt( Item item, Player player );
		public delegate void OpenVanillaBagEvt( string context, Player player, int arg );
		public delegate void PickAmmoEvt( Item item, Player player, ref int type, ref float speed, ref int damage, ref float knockback );
		public delegate void PostDrawInInventoryEvt( Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale );
		public delegate void PostDrawInWorldEvt( Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI );
		public delegate void PostDrawTooltipEvt( Item item, ReadOnlyCollection<DrawableTooltipLine> lines );
		public delegate void PostDrawTooltipLineEvt( Item item, DrawableTooltipLine line );
		public delegate void PostReforgeEvt( Item item );
		public delegate void PostUpdateEvt( Item item );
		public delegate void PreDrawInInventoryEvt( Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale );
		public delegate void PreDrawInWorldEvt( Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI );
		public delegate void PreDrawTooltipEvt( Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y );
		public delegate void PreDrawTooltipLineEvt( Item item, DrawableTooltipLine line, ref int yOffset );
		public delegate void PreOpenVanillaBagEvt( string context, Player player, int arg );
		public delegate void PreReforgeEvt( Item item );
		public delegate void PreUpdateVanitySetEvt( Player player, string set );
		public delegate void RightClickEvt( Item item, Player player );
		public delegate void SaveEvt( Item item );
		public delegate void SetDefaultsEvt( Item item );
		public delegate void SetMatchEvt( int armorSlot, int type, bool male, ref int equipSlot, ref bool robes );
		public delegate void ShootEvt( Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack );
		public delegate void UpdateEvt( Item item, ref float gravity, ref float maxFallSpeed );
		public delegate void UpdateAccessoryEvt( Item item, Player player, bool hideVisual );
		public delegate void UpdateArmorSetEvt( Player player, string set );
		public delegate void UpdateEquipEvt( Item item, Player player );
		public delegate void UpdateInventoryEvt( Item item, Player player );
		public delegate void UpdateVanitySetEvt( Player player, string set );
		public delegate void UseItemEvt( Item item, Player player );
		public delegate void UseItemFrameEvt( Item item, Player player );
		public delegate void UseItemHitboxEvt( Item item, Player player, ref Rectangle hitbox, ref bool noHitbox );
		public delegate void UseStyleEvt( Item item, Player player );
		public delegate void UseTimeMultiplierEvt( Item item, Player player );
		public delegate void VerticalWingSpeedsEvt( Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend );
		public delegate void WingUpdateEvt( int wings, Player player, bool inUse );
	}*/
}