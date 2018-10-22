﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using Terraria;
using HamstarHelpers.Helpers.PlayerHelpers;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.NetProtocols {
	class PlayerNewIdProtocol : PacketProtocol {
		public IDictionary<int, string> PlayerIds;



		////////////////

		private PlayerNewIdProtocol( PacketProtocolDataConstructorLock ctor_lock ) {
			this.PlayerIds = ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds;
		}


		////////////////

		protected override void SetClientDefaults() {
			this.PlayerIds[ Main.myPlayer ] = PlayerIdentityHelpers.GetMyProperUniqueId();
		}

		protected override void SetServerDefaults( int to_who ) {
		}


		////////////////

		protected override void ReceiveWithServer( int from_who ) {
			var kv = this.PlayerIds.Single();
			this.PlayerIds[ kv.Key ] = kv.Value;

			this.SendToClient( -1, -1 );
		}

		protected override void ReceiveWithClient() {
			ModHelpersMod.Instance.PlayerIdentityHelpers.PlayerIds = this.PlayerIds;
		}
	}
}