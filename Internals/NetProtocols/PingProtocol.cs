﻿using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Services.Network.NetIO.PayloadTypes;
using HamstarHelpers.Services.Network.NetIO;


namespace HamstarHelpers.Internals.NetProtocols {
	/// @private
	class PingProtocol : NetProtocolBidirectionalPayload {
		public static void QuickSendToServer() {
			NetIO.SendToServer( new PingProtocol() );
		}



		////////////////

		public long StartTime = -1;
		public long EndTime = -1;



		////////////////

		private PingProtocol() {
			this.StartTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;
		}


		////////////////

		public override void ReceiveOnServer( int fromWho ) {
			if( this.EndTime == -1 ) {
				NetIO.SendToClients( this, fromWho, -1 );
			} else {
				int delta = (int)( this.EndTime - this.StartTime );
				ModHelpersMod.Instance.NetHelpers.UpdatePing( delta );
			}
		}


		public override void ReceiveOnClient( int fromWho ) {
			this.EndTime = (long)SystemHelpers.TimeStamp().TotalMilliseconds;

			NetIO.SendToServer( this );

			if( this.StartTime != -1 ) {
				int delta = (int)( this.EndTime - this.StartTime );
				ModHelpersMod.Instance.NetHelpers.UpdatePing( delta );
			}
		}
	}
}
