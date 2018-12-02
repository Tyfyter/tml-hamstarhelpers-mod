﻿using HamstarHelpers.Components.Network.Data;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public abstract class HitRadiusProjectileEntityComponent : CustomEntityComponent {
		protected class HitRadiusProjectileEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : HitRadiusProjectileEntityComponent {
			public float Radius;

			public HitRadiusProjectileEntityComponentFactory( float radius ) {
				this.Radius = radius;
			}

			protected override void InitializeComponent( T data ) {
				data.Radius = this.Radius;
			}
		}



		////////////////

		public static HitRadiusProjectileEntityComponent CreateAttackableEntityComponent( float radius ) {
			var factory = new HitRadiusProjectileEntityComponentFactory<HitRadiusProjectileEntityComponent>( radius );
			return factory.Create();
		}


		////////////////
		
		public bool IsImmune = false;
		public float Radius;



		////////////////

		protected HitRadiusProjectileEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Projectile projectile, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Projectile projectile, int damage );
	}
}
