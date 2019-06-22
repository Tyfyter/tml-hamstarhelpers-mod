﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;


namespace HamstarHelpers.Internals.WebRequests {
	/** @private */
	public struct PostModTagsData {
		public string modname;
		public string modtags;
	}



	/** @private */
	class PostModInfo {
		public static void SubmitModInfo( string modName, ISet<string> modTags, Action<string> onSuccess, Action<Exception, string> onError, Action onCompletion=null ) {
			string url = "http://hamstar.pw/hamstarhelpers/mod_info_submit/";
			var json = new PostModTagsData {
				modname = modName,
				modtags = string.Join(",", modTags)
			};

			string jsonStr = JsonConvert.SerializeObject( json, Formatting.Indented );
			byte[] jsonBytes = Encoding.UTF8.GetBytes( jsonStr );

			Action<String> onResponse = ( output ) => {
				JObject respJson = JObject.Parse( output );
				JToken msg = respJson.SelectToken( "Msg" );

				if( msg == null ) {
					onSuccess( "Failure." );
				} else {
					onSuccess( msg.ToObject<string>() );
				}
			};

			Action<Exception, string> wrappedOnError = ( Exception e, string str ) => {
				LogHelpers.Log( "!ModHelpers.PostModInfo.SubmitModInfo - Failed for post: "+jsonStr );
				onError( e, str );
			};

			NetPlayHelpers.MakePostRequestAsync( url, jsonBytes, onResponse, wrappedOnError, onCompletion );
		}
	}
}
