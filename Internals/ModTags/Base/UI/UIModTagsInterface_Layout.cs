﻿using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ModTags.Base.Manager;
using HamstarHelpers.Internals.ModTags.Base.UI.Buttons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		private void LayoutCategoryButtons() {
			float top = this.PositionY - 2;
			float x = this.PositionXCenterOffset;
			float y = top;

			foreach( UICategoryMenuButton catButton in this.CategoryButtons.Values ) {
				catButton.SetMenuSpacePosition( x, y );

				y += UICategoryMenuButton.ButtonHeight;
				if( y >= (UIModTagsInterface.CategoryPanelHeight + top - 2) ) {
					y = top;
					x += UICategoryMenuButton.ButtonWidth - 2;
				}
			}
		}

		private void LayoutTagButtonsByCategory() {
			float x, y;
			float top = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
			float maxY = UIModTagsInterface.TagsPanelHeight + top - UIResetTagsMenuButton.ButtonHeight - 4;
			TagDefinition[] tags = this.Manager.MyTags;

			IEnumerable<IGrouping<string, TagDefinition>> groups = tags.GroupBy( tagDef => tagDef.Category );

			foreach( IGrouping<string, TagDefinition> group in groups ) {
				x = this.PositionXCenterOffset;
				y = top;

				foreach( TagDefinition tagDef in group ) {
					UITagMenuButton button = this.TagButtons[ tagDef.Tag ];

					button.SetMenuSpacePosition( x, y );

					if( group.Key == this.CurrentCategory ) {
						button.TakeOut();
					} else {
						button.PutAway();
					}

					y += UITagMenuButton.ButtonHeight;
					if( y >= maxY ) {
						y = this.PositionY + UIModTagsInterface.CategoryPanelHeight;
						x += UITagMenuButton.ButtonWidth;
					}
				}
			}
		}


		////////////////

		public Vector2 GetTagControlsTopLeftPositionOffset() {
			float x = this.PositionXCenterOffset;
			float y = this.PositionY
				+ UIModTagsInterface.CategoryPanelHeight
				+ UIModTagsInterface.TagsPanelHeight
				- UIResetTagsMenuButton.ButtonHeight;

			return new Vector2( x, y );
		}

		public Rectangle GetCategoryPanelRectangle() {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			int wid = UIModTagsInterface.PanelWidth;
			return new Rectangle( x, y, wid, UIModTagsInterface.CategoryPanelHeight );
		}

		public Rectangle GetTagsPanelRectangle() {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			int wid = UIModTagsInterface.PanelWidth;
			return new Rectangle( x, y + UIModTagsInterface.CategoryPanelHeight + 2, wid, UIModTagsInterface.TagsPanelHeight );
		}
	}
}