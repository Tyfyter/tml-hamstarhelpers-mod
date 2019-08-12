﻿using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Internals.ModTags.Base.UI;
using System;


namespace HamstarHelpers.Internals.ModTags.ModBrowser.UI {
	partial class UIModTagsModBrowser : UIModTags<ModTagsModBrowserManager> {
		public UIModTagsModBrowser( UITheme theme, ModTagsModBrowserManager manager )
				: base( theme, manager, true ) {
		}
	}
}