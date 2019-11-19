﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Projectiles.Attributes;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups.Definitions {
	/// <summary></summary>
	public partial class ProjectileGroupIDs {
		/// <summary></summary>
		public static readonly string AnyExplosive = "Any Explosive";
	}




	partial class EntityGroupDefs {
		internal static void DefineProjectileGroups1( IList<EntityGroupMatcherDefinition<Projectile>> defs ) {
			// General

			defs.Add( new EntityGroupMatcherDefinition<Projectile>(
				ProjectileGroupIDs.AnyExplosive, null,
				new ProjectileGroupMatcher( ( proj, grp ) => {
					int _;
					return ProjectileAttributeHelpers.IsExplosive( proj.type, out _, out _ );
				} )
			) );
		}
	}
}