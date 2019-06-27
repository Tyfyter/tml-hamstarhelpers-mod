﻿using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Helpers.Projectiles;
using HamstarHelpers.Helpers.Tiles;
using Microsoft.Xna.Framework;
using Terraria;


namespace HamstarHelpers.Helpers.Entities {
	/** <summary>Assorted static "helper" functions pertaining to `Entity`s (parent class of Item, NPC, Player, and Projectile).</summary> */
	public static class EntityHelpers {
		public static int GetVanillaSnapshotHash( Entity ent, bool noContext ) {
			int hash = ("active"+ent.active).GetHashCode();

			if( !noContext ) {
				//hash ^= ("position"+ent.position).GetHashCode();
				//hash ^= ("velocity"+ent.velocity).GetHashCode();
				//hash ^= ("oldPosition"+ent.oldPosition).GetHashCode();
				//hash ^= ("oldVelocity"+ent.oldVelocity).GetHashCode();
				//hash ^= ("oldDirection"+ent.oldDirection).GetHashCode();
				//hash ^= ("direction"+ent.direction).GetHashCode();
				hash ^= ("whoAmI"+ent.whoAmI).GetHashCode();
				//hash ^= ("wet"+ent.wet).GetHashCode();
				//hash ^= ("honeyWet"+ent.honeyWet).GetHashCode();
				//hash ^= ("wetCount"+ent.wetCount).GetHashCode();
				//hash ^= ("lavaWet"+ent.lavaWet).GetHashCode();
			}
			hash ^= ("width"+ent.width).GetHashCode();
			hash ^= ("height"+ent.height).GetHashCode();
			
			return hash;
		}


		public static void ApplyForce( Entity ent, Vector2 worldPosFrom, float force ) {
			Vector2 offset = worldPosFrom - ent.position;
			Vector2 forceVector = Vector2.Normalize( offset ) * force;
			ent.velocity += forceVector;
		}


		public static bool SimpleLineOfSight( Vector2 position, Entity to ) {
			var trace = new Utils.PerLinePoint( delegate ( int tileX, int tileY ) {
				return !TileHelpers.IsSolid( Framing.GetTileSafely( tileX, tileY ) );
			} );
			return Utils.PlotTileLine( position, to.position, 1, trace );
		}


		////////////////

		public static string GetQualifiedName( Entity ent ) {
			if( ent is Item ) {
				return ItemIdentityHelpers.GetQualifiedName( (Item)ent );
			}
			if( ent is NPC ) {
				return NPCIdentityHelpers.GetQualifiedName( (NPC)ent );
			}
			if( ent is Projectile ) {
				return ProjectileIdentityHelpers.GetQualifiedName( (Projectile)ent );
			}
			if( ent is Player ) {
				return ( (Player)ent ).name;
			}
			return "...a "+ent.GetType().Name;
		}
	}
}