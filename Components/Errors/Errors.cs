﻿using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Components.Errors {
	class HamstarExceptionManager {
		internal static readonly object MyLock = new object();

		internal readonly IDictionary<string, int> MsgCount = new Dictionary<string, int>();
	}




	public class HamstarException : Exception {
		public HamstarException( string msg ) : base( msg ) {
			this.Initialize( msg );
		}

		public HamstarException( string msg, Exception inner ) : base( msg, inner ) {
			this.Initialize( msg );
		}


		////////////////

		private void Initialize( string msg ) {
			string context = DebugHelpers.GetCurrentContext( 3 );
			var msgCount = ModHelpersMod.Instance.ExceptionMngr.MsgCount;
			int count = 0;

			lock( HamstarExceptionManager.MyLock ) {
				if( msgCount.TryGetValue( msg, out count ) ) {
					if( count > 10 && (Math.Log10( count ) % 1) != 0 ) {
						return;
					}
				} else {
					msgCount[msg] = 0;
				}
				msgCount[msg]++;
			}

			if( this.InnerException != null ) {
				LogHelpers.Log( "!"+context+" (E#" + count + ") - " + msg + " | " + this.InnerException.Message );
			} else {
				LogHelpers.Log( "!"+context+" (E#" + count + ") - " + msg );
			}
		}
	}
}
