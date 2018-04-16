﻿using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.MiscHelpers;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Utilities.Messages {
	public class InboxMessages {
		public static void SetMessage( string which, string msg, bool force_unread, Action<bool> on_run=null ) {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox;

			inbox.Messages[which] = msg;
			inbox.MessageActions[which] = on_run;

			if( inbox.Messages.ContainsKey( which ) ) {
				if( force_unread ) {
					inbox.Order.Remove( which );
				} else {
					return;
				}
			}
			
			inbox.Order.Add( which );
		}


		public static string DequeueMessage() {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox;
			
			if( inbox.Current >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ inbox.Current++ ];
			string msg = inbox.Messages[ which ];

			return msg;
		}


		public static string GetMessageAt( int i, out bool is_unread ) {
			InboxMessages inbox = HamstarHelpersMod.Instance.Inbox;
			is_unread = false;

			if( i < 0 || i >= inbox.Order.Count ) {
				return null;
			}

			string which = inbox.Order[ i ];
			string msg = inbox.Messages[ which ];

			is_unread = i >= inbox.Current;

			return msg;
		}



		////////////////

		private IDictionary<string, string> Messages = new Dictionary<string, string>();
		private IDictionary<string, Action<bool>> MessageActions = new Dictionary<string, Action<bool>>();
		private IList<string> Order = new List<string>();
		public int Current { get; private set; }


		////////////////

		internal InboxMessages() {
			this.Current = 0;

			bool success;
			InboxMessages inbox_copy = this.LoadFromFile( out success );

			if( success ) {
				this.Messages = inbox_copy.Messages;
				this.MessageActions = inbox_copy.MessageActions;
				this.Order = inbox_copy.Order;
				this.Current = inbox_copy.Current;
			}
		}
		
		~InboxMessages() {
			this.SaveToFile();
		}


		////////////////

		internal InboxMessages LoadFromFile( out bool success ) {
			string pid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out success );
			if( !success ) {
				return null;
			}

			return DataFileHelpers.LoadBinary<InboxMessages>( HamstarHelpersMod.Instance, "Inbox_" + pid, out success );
		}

		internal void SaveToFile() {
			bool success;
			string pid = PlayerIdentityHelpers.GetUniqueId( Main.LocalPlayer, out success );
			if( !success ) {
				return;
			}

			DataFileHelpers.SaveAsBinary<InboxMessages>( HamstarHelpersMod.Instance, "Inbox_" + pid, this );
		}
	}
}