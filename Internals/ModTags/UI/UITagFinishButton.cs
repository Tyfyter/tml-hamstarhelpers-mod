﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModInfoTagsMenuContext UIManager;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( ModInfoTagsMenuContext modtagui )
				: base( UITheme.Vanilla, "", 72f, 40f, -286f, 172f, 0.55f, true ) {
			this.UIManager = modtagui;

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify" ) {
				this.SetTagSubmitMode();
			} else {
				this.UIManager.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
			this.UIManager.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.UIManager.EnableTagButtons();
		}
		

		////////////////

		public void SetTagUpdateMode() {
			this.SetText( "Modify" );

			this.UpdateEnableState();
		}

		public void SetTagSubmitMode() {
			this.SetText( "Submit" );
			this.Disable();
			
			this.UIManager.EnableTagButtons();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			if( string.IsNullOrEmpty(this.UIManager.ModName) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( this.UIManager.ModName ) ) {
				this.Disable();
				return;
			}

			if( this.UIManager.GetTagsOfState(1).Count >= 2 ) {
				this.Enable();
			} else {
				this.Disable();
			}
		}
	}
}
