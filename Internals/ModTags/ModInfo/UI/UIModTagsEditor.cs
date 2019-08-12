﻿using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using Terraria.UI;


namespace HamstarHelpers.Internals.ModTags.ModInfo.UI {
	partial class UIModTagsEditor : UIModTags<ModTagsEditorManager> {
		protected UIHiddenPanel HiddenPanel;
		protected UITagFinishButton FinishButton;



		////////////////

		public UIModTagsEditor( UITheme theme,
				ModTagsEditorManager manager,
				UIState uiModInfo )
				: base( theme, manager, false ) {
			this.InitializeEditorControls( uiModInfo );
		}


		////////////////

		public override void RefreshButtonEnableStates() {
			base.RefreshButtonEnableStates();
			this.FinishButton.RefreshEnableState();
		}


		////////////////

		public bool CanEditTags() {
			return this.FinishButton.Text == "Modify Tags";
		}

		////////////////

		public void EnableSubmitOption() {
			this.FinishButton.SetModeSubmit();
		}

		public void DisableSubmitOption() {
			this.FinishButton.SetModeReadOnly();
		}

		////////////////

		public void LockFinishButton() {
			if( !this.FinishButton.IsLocked ) {
				this.FinishButton.Lock();
			}
		}

		public void UnlockFinishButton() {
			if( this.FinishButton.IsLocked ) {
				this.FinishButton.Unlock();
			}
		}
	}
}