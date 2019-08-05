﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.User;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class ModCallCommand : ModCommand {
		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		/// @private
		public override string Command => "mh-mod-call";
		/// @private
		public override string Usage => "/" + this.Command + " MyModName ModAPIFunctionName unquotedstringparam 42 \"quote-wrapped strings allow spaces\" anotherparameter";
		/// @private
		public override string Description => "Runs Mod.Call(). Use with care!"
			+ "\n   Parameters: <mod name> <parameter 1> <parameter 2> etc...";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				LogHelpers.Warn( "Not supposed to run on client." );
				return;
			}

			if( Main.netMode == 2 && caller.CommandType != CommandType.Console ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			var mymod = ModHelpersMod.Instance;
			if( !mymod.Config.ModCallCommandEnabled ) {
				throw new UsageException( "Mod.Call() command disabled by settings." );
			}

			if( args.Length < 2 ) {
				if( args.Length == 0 ) {
					throw new UsageException( "No arguments given." );
				} else {
					throw new UsageException( "More arguments needed." );
				}
			}

			Mod callmod = null;
			try {
				callmod = ModLoader.GetMod( args[0] );
				if( callmod == null ) { throw new ModHelpersException("Bad call mod."); }
			} catch( Exception ) {
				throw new UsageException( "Invald mod name " + args[0] );
			}

			try {
				object[] callArgs = new object[args.Length - 1];

				for( int i = 1; i < args.Length; i++ ) {
					callArgs[i - 1] = DotNetHelpers.ParseToInferredPrimitiveType( args[i] );
				}

				callmod.Call( callArgs );
			} catch( Exception e ) {
				caller.Reply( e.Message, Color.Red );
			}
		}
	}
}
