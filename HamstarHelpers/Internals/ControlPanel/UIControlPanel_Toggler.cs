﻿using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.ControlPanel.ModControlPanel;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Messages.Inbox;
using HamstarHelpers.Services.Hooks.LoadHooks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.UI;


namespace HamstarHelpers.Internals.ControlPanel {
	/// @private
	partial class UIControlPanel : UIState {
		private static Version AlertSinceVersion = new Version( 2, 0, 0 );


		////////////////

		private static Vector2 TogglerPosition {
			get {
				var config = ModHelpersConfig.Instance;
				int x = config.ControlPanelIconX < 0 ? Main.screenWidth + config.ControlPanelIconX : config.ControlPanelIconX;
				int y = config.ControlPanelIconY < 0 ? Main.screenHeight + config.ControlPanelIconY : config.ControlPanelIconY;

				return new Vector2( x, y );
			}
		}
		


		////////////////

		public bool IsTogglerLit { get; private set; }

		
		////////////////

		private void InitializeToggler() {
			this.IsTogglerLit = false;

			LoadHooks.AddWorldLoadEachHook( () => {
				var uiModCtrlPanel = (UIModControlPanelTab)ModHelpersMod.Instance.ControlPanel.DefaultTab;
				int modUpdateCount = uiModCtrlPanel.GetModUpdatesAvailable();
				
				if( modUpdateCount > 0 ) {
					InboxMessages.SetMessage( "mod_updates", modUpdateCount + " mod updates available. See mod browser.", true );
				}
			} );
		}


		////////////////

		public bool IsTogglerShown() {
			return Main.playerInventory;
		}

		public bool IsTogglerUpdateAlertShown() {
			var mymod = ModHelpersMod.Instance;
			if( mymod.Data == null ) {
				LogHelpers.WarnOnce( "No mod data." );
				return false;
			}

			var ver = new Version( mymod.Data.ControlPanelNewSince );

			if( ver < UIControlPanel.AlertSinceVersion ) {
				return true;
			}

			UIModControlPanelTab uiModCtrlPanel = mymod.ControlPanel.DefaultTab;
			if( uiModCtrlPanel.GetModUpdatesAvailable() > 0 ) {
				return true;
			}

			return false;
		}


		////////////////

		public void UpdateToggler() {
			this.CheckTogglerMouseInteraction();

			if( this.IsTogglerLit ) {
				Main.LocalPlayer.mouseInterface = true;
			}
		}


		////////////////

		public void DrawToggler( SpriteBatch sb ) {
			if( !this.IsTogglerShown() ) { return; }

			bool alertShown = this.IsTogglerUpdateAlertShown();
			Texture2D tex;
			Color color;

			if( this.IsTogglerLit ) {
				tex = UIControlPanel.ControlPanelIconLit;
				color = new Color( 192, 192, 192, 192 );
			} else {
				tex = UIControlPanel.ControlPanelIcon;
				color = new Color( 160, 160, 160, 160 );
			}

			sb.Draw( tex, UIControlPanel.TogglerPosition, null, color );

			if( alertShown ) {
				this.DrawTogglerAlert( sb );
			}

			if( this.IsTogglerLit ) {
				if( alertShown ) {
					sb.DrawString( Main.fontMouseText, "New mod updates!", new Vector2( Main.mouseX + 8, Main.mouseY + 8 ), AnimatedColors.Alert.CurrentColor );
				} else {
					sb.DrawString( Main.fontMouseText, "Mod Control Panel", new Vector2( Main.mouseX + 8, Main.mouseY + 8 ), Color.White );
				}
			}
		}

		
		private void DrawTogglerAlert( SpriteBatch sb ) {
			Color color = AnimatedColors.Alert != null ? AnimatedColors.Alert.CurrentColor : Color.White;
			Vector2 pos = UIControlPanel.TogglerPosition;
			pos.Y += 6f;
			//pos.X += 56f - (Main.fontMouseText.MeasureString("New!").X * 0.5f);
			//pos.Y -= 4f;

			//sb.DrawString( Main.fontMouseText, "New!", pos, color );
			sb.DrawString( Main.fontMouseText, "New!", pos+new Vector2(-0.35f,-0.35f), Color.Black, 0f, default( Vector2 ), 0.64f, SpriteEffects.None, 1f );
			sb.DrawString( Main.fontMouseText, "New!", pos, color, 0f, default(Vector2), 0.6f, SpriteEffects.None, 1f );
		}


		////////////////
		
		private void CheckTogglerMouseInteraction() {
			bool isClick = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 pos = UIControlPanel.TogglerPosition;
			Vector2 size = UIControlPanel.ControlPanelIcon.Size();

			this.IsTogglerLit = false;

			if( this.IsTogglerShown() ) {
				if( Main.mouseX >= pos.X && Main.mouseX < (pos.X + size.X) ) {
					if( Main.mouseY >= pos.Y && Main.mouseY < (pos.Y + size.Y) ) {
						if( isClick && !this.HasClicked ) {
							if( this.IsOpen ) {
								this.Close();
							} else if( this.CanOpen() ) {
								this.Open();

								var mymod = ModHelpersMod.Instance;
								Version oldVers;
								Version newVers = UIControlPanel.AlertSinceVersion;

								if( Version.TryParse(mymod.Data.ControlPanelNewSince, out oldVers) && oldVers != newVers ) {
									mymod.Data.ControlPanelNewSince = newVers.ToString();
									mymod.SaveModData();
								}
							}
						}

						this.IsTogglerLit = true;
					}
				}
			}

			this.HasClicked = isClick;
		}
	}
}