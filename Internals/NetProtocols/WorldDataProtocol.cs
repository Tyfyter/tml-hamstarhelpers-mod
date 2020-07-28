﻿using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.Network.NetIO;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class WorldDataRequestProtocol : NetProtocolRequestServerPayload<WorldDataProtocol> {
		public static void QuickRequest() {
			NetIO.RequestFromServer( new WorldDataRequestProtocol() );
		}
	}




	/// @private
	class WorldDataProtocol : NetProtocolClientPayload {
		public int HalfDays;
		public bool HasObsoletedWorldId;
		public string ObsoletedWorldId;



		////////////////

		private WorldDataProtocol() {
			var myworld = ModContent.GetInstance<ModHelpersWorld>();

			this.HalfDays = WorldStateHelpers.GetElapsedHalfDays();
			this.HasObsoletedWorldId = myworld.HasObsoleteId;
			this.ObsoletedWorldId = myworld.ObsoleteId;
		}


		////////////////

		public override void ReceiveOnClient( int fromWho ) {
			var mymod = ModHelpersMod.Instance;
			var myworld = ModContent.GetInstance<ModHelpersWorld>();
			var myplayer = (ModHelpersPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, ModHelpersMod.Instance, "ModHelpersPlayer" );

			myworld.HasObsoleteId = this.HasObsoletedWorldId;
			myworld.ObsoleteId = this.ObsoletedWorldId;

			mymod.WorldStateHelpers.LoadFromData( this.HalfDays, this.ObsoletedWorldId );

			myplayer.Logic.FinishWorldDataSyncOnLocal();
		}
	}
}
