﻿using HamstarHelpers.Components.CustomEntity;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class CustomEntityAllProtocol : PacketProtocol {
		public CustomEntity[] Entities;



		////////////////

		private CustomEntityAllProtocol( PacketProtocolDataConstructorLock ctor_lock ) { }


		////////////////

		protected override void SetServerDefaults( int from_who ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<PeriodicSyncEntityComponent>();

			this.Entities = ents.ToArray();

			/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Log( "ModHelpers.CustomEntityAllProtocol.SetServerDefaults - Sending " + string.Join(",\n   ", this.Entities.Select(e=>e.ToString())) );
			}*/
		}


		////////////////

		protected override void ReceiveWithClient() {
			CustomEntityManager.ClearAllEntities();

			foreach( CustomEntity ent in this.Entities ) {
				/*if( ModHelpersMod.Instance.Config.DebugModeCustomEntityInfo ) {
					LogHelpers.Log( "ModHelpers.CustomEntityAllProtocol.ReceiveWithClient - New entity " + ent.ToString() );
				}*/

				CustomEntityManager.SetEntityByWho( ent.Core.whoAmI, ent );
			}

			SaveableEntityComponent.PostLoadAll();

			Promises.TriggerValidatedPromise( SaveableEntityComponent.LoadAllValidator, SaveableEntityComponent.MyValidatorKey, null );
		}
	}
}
