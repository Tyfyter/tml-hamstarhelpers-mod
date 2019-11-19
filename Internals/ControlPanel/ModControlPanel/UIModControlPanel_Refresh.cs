﻿using HamstarHelpers.Services.ModHelpers;
using Terraria;


namespace HamstarHelpers.Internals.ControlPanel.ModControlPanel {
	/// @private
	partial class UIModControlPanelTab : UIControlPanelTab {
		public void RefreshModLockButton() {
			bool areModsLocked = ModLockService.IsWorldLocked();
			string status = areModsLocked ? ": ON" : ": OFF";
			bool isEnabled = true;

			if( !ModHelpersMod.Config.WorldModLockEnable ) {
				status += " (disabled)";
				isEnabled = false;
			} else if( Main.netMode != 0 ) {
				status += " (single-player only)";
				isEnabled = false;
			}

			if( !isEnabled ) {
				if( this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Disable();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.ModLockButton.Enable();
				}
			}

			this.ModLockButton.SetText( UIModControlPanelTab.ModLockTitle + status );
		}


		////////////////

		public void UpdateElements() {
			if( !ModHelpersMod.Config.WorldModLockEnable ) {
				if( this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			} else {
				if( !this.ModLockButton.IsEnabled ) {
					this.RefreshModLockButton();
				}
			}
		}
	}
}