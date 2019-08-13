﻿using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	/// @private
	partial class UIModTagsResetMenuButton : UIMenuButton {
		private readonly ModTagsManager Manager;

		public bool IsLocked { get; private set; }



		////////////////

		public UIModTagsResetMenuButton( UITheme theme, ModTagsManager manager )
				: base( theme, "Reset Tags", 98f, 24f, -196f, 172f, 0.36f, true ) {
			this.Manager = manager;
		}


		////////////////

		public override void Click( UIMouseEvent evt ) {
			if( !this.IsEnabled ) { return; }

			this.Manager.TagsUI.ResetTagButtons( false );
		}


		////////////////

		public void Lock() {
			this.IsLocked = true;

			this.RefreshEnableState();
		}

		public void Unlock() {
			this.IsLocked = false;

			this.RefreshEnableState();
		}
		

		////////////////

		public void RefreshEnableState() {
			if( this.IsLocked ) {
				this.Disable();
				return;
			}

			//if( ModInfoTagsMenuContext.RecentTaggedMods.Contains( modInfoContext.CurrentModName ) ) {
			if( this.Manager.IsCurrentModRecentlyTagged() ) {
				this.Disable();
				return;
			}

			if( this.Manager.GetTagsWithGivenState(1).Count > 0 || this.Manager.GetTagsWithGivenState(-1).Count > 0 ) {
				//modInfoContext.FinishButton.Text == "Modify Tags"
				if( this.Manager.CanEditTags() ) {
					this.Disable();
					return;
				}
				
				this.Enable();
				return;
			} else {
				this.Disable();
				return;
			}
		}
	}
}
