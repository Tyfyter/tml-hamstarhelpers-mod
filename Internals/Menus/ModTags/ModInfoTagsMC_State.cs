﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers.Menus;
using HamstarHelpers.Internals.WebRequests;
using HamstarHelpers.Services.Menus;
using HamstarHelpers.Services.Promises;
using System.Collections.Generic;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags {
	partial class ModInfoTagsMenuContext : TagsMenuContextBase {
		public override void Show( UIState ui ) {
			base.Show( ui );
			this.ShowGeneral( ui );
		}

		public override void Hide( UIState ui ) {
			base.Hide( ui );
			this.HideGeneral( ui );
		}


		////////////////

		private void ShowGeneral( UIState ui ) {
			string mod_name = MenuModHelper.GetModName( MenuContextService.GetCurrentMenuUI(), ui );
			if( mod_name == null ) {
				LogHelpers.Log( "Could not load mod tags; no mod found" );
				return;
			}

			this.InfoDisplay.SetDefaultText( "" );

			this.ResetUIState( mod_name );
			this.SetCurrentMod( ui, mod_name );
			this.RecalculateMenuObjects();
		}

		////////////////

		private void HideGeneral( UIState ui ) {
			this.InfoDisplay.SetDefaultText( "" );

			this.ResetMenuObjects();
		}


		////////////////

		private void ResetUIState( string mod_name ) {
			if( !ModInfoTagsMenuContext.RecentTaggedMods.Contains( mod_name ) ) {
				if( this.FinishButton.IsLocked ) {
					this.FinishButton.Unlock();
				}
			} else {
				if( !this.FinishButton.IsLocked ) {
					this.FinishButton.Lock();
				}
			}

			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
				kv.Value.SetTagState( 0 );
			}
		}


		////////////////

		private void SetCurrentMod( UIState ui, string mod_name ) {
			this.CurrentModName = mod_name;

			Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
				this.AllModTagsSnapshot = args.ModTags;

				ISet<string> net_modtags = args.Found && args.ModTags.ContainsKey( mod_name ) ?
						args.ModTags[ mod_name ] :
						new HashSet<string>();
				bool has_net_tags = net_modtags.Count > 0;

//LogHelpers.Log( "SetCurrentMod modname: " + mod_name + ", modtags: " + string.Join(",", net_modtags ) );
				if( has_net_tags ) {
					this.InfoDisplay.SetDefaultText( "Do these tags look incorrect? If so, modify them." );
					this.FinishButton.SetModeReadOnly();
					this.ResetButton.Disable();
				} else {
					this.InfoDisplay.SetDefaultText( "No tags set for this mod. Why not add some?" );
					this.FinishButton.SetModeSubmit();
				}
				
				foreach( var kv in this.TagButtons ) {
					if( !has_net_tags ) {
						kv.Value.Enable();
					}

					kv.Value.SetTagState( net_modtags.Contains(kv.Key) ? 1 : 0 );
				}

				return false;
			} );
		}
	}
}