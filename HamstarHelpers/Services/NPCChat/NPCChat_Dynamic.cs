﻿using System;
using Terraria;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.Dialogue {
	/// <summary>
	/// Allows editing the current NPC dialogue message.
	/// </summary>
	public class DynamicDialogueHandler {
		/// <summary>
		/// Accepts the current chat message and returns an (optionally) edited version.
		/// </summary>
		public Func<string, string> GetChat { get; }

		/// <summary>
		/// Indicates if the current NPC is showing an alert icon.
		/// </summary>
		public Func<bool> IsShowingAlert { get; }


		/// <summary></summary>
		/// <param name="getChat"></param>
		/// <param name="isShowingAlert"></param>
		public DynamicDialogueHandler( Func<string?, string> getChat, Func<bool> isShowingAlert ) {
			this.GetChat = getChat;
			this.IsShowingAlert = isShowingAlert;
		}
	}



	////////////////

	/// <summary>
	/// Provides a service for adding or removing town NPC chats (based on weight values).
	/// </summary>
	public partial class DialogueEditor : ILoadable {
		/// <summary>
		/// Retrieves the current dynamic dialogue message handler.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static DynamicDialogueHandler GetDynamicDialogueHandler( int npcType ) {
			DialogueEditor nc = TmlHelpers.SafelyGetInstance<DialogueEditor>();
			return nc.DynamicDialogueHandlers.GetOrDefault( npcType );
		}

		/// <summary>
		/// Sets the current priority dynamic dialogue message handler for a given NPC.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="handler"></param>
		public static void SetPriorityChat( int npcType, DynamicDialogueHandler handler ) {
			DialogueEditor nc = TmlHelpers.SafelyGetInstance<DialogueEditor>();
			nc.DynamicDialogueHandlers[npcType] = handler;
		}
	}
}
