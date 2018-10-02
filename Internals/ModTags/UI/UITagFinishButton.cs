﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.UI {
	partial class UITagFinishButton : UIMenuButton {
		private readonly ModInfoTagsMenuContext MenuContext;

		public bool IsLocked { get; private set; }



		////////////////

		public UITagFinishButton( ModInfoTagsMenuContext menu_context )
				: base( UITheme.Vanilla, "", 72f, 40f, -300f, 172f, 0.55f, true ) {
			this.MenuContext = menu_context;

			this.RecalculatePos();
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			if( this.Text == "Modify" ) {
				this.SetModeSubmit();
			} else {
				this.MenuContext.SubmitTags();
			}
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.UpdateEnableState();
			this.MenuContext.DisableTagButtons();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.UpdateEnableState();
			this.MenuContext.EnableTagButtons();
			
			if( this.MenuContext.ResetButton.IsLocked ) {
				this.MenuContext.ResetButton.Unlock();
			}
		}
		

		////////////////

		public void SetModeReadOnly() {
			this.SetText( "Modify" );
			
			this.UpdateEnableState();
			this.MenuContext.ResetButton.UpdateEnableState();
		}

		public void SetModeSubmit() {
			this.SetText( "Submit" );
			
			this.MenuContext.EnableTagButtons();

			this.UpdateEnableState();
			this.MenuContext.ResetButton.UpdateEnableState();
		}

		////////////////

		public void UpdateEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			if( string.IsNullOrEmpty(this.MenuContext.CurrentModName) ) {
				this.Disable();
				return;
			}

			if( this.Text == "Modify" ) {
				this.Enable();
				return;
			}

			if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( this.MenuContext.CurrentModName ) ) {
				this.Disable();
				return;
			}

			if( this.MenuContext.GetTagsOfState(1).Count >= 2 ) {
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
