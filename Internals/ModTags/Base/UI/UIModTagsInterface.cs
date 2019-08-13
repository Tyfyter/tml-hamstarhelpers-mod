﻿using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Elements.Menu;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using System;


namespace HamstarHelpers.Internals.ModTags.Base.UI {
	abstract partial class UIModTagsInterface : UIThemedPanel {
		public static int PanelWidth { get; private set; } = 192;
		public static int CategoryPanelHeight { get; private set; } = 128;
		public static int TagsPanelHeight { get; private set; } = 368;

		public static int CategoryButtonWidth { get; private set; } = 160;
		public static int CategoryButtonHeight { get; private set; } = 32;



		////////////////

		public string SelectedCategory { get; protected set; }

		protected readonly IDictionary<string, UIMenuButton> CategoryButtons = new Dictionary<string, UIMenuButton>();
		protected readonly IDictionary<string, UITagMenuButton> TagButtons = new Dictionary<string, UITagMenuButton>();


		////////////////

		protected int PositionXCenterOffset;
		protected int PositionY;

		protected ModTagsManager Manager;

		protected UIResetTagsMenuButton ResetButton;



		////////////////

		public UIModTagsInterface( UITheme theme, ModTagsManager manager, bool canExcludeTags )
				: base( theme, true ) {
			this.Manager = manager;

			this.PositionXCenterOffset = -400 + 4;
			this.PositionY = 64 + 4;
			this.InitializeControls( canExcludeTags );

			this.RefreshTheme();
			this.Recalculate();
		}


		////////////////

		public override void Recalculate() {
			float x = ((float)Main.screenWidth * 0.5f) - this.PositionXCenterOffset;

			this.Top.Set( this.PositionY, 0f );
			this.Left.Set( x, 0f );
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			int x = (int)this.Left.Pixels;
			int y = (int)this.Top.Pixels;
			var rect1 = new Rectangle( x, y, UIModTagsInterface.PanelWidth, UIModTagsInterface.CategoryPanelHeight );
			var rect2 = new Rectangle( x, y+132, UIModTagsInterface.PanelWidth, UIModTagsInterface.TagsPanelHeight );

			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect1, 2 );
			HUDHelpers.DrawBorderedRect( sb, this.Theme.MainBgColor, this.Theme.MainEdgeColor, rect2, 2 );

			base.Draw( sb );
		}
	}
}