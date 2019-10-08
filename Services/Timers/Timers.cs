﻿using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.Timers {
	/// <summary>
	/// Provides a way to delay the onset of a given action by a set amount of ticks. As a secondary function,
	/// MainOnTickGet() provides a way to use Main.OnTick for running background tasks at 60FPS.
	/// </summary>
	public partial class Timers {
		private readonly static object MyLock = new object();



		////////////////

		/// <summary>
		/// Returns a delegate that returns true when at least 1/60th second (1 60FPS 'tick') has fully elapsed since
		/// the last time the delegate returned true.
		/// </summary>
		/// <returns></returns>
		public static Func<bool> MainOnTickGet() {
			long then = DateTime.Now.Ticks;
			int frames = 0, subFrames;
//int FRAMES=0;

			return () => {
				long now = DateTime.Now.Ticks;
				int span = (int)( now - then );

				frames += span / (10000000 / 90);
				subFrames = span % (10000000 / 90);

				then = now - subFrames;
//DebugHelpers.Print("blahh", "frames: "+FRAMES, 20);
				
				if( frames > 0 ) {
//FRAMES+=frames;
					frames = 0;//frames--;
					return true;
				}
				return false;
			};
		}


		////////////////

		/// <summary>
		/// Creates a 'timer' that waits the given amount of ticks before running the given action. Not multi-threaded,
		/// but does not obstruct current thread.
		/// </summary>
		/// <param name="name">Identifier of timer. Re-assigning with this identifier replaces any existing timer.</param>
		/// <param name="tickDuration"></param>
		/// <param name="action">Action to run. Returns `true` to make the action repeat after another period of the
		/// given tick duration.</param>
		public static void SetTimer( string name, int tickDuration, Func<bool> action ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running[name] = (Callback: action, Elapsed: tickDuration );
				timers.Elapsed[name] = 0;
				timers.Expired.Remove( name );
			}
		}


		/// <summary>
		/// Indicates time remaining for the given 'timer'.
		/// </summary>
		/// <param name="name">Identifier of timer.</param>
		/// <returns></returns>
		public static int GetTimerTickDuration( string name ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return 0; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					return timers.Running[name].Elapsed - timers.Elapsed[ name ];
				}
			}

			return 0;
		}


		/// <summary>
		/// Ends a given 'timer'.
		/// </summary>
		/// <param name="name">Identifier of timer.</param>
		public static void UnsetTimer( string name ) {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				if( timers.Running.ContainsKey( name ) ) {
					timers.Running.Remove( name );
					timers.Elapsed.Remove( name );
					timers.Expired.Remove( name );
				}
			}
		}


		/// <summary>
		/// Ends all 'timers'.
		/// </summary>
		public static void ResetAll() {
			var timers = ModHelpersMod.Instance?.Timers;
			if( timers == null ) { return; }

			lock( Timers.MyLock ) {
				timers.Running = new Dictionary<string, (Func<bool>, int)>();
				timers.Elapsed = new Dictionary<string, int>();
				timers.Expired = new HashSet<string>();
			}
		}
	}
}
