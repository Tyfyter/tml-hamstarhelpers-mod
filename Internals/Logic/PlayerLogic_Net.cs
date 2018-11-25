﻿using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Internals.Logic {
	class PlayerLogicPromiseArguments : PromiseArguments {
		public int Who;
	}




	partial class PlayerLogic {
		public void OnSingleConnect( ModHelpersMod mymod, Player player ) {
			if( !this.HasLoadedUID ) {
				LogHelpers.Log( "!ModHelpers.PlayerLogic.OnSingleConnect - No UID for " + player.name + " (" + player.whoAmI + ")" );
				this.HasLoadedUID = true; // Ugly failsafe
			}

			if( !mymod.ConfigJson.LoadFile() ) {
				mymod.ConfigJson.SaveFile();
			}
			
			this.FinishModSettingsSync();
			this.FinishWorldDataSync();

			mymod.ControlPanel.LoadModListAsync();
		}

		public void OnClientConnect( ModHelpersMod mymod, Player player ) {
			if( !this.HasLoadedUID ) {
				LogHelpers.Log( "!ModHelpers.PlayerLogic.OnCurrentClientConnect - No UID for " + player.name + " (" + player.whoAmI + ") to send to server" );
				this.HasLoadedUID = true;	// Ugly failsafe
			}

			// Send
			PacketProtocolSendToServer.QuickSend<PlayerOldIdProtocol>();
			PlayerDataProtocol.SyncToEveryone( this.PermaBuffsById, this.HasBuffIds, this.EquipSlotsToItemTypes );

			// Receive
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>();
			PacketProtocolRequestToServer.QuickRequest<WorldDataProtocol>();

			mymod.ControlPanel.LoadModListAsync();
			
			Promises.AddValidatedPromise<PlayerLogicPromiseArguments>( PlayerLogic.ClientConnectValidator, ( args ) => {
				if( Main.netMode != 1 ) {
					PacketProtocolSendToClient.QuickSend<CustomEntityAllProtocol>( args.Who, -1 );
				}
				return true;
			} );
		}

		public void OnServerConnect( ModHelpersMod mymod, Player player ) {
			this.FinishModSettingsSync();
			this.FinishWorldDataSync();

			var args = new PlayerLogicPromiseArguments { Who = player.whoAmI };

			Promises.TriggerValidatedPromise( PlayerLogic.ServerConnectValidator, PlayerLogic.MyValidatorKey, args );
		}


		////////////////

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
			if( this.HasSyncedState() ) { this.FinishSync(); }
		}
		public void FinishWorldDataSync() {
			this.HasSyncedModData = true;
			if( this.HasSyncedState() ) { this.FinishSync(); }
		}

		public bool HasSyncedState() {
			return this.HasSyncedModSettings && this.HasSyncedModData && this.HasLoadedUID;
		}

		private void FinishSync() {
			if( this.IsSynced ) { return; }
			this.IsSynced = true;
		}


		////////////////

		public void NetReceiveDataClient( ISet<int> perma_buff_ids, ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}

		public void NetReceiveDataServer( ISet<int> perma_buff_ids, ISet<int> has_buff_ids, IDictionary<int, int> equip_slots_to_item_types ) {
			this.PermaBuffsById = perma_buff_ids;
			this.HasBuffIds = has_buff_ids;
			this.EquipSlotsToItemTypes = equip_slots_to_item_types;
		}


		public void NetReceiveIdServer( bool has_uid, string uid ) {
			this.HasLoadedUID = has_uid;
			this.PrivateUID = uid;
		}
	}
}
