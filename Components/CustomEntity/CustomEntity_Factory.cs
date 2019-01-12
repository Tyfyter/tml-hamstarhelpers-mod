﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected abstract class CustomEntityFactory<T> : Factory<T> where T : CustomEntity {
			public readonly Player OwnerPlayer;


			////////////////
			
			protected CustomEntityFactory( Player ownerPlr=null ) {
				this.OwnerPlayer = ownerPlr;
			}

			////////////////

			protected sealed override void Initialize( T ent ) {
				if( this.OwnerPlayer != null ) {
					ent.OwnerPlayerWho = this.OwnerPlayer.whoAmI;
					ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( this.OwnerPlayer );
				} else {
					ent.OwnerPlayerWho = -1;
					ent.OwnerPlayerUID = "";
				}

				ent.Core = ent.CreateCore( this );
				ent.Components = ent.CreateComponents( this );

				ent.OnInitialize();
			}
		}



		////////////////

		private new static CustomEntity CreateRawUninitialized( Type mytype ) {
			if( !mytype.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new NotImplementedException( mytype.Name+" is not a CustomEntity subclass." );
			}

			//var ent = (CustomEntity)PacketProtocolData.CreateRaw( mytype );
			var ent = (CustomEntity)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock() },
				null
			);

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = CustomEntity.CreateRawUninitialized( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";

			ent.OnInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, Player ownerPlr ) {
			var ent = CustomEntity.CreateRawUninitialized( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = ownerPlr.whoAmI;
			ent.OwnerPlayerUID = PlayerIdentityHelpers.GetProperUniqueId( ownerPlr );

			ent.OnInitialize();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string ownerUid="" ) {
			var ent = CustomEntity.CreateRawUninitialized( mytype );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = ownerUid;
			
			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( ownerUid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.OnInitialize();

			return ent;
		}
	}
}