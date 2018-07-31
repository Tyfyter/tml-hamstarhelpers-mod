﻿using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	public partial class CustomEntityManager {
		public static CustomEntityManager Instance { get { return HamstarHelpersMod.Instance.CustomEntMngr; } }



		////////////////

		private int LatestEntityID = 0;
		private readonly IDictionary<int, CustomEntity> EntityTemplates = new Dictionary<int, CustomEntity>();

		internal readonly IDictionary<int, CustomEntity> EntitiesByIndexes = new Dictionary<int, CustomEntity>();
		internal readonly IDictionary<Type, ISet<int>> EntitiesByComponentType = new Dictionary<Type, ISet<int>>();



		////////////////

		internal CustomEntityManager() {
			Main.OnTick += CustomEntityManager._Update;

			// Initialize components
			var component_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );

			foreach( var component_type in component_types ) {
				Type[] nested_types = component_type.GetNestedTypes( BindingFlags.Public | BindingFlags.NonPublic );

				foreach( var nested_type in nested_types ) {
					if( nested_type.IsSubclassOf(typeof(CustomEntityComponent.StaticInitializer)) ) {
						var static_init = (CustomEntityComponent.StaticInitializer)Activator.CreateInstance( nested_type );
						static_init.StaticInitializationWrapper();
					}
				}
			}

			Promises.AddPostWorldUnloadEachPromise( () => {
				this.EntitiesByIndexes.Clear();
				this.EntitiesByComponentType.Clear();
			} );

			Promises.AddModUnloadPromise( () => {
				this.LatestEntityID = 0;
				this.EntityTemplates.Clear();
			} );
		}

		~CustomEntityManager() {
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		private static void _Update() { // <- Just in case references are doing something funky...
			HamstarHelpersMod mymod = HamstarHelpersMod.Instance;
			if( mymod == null || mymod.CustomEntMngr == null ) { return; }

			mymod.CustomEntMngr.Update();
		}

		internal void Update() {
			foreach( CustomEntity ent in this.EntitiesByIndexes.Values.ToArray() ) {
				ent.Update();
			}
		}


		////////////////

		internal void DrawAll( SpriteBatch sb ) {
			foreach( CustomEntity ent in this.EntitiesByIndexes.Values ) {
				var draw_comp = ent.GetComponentByType<DrawsInGameEntityComponent>();
				if( draw_comp != null ) {
					draw_comp.Draw( sb, ent );
				}
			}
		}
	}
}
