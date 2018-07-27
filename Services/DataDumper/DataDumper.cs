﻿using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using HamstarHelpers.Internals.NetProtocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.DataDumper {
	public static class DataDumper {
		private static object MyLock = new object();
		private static object MyDataStorekey = new object();
		private static int Dumps = 0;



		////////////////

		internal static IDictionary<string, Func<string>> GetDumpables() {
			bool success;
			var dumpables = (IDictionary<string, Func<string>>)DataStore.DataStore.Get( DataDumper.MyDataStorekey, out success );

			lock( DataDumper.MyLock ) {
				if( !success ) {
					dumpables = new Dictionary<string, Func<string>>();
					DataStore.DataStore.Set( DataDumper.MyDataStorekey, dumpables );
				}
			}

			return dumpables;
		}


		////////////////

		internal static string GetFileName( string prefix ) {
			string netmode;
			switch( Main.netMode ) {
			case 0:
				netmode = "single";
				break;
			case 1:
				netmode = "client";
				break;
			case 2:
				netmode = "server";
				break;
			default:
				netmode = "!";
				break;
			}

			double now_seconds = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;

			string now_seconds_whole = ( (int)now_seconds ).ToString( "D6" );
			string now_seconds_decimal = ( now_seconds - (int)now_seconds ).ToString( "N2" );
			string now = now_seconds_whole + "." + ( now_seconds_decimal.Length > 2 ? now_seconds_decimal.Substring( 2 ) : now_seconds_decimal );

			return prefix + "_"+netmode +"_"+now + "_dump.json";
		}


		public static void SetDumpSource( string name, Func<string> dump ) {
			var dumpables = DataDumper.GetDumpables();

			lock( DataDumper.MyLock ) {
				dumpables[ name ] = dump;
			}
		}


		////////////////

		public static string DumpToFile() {
			IDictionary<string, string> data;
			IDictionary<string, Func<string>> dumpables = DataDumper.GetDumpables();

			Func<KeyValuePair<string, Func<string>>, string> getKey = ( kv ) => {
				try { return kv.Value(); }
				catch { return "ERROR"; }
			};

			lock( DataDumper.MyLock ) {
				data = dumpables.ToDictionary( kv => kv.Key, getKey );
			}
			
			string file_name = DataDumper.GetFileName( (DataDumper.Dumps++)+"" );
			string rel_path = "Logs" + Path.DirectorySeparatorChar + "Dumps";
			var json_file = new JsonConfig<IDictionary<string, string>>( file_name, rel_path, data );

			json_file.SaveFile();

			// Allow admins to dump on behalf of server, also
			if( Main.netMode == 1 ) {
				bool success;
				if( UserHelpers.HasBasicServerPrivilege( Main.LocalPlayer, out success ) ) {
					PacketProtocol.QuickRequestToServer<DataDumpProtocol>();
				}
			}

			return file_name;
		}
	}
}
