﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected abstract class CustomEntityFactory<T> : Factory<T> where T : CustomEntity {
			protected readonly Player OwnerPlayer;


			////////////////
			
			protected CustomEntityFactory( Player owner_plr=null ) {
				this.OwnerPlayer = owner_plr;
			}

			////////////////

			public override void Initialize( T data ) {
				if( this.OwnerPlayer != null ) {
					data.OwnerPlayerWho = this.OwnerPlayer.whoAmI;
					data.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( this.OwnerPlayer );
				} else {
					data.OwnerPlayerWho = -1;
					data.OwnerPlayerUID = "";
				}

				data.Core = this.InitializeCore();
				data.Components = this.InitializeComponents();

				this.InitializeEntity( data );

				data.PostInitialize();
			}

			////

			protected abstract CustomEntityCore InitializeCore();
			protected abstract IList<CustomEntityComponent> InitializeComponents();
			protected abstract void InitializeEntity( T ent );
		}



		////////////////

		private new static CustomEntity CreateRaw( Type mytype ) {
			if( mytype.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new NotImplementedException( mytype.Name+" is not a CustomEntity subclass." );
			}

			var data = (CustomEntity)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( CustomEntity ) ) },
				null
			);

			return data;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";

			ent.PostInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = owner_plr.whoAmI;
			ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( owner_plr );

			ent.PostInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string owner_uid="" ) {
			var ent = CustomEntity.CreateRaw( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = owner_uid;

			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( owner_uid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.PostInitialize();

			return ent;
		}



		////////////////

		protected CustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) {
			if( !ctor_lock.FactoryType.IsSubclassOf( typeof(CustomEntityFactory<>) ) ) {
				if( ctor_lock.FactoryType != typeof(CustomEntity) ) {
					throw new NotImplementedException( "CustomEntity " + this.GetType().Name + " uses invalid factory " + ctor_lock.FactoryType.Name );
				}
			}
		}


		////////////////
		
		internal void RefreshOwnerWho() {
			if( Main.netMode == 1 ) {
				throw new HamstarException( "No client." );
			}

			if( string.IsNullOrEmpty( this.OwnerPlayerUID ) ) {
				this.OwnerPlayerWho = -1;
				return;
			}
			
			Player owner = PlayerIdentityHelpers.GetPlayerByProperId( this.OwnerPlayerUID );

			this.OwnerPlayerWho = owner == null ? -1 : owner.whoAmI;
		}


		////////////////

		private void CopyChangesFrom( CustomEntityCore core, IList<CustomEntityComponent> components, Player owner_plr ) {   // TODO: Actually copy changes only!
			//Type my_factory_type = this.GetMainFactoryType();
			//ConstructorInfo ctor = my_factory_type.GetConstructor( new Type[] {
			//	typeof(CustomEntityCore), typeof(IList<CustomEntityComponent>), typeof(string), typeof(CustomEntity)
			//} );
			//var new_ent = (CustomEntity)ctor.Invoke( new object[] { core, components, player_uid } );

			this.Core = new CustomEntityCore( core );
			this.OwnerPlayerWho = owner_plr != null ? owner_plr.whoAmI : -1;
			//this.OwnerPlayerUID = owner_plr != null ? PlayerIdentityHelpers.GetProperUniqueId(owner_plr) : "";

			this.Components = components.Select( c => c.InternalClone() ).ToList();

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();

			this.PostInitialize();
		}


		////////////////

		private void RefreshComponentTypeNames() {
			int comp_count = this.Components.Count;

			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();

			for( int i = 0; i < comp_count; i++ ) {
				Type comp_type = this.Components[i].GetType();
				string comp_name = comp_type.Name;

				this.ComponentsByTypeName[ comp_name ] = i;

				do {
					this.AllComponentsByTypeName[ comp_name ] = i;

					comp_type = comp_type.BaseType;
					comp_name = comp_type.Name;
				} while( comp_type.Name != "CustomEntityComponent" );
			}
		}
	}
}
