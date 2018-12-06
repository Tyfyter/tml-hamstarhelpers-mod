﻿using HamstarHelpers.Components.UI;
using HamstarHelpers.Components.UI.Elements.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.Menus.ModTags.UI {
	internal class UITagButton : UIMenuButton {
		public const float ColumnWidth = 102f;
		public const float RowHeight = 16f;
		public const int ColumnHeightTall = 31;
		public const int ColumnHeightShort = 8;
		public const int ColumnsInMid = 5;
		public const int LastColumnPos = 1;
		public const int LastColumnRowStart = 8;



		////////////////

		private readonly TagsMenuContextBase MenuContext;

		public int Column;
		public int Row;

		////////////////
		
		public string Desc { get; private set; }
		public int TagState { get; private set; }



		////////////////

		public UITagButton( TagsMenuContextBase menuContext, int pos, string label, string desc, bool canNegateTags )
				: base( UITheme.Vanilla, label, UITagButton.ColumnWidth, UITagButton.RowHeight, -308f, 40, 0.6f, false ) {
			this.MenuContext = menuContext;
			this.TagState = 0;
			this.DrawPanel = false;
			this.Desc = desc;

			int colTall = UITagButton.ColumnHeightTall;
			int colShort = UITagButton.ColumnHeightShort;
			int colsInMid = UITagButton.ColumnsInMid;
			int lastColPos = colTall + ( colShort * colsInMid );

			if( pos < colTall ) {
				this.Column = 0;
				this.Row = pos;
			} else if( pos >= lastColPos ) {
				this.Column = UITagButton.LastColumnPos;
				this.Row = UITagButton.LastColumnRowStart + pos - lastColPos;
			} else {
				this.Column = 1 + (( pos - colTall ) / colShort );
				this.Row = ( pos - colTall ) % colShort;
			}
			
			this.OnClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled ) { return; }
				this.TogglePositiveTag();
			};
			this.OnRightClick += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( !this.IsEnabled || !canNegateTags ) { return; }
				this.ToggleNegativeTag();
			};
			this.OnMouseOver += ( UIMouseEvent evt, UIElement listeningElement ) => {
				MenuContext.InfoDisplay?.SetText( desc );
				this.RefreshTheme();
			};
			this.OnMouseOut += ( UIMouseEvent evt, UIElement listeningElement ) => {
				if( MenuContext.InfoDisplay?.GetText() == desc ) {
					MenuContext.InfoDisplay?.SetText( "" );
				}
				this.RefreshTheme();
			};

			this.Disable();
			this.RecalculatePos();
			this.RefreshTheme();
		}


		////////////////

		public override void RecalculatePos() {
			float width = this.Width.Pixels;
			float left = (((Main.screenWidth / 2) + this.XCenterOffset) - (width - 8)) + ((width - 2) * this.Column);
			float top = (UITagButton.RowHeight * this.Row) + this.YPos;

			this.Left.Set( left, 0f );
			this.Top.Set( top, 0f );
		}


		////////////////

		public void SetTagState( int state ) {
			if( state < -1 || state > 1 ) { throw new Exception( "Invalid state." ); }
			if( this.TagState == state ) { return; }
			this.TagState = state;

			this.MenuContext.OnTagStateChange( this );
			this.RefreshTheme();
		}

		public void TogglePositiveTag() {
			this.TagState = this.TagState <= 0 ? 1 : 0;

			this.MenuContext.OnTagStateChange( this );
			this.RefreshTheme();
		}

		public void ToggleNegativeTag() {
			this.TagState = this.TagState >= 0 ? -1 : 0;

			this.MenuContext.OnTagStateChange( this );
			this.RefreshTheme();
		}


		////////////////

		public override void RefreshTheme() {
			base.RefreshTheme();

			if( this.TagState > 0 ) {
				this.TextColor = Color.LimeGreen;
			} else if( this.TagState < 0 ) {
				this.TextColor = Color.Red;
			}
		}


		////////////////

		public Color GetBgColor() {
			Color bgColor = !this.IsEnabled ?
				this.Theme.ButtonBgDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonBgLitColor :
					this.Theme.ButtonBgColor;
			byte a = bgColor.A;
			
			if( this.Desc.Contains("Mechanics:") ) {
				bgColor = Color.Lerp( bgColor, Color.Gold, 0.3f );
			} else if( this.Desc.Contains("Theme:") ) {
				bgColor = Color.Lerp( bgColor, Color.DarkTurquoise, 0.4f );
			} else if( this.Desc.Contains( "Content:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DarkRed, 0.3f );
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Desc.Contains( "When:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Green, 0.4f );
			} else if( this.Desc.Contains( "State:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DarkViolet, 0.4f );
			} else if( this.Desc.Contains( "Judgmental:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.DimGray, 0.4f );
			} else if( this.Desc.Contains( "Priviledge:" ) ) {
				bgColor = Color.Lerp( bgColor, Color.Black, 0.4f );
			}
			bgColor.A = a;

			return bgColor;
		}

		public Color GetEdgeColor() {
			Color edgeColor = !this.IsEnabled ?
				this.Theme.ButtonEdgeDisabledColor :
				this.IsMouseHovering ?
					this.Theme.ButtonEdgeLitColor :
					this.Theme.ButtonEdgeColor;
			byte a = edgeColor.A;
			
			if( this.Desc.Contains( "Mechanics:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Goldenrod, 0.35f );
			} else if( this.Desc.Contains( "Theme:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Aquamarine, 0.25f );
			} else if( this.Desc.Contains( "Content:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Red, 0.25f );
			//} else if( this.Desc.Contains( "Where:" ) ) {
			//	edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Desc.Contains( "When:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Green, 0.25f );
			} else if( this.Desc.Contains( "State:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DarkViolet, 0.4f );
			} else if( this.Desc.Contains( "Judgmental:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.DimGray, 0.4f );
			} else if( this.Desc.Contains( "Priviledge:" ) ) {
				edgeColor = Color.Lerp( edgeColor, Color.Black, 0.4f );
			}
			edgeColor.A = a;

			return edgeColor;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			Rectangle rect = this.GetOuterDimensions().ToRectangle();
			rect.X += 4;
			rect.Y += 4;
			rect.Width -= 4;
			rect.Height -= 5;

			HudHelpers.DrawBorderedRect( sb, this.GetBgColor(), this.GetEdgeColor(), rect, 2 );

			base.Draw( sb );
		}
	}
}
