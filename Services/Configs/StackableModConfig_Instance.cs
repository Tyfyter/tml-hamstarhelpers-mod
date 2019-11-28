﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Helps implement mod config stacking behavior. Replace your ModConfig classes with this class.
	/// </summary>
	public abstract class StackableModConfig : ModConfig {
		/// @private
		public override void OnChanged() {
			ModConfigStack.Uncache( this.GetType() );
		}


		/// <summary>
		/// Convenience method for pulling changed settings from a given config instance into the current one.
		/// </summary>
		/// <param name="changes"></param>
		public void CopyFrom( StackableModConfig changes ) {
			if( changes.GetType() != this.GetType() ) {
				throw new ModHelpersException( "Mismatched StackableModConfig types; found "+changes.GetType().Name
					+", expected "+this.GetType().Name );
			}
			ModConfigStack.SetStackedConfigChangesOnly( changes );
		}
	}
}
