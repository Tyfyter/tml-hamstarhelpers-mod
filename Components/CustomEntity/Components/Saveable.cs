﻿using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.WorldHelpers;
using HamstarHelpers.Services.Promises;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		internal readonly static object MyValidatorKey;
		public readonly static PromiseValidator LoadAllValidator;

		internal readonly static object LoadAllDataKey = new object();



		////////////////

		static SaveableEntityComponent() {
			SaveableEntityComponent.LoadAllDataKey = new object();
			SaveableEntityComponent.MyValidatorKey = new object();
			SaveableEntityComponent.LoadAllValidator = new PromiseValidator( SaveableEntityComponent.MyValidatorKey );
		}


		////////////////

		public static string GetFileNameBase() {
			return "world_" + WorldHelpers.GetUniqueIdWithSeed() + "_ents";
		}


		////////////////

		[PacketProtocolIgnore]
		public bool AsJson = true;



		////////////////

		protected SaveableEntityComponent( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }

		protected override void OnInitialize() { }
	}
}
