﻿using System;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogHelpers {
		private static void DirectInfo( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Info( msg );
					//ErrorLogger.Log( logged + msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Info( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
						//ErrorLogger.Log( "FALLBACK LOGGER 2 (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}

		private static void DirectAlert( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Error( msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Error( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}

		private static void DirectWarn( string msg ) {
			ModHelpersMod mymod = ModHelpersMod.Instance;
			var logHelpers = mymod.LogHelpers;

			try {
				lock( LogHelpers.MyLock ) {
					mymod.Logger.Fatal( msg );
				}
			} catch( Exception e ) {
				try {
					lock( LogHelpers.MyLock ) {
						mymod.Logger.Fatal( "FALLBACK LOGGER (" + e.GetType().Name + ") " + msg );
					}
				} catch { }
			}
		}
	}
}