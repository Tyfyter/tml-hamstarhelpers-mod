﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.UI.Menu.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base {
	abstract partial class ModTagsManager {
		protected static ISet<string> RecentTaggedMods = new HashSet<string>();

		private UIInfoDisplay InfoDisplay;



		////////////////

		public UIModTagsInterface TagsUI { get; protected set; }

		public virtual TagDefinition[] MyTags => ModTagsManager.Tags;

		public string CurrentModName { get; protected set; }
		public bool CanExcludeTags { get; private set; }

		public IDictionary<string, ISet<string>> AllModTagsSnapshot { get; protected set; }



		////////////////

		protected ModTagsManager( UIInfoDisplay infoDisplay, bool canExcludeTags ) {
			this.InfoDisplay = infoDisplay;
			this.CanExcludeTags = canExcludeTags;
			//this.TagsUI = new UIModTagsPanel( UITheme.Vanilla, this, uiContext, this.CanExcludeTags );
		}


		////////////////

		public bool IsCurrentModRecentlyTagged() {
			return ModTagsManager.RecentTaggedMods.Contains( this.CurrentModName );
		}

		////////////////

		public ISet<string> GetTagsWithGivenState( int state ) {
			return this.TagsUI.GetTagsWithGivenState( state );
		}


		////////////////

		public virtual bool CanEditTags() {
			return false;
		}
	}
}
