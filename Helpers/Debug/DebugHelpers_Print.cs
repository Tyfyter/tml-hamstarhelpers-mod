﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Helpers.Debug {
	/** <summary>Assorted static "helper" functions pertaining to debugging and debug outputs.</summary> */
	public static partial class DebugHelpers {
		private static object MyPrintLock = new object();

		////////////////

		internal static bool Once;
		internal static int OnceInAWhile;

		public static IDictionary<string, string> Texts = new Dictionary<string, string>();
		public static IDictionary<string, int> TextTimes = new Dictionary<string, int>();
		public static IDictionary<string, int> TextTimeStart = new Dictionary<string, int>();
		public static IDictionary<string, int> TextShade = new Dictionary<string, int>();



		////////////////

		public static void MsgOnce( string msg ) {
			if( DebugHelpers.Once ) { return; }
			DebugHelpers.Once = true;

			Main.NewText( msg );
		}

		public static void MsgOnceInAWhile( string msg ) {
			if( DebugHelpers.OnceInAWhile > 0 ) { return; }
			DebugHelpers.OnceInAWhile = 60 * 10;

			Main.NewText( msg );
		}


		////////////////
		
		public static void Print( string msgLabel, string msg, int duration ) {
			lock( DebugHelpers.MyPrintLock ) {
				DebugHelpers.Texts[msgLabel] = msg;
				DebugHelpers.TextTimes[msgLabel] = duration;
				DebugHelpers.TextTimeStart[msgLabel] = duration;
				DebugHelpers.TextShade[msgLabel] = 255;

				if( DebugHelpers.Texts.Count > 16 ) {
					foreach( string key in DebugHelpers.TextTimes.Keys.ToList() ) {
						if( DebugHelpers.TextTimes[key] > 0 ) { continue; }

						DebugHelpers.Texts.Remove( key );
						DebugHelpers.TextTimes.Remove( key );
						DebugHelpers.TextTimeStart.Remove( key );
						DebugHelpers.TextShade.Remove( key );

						if( DebugHelpers.Texts.Count <= 16 ) { break; }
					}
				}
			}
		}

		////////////////

		internal static void PrintAll( SpriteBatch sb ) {
			int yPos = 0;

			lock( DebugHelpers.MyPrintLock ) {
				foreach( string key in DebugHelpers.Texts.Keys.ToList() ) {
					string msg = key + ":  " + DebugHelpers.Texts[key];
					Color color = Color.White;

					if( DebugHelpers.TextShade.ContainsKey(key) ) {
						int shade = DebugHelpers.TextShade[key];
						if( DebugHelpers.TextTimes.ContainsKey(key) ) {
							float timeRatio = (float)DebugHelpers.TextTimes[key] / (float)DebugHelpers.TextTimeStart[key];
							shade = (int)Math.Min( 255f, 255f * timeRatio );
						} else {
							DebugHelpers.TextShade[key]--;
						}
						color.R = color.G = color.B = color.A = (byte)Math.Max(shade, 16);
					}

					sb.DrawString( Main.fontMouseText, msg, new Vector2( 8, (Main.screenHeight - 32) - yPos ), color );

					if( DebugHelpers.TextTimes.ContainsKey(key) ) {
						if( DebugHelpers.TextTimes[key] > 0 ) {
							DebugHelpers.TextTimes[key]--;
						}
					}
					yPos += 24;
				}
			}
		}
	}
}