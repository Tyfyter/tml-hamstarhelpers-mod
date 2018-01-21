﻿using HamstarHelpers.NetProtocol;
using HamstarHelpers.TmlHelpers;
using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersPlayer : ModPlayer {
		public string UID { get; private set; }
		public ISet<int> PermaBuffsById { get; private set; }
		internal string ControlPanelNewSince = "1.0.0";

		private ISet<int> HasBuffIds = new HashSet<int>();
		private IDictionary<int, int> EquipSlotsToItemTypes = new Dictionary<int, int>();

		public bool HasSyncedModSettings { get; private set; }
		public bool HasSyncedPlayerData { get; private set; }
		public bool HasSyncedModData { get; private set; }



		////////////////

		public HamstarHelpersPlayer() {
			this.PermaBuffsById = new HashSet<int>();
		}

		public override void Initialize() {
			this.UID = Guid.NewGuid().ToString( "D" );
			this.HasSyncedModSettings = false;
			this.HasSyncedPlayerData = false;
			this.HasSyncedModData = false;
			this.PermaBuffsById = new HashSet<int>();
		}

		////////////////

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (HamstarHelpersPlayer)client_clone;
			clone.UID = this.UID;
			clone.PermaBuffsById = this.PermaBuffsById;
			clone.HasBuffIds = this.HasBuffIds;
			clone.EquipSlotsToItemTypes = this.EquipSlotsToItemTypes;
			clone.HasSyncedModSettings = this.HasSyncedModSettings;
			clone.HasSyncedPlayerData = this.HasSyncedPlayerData;
			clone.HasSyncedModData = this.HasSyncedModData;
		}
		
		public override void SendClientChanges( ModPlayer client_player ) {
			var myclient = (HamstarHelpersPlayer)client_player;

			if( (Main.netMode == 2 && !myclient.UID.Equals(this.UID)) || !myclient.PermaBuffsById.SetEquals(this.PermaBuffsById) ) {
//LogHelpers.Log( "SendClientChanges to: " + ( Main.netMode == 2 && !myclient.UID.Equals( this.UID ) ) + "|"+ myclient.PermaBuffsById.SetEquals( this.PermaBuffsById ) + ", client: "+ client_player.player.whoAmI+ ", whoAmI: "+this.player.whoAmI );
				ClientPacketHandlers.SendPlayerData( (HamstarHelpersMod)this.mod, -1 );
			}
		}

		public override void OnEnterWorld( Player player ) {
//LogHelpers.Log( "OnEnterWorld player: " + player.whoAmI+ ", me: "+this.player.whoAmI );
			var mymod = (HamstarHelpersMod)this.mod;

			if( player.whoAmI == this.player.whoAmI ) {
				if( Main.netMode == 0 ) {   // Single player only
					if( !mymod.JsonConfig.LoadFile() ) {
						mymod.JsonConfig.SaveFile();
					}
				}

				// Sync mod (world) data; must be called after world is loaded
				if( Main.netMode == 1 ) {
					ClientPacketHandlers.SendPlayerData( mymod, -1 );
					ClientPacketHandlers.SendRequestPlayerData( mymod, -1 );
					ClientPacketHandlers.SendRequestModSettings( mymod );
					ClientPacketHandlers.SendRequestModData( mymod );
				}

				if( Main.netMode != 1 ) {   // NOT client; clients won't receive their own data back from server
					this.FinishPlayerDataSync();
					this.FinishModSettingsSync();
					this.FinishModDataSync();
				}
			}
		}


		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
		}
		public void FinishPlayerDataSync() {
			this.HasSyncedPlayerData = true;
		}
		public void FinishModDataSync() {
			this.HasSyncedModData = true;
		}


		////////////////

		public void NetSend( BinaryWriter writer, bool include_uid=true ) {
			if( include_uid ) {
				writer.Write( (string)this.UID );
			}

			writer.Write( (int)this.PermaBuffsById.Count );

			foreach( int buff_id in this.PermaBuffsById ) {
				writer.Write( (int)buff_id );
			}
		}

		public void NetReceive( BinaryReader reader, bool include_uid = true ) {
			this.PermaBuffsById = new HashSet<int>();

			if( include_uid ) {
				this.UID = reader.ReadString();
			}

			int perma_buff_id_count = reader.ReadInt32();

			for( int i = 0; i < perma_buff_id_count; i++ ) {
				this.PermaBuffsById.Add( reader.ReadInt32() );
			}
		}


		////////////////

		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey( "uid" ) ) {
				this.UID = tags.GetString( "uid" );
			}
			if( tags.ContainsKey( "cp_new_since" ) ) {
				this.ControlPanelNewSince = tags.GetString( "cp_new_since" );
			}
			if( tags.ContainsKey( "perma_buffs" ) ) {
				var perma_buffs = tags.GetList<int>( "perma_buffs" );
				this.PermaBuffsById = new HashSet<int>( perma_buffs.ToArray() );
			}
		}

		public override TagCompound Save() {
			var perma_buffs = this.PermaBuffsById.ToArray();

			TagCompound tags = new TagCompound {
				{ "uid", this.UID },
				{ "cp_new_since", this.ControlPanelNewSince },
				{ "perma_buffs", perma_buffs }
			};
			return tags;
		}


		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {	// Current player
				PlayerMessage.UpdatePlayerLabels();
				SimpleMessage.UpdateMessage();
			}

			var mymod = (HamstarHelpersMod)this.mod;
			var modworld = mymod.GetModWorld<HamstarHelpersWorld>();

			if( Main.netMode == 1 ) {   // Client only
				if( this.player.whoAmI == Main.myPlayer ) { // Current player only
					modworld.WorldLogic.Update( mymod );
				}
			} else if( Main.netMode == 2 ) {    // Server
				modworld.WorldLogic.IsServerPlaying = true;	// Needed?
			}

			foreach( int buff_id in this.PermaBuffsById ) {
				this.player.AddBuff( buff_id, 3 );
			}

			this.CheckBuffHooks();
			this.CheckArmorEquipHooks();
		}


		////////////////

		public override void ProcessTriggers( TriggersSet triggers_set ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( mymod.ControlPanelHotkey.JustPressed ) {
				if( mymod.Config.DisableControlPanel ) {
					Main.NewText( "Control panel disabled.", Color.Red );
				} else {
					mymod.ControlPanel.Open();
				}
			}
		}


		////////////////

		private void CheckBuffHooks() {
			// Add new buffs
			for( int i = 0; i < this.player.buffTime.Length; i++ ) {
				if( this.player.buffTime[i] > 0 ) {
					int buff_id = this.player.buffType[i];
					if( !this.HasBuffIds.Contains( buff_id ) ) {
						this.HasBuffIds.Add( buff_id );
					}
				}
			}

			// Remove old buffs + fire hooks
			foreach( int buff_id in this.HasBuffIds.ToArray() ) {
				if( this.player.FindBuffIndex( buff_id ) == -1 ) {
					this.HasBuffIds.Remove( buff_id );
					TmlPlayerHelpers.OnBuffExpire( this.player, buff_id );
				}
			}
		}

		private void CheckArmorEquipHooks() {
			for( int i = 0; i < this.player.armor.Length; i++ ) {
				Item item = this.player.armor[i];

				if( item != null && !item.IsAir ) {
					bool found = this.EquipSlotsToItemTypes.ContainsKey( i );

					if( found && item.type != this.EquipSlotsToItemTypes[i] ) {
						TmlPlayerHelpers.OnArmorUnequip( this.player, i, this.EquipSlotsToItemTypes[i] );
					}

					if( !found || item.type != this.EquipSlotsToItemTypes[i] ) {
						this.EquipSlotsToItemTypes[i] = item.type;
						TmlPlayerHelpers.OnArmorEquip( this.player, i, item );
					}
				} else {
					if( this.EquipSlotsToItemTypes.ContainsKey(i) ) {
						TmlPlayerHelpers.OnArmorUnequip( this.player, i, this.EquipSlotsToItemTypes[i] );
						this.EquipSlotsToItemTypes.Remove( i );
					}
				}
			}
		}
	}
}
