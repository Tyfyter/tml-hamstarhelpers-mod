﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity.Templates {
	public class CustomEntityTemplateManager {
		public static CustomEntity CreateEntityByID( int id, Player owner ) {
			string uid = "";

			if( owner != null ) {
				uid = PlayerIdentityHelpers.GetProperUniqueId( owner );
			}

			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;
			CustomEntityTemplate template = null;

			if( !templates.Templates.TryGetValue( id, out template ) ) {
				return null;
			}

			var core = new CustomEntityCore( template.DisplayName, template.Width, template.Height, default(Vector2), 0 );
			var components = template.Components.Select( c => c.InternalClone() ).ToList();

			if( !string.IsNullOrEmpty(uid) ) {
				return new CustomEntity( uid, core, components );
			}
			return new CustomEntity( core, components );
		}


		////////////////
		
		public static int Add( string name, int width, int height, IList<CustomEntityComponent> components ) {
			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;

			var template = new CustomEntityTemplate( name, width, height, components );

			return CustomEntityTemplateManager.Add( template );
		}

		internal static int Add( CustomEntityTemplate template ) {
			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;

			int id = templates.LatestEntityID++;

			templates.Templates[ id ] = template;

			return id;
		}


		////////////////

		internal static void Clear() {
			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;

			templates.Templates.Clear();
		}

		////////////////

		public static int Count() {
			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;

			return templates.LatestEntityID;
		}


		////////////////

		public static int GetID( IList<CustomEntityComponent> components ) {
			CustomEntityTemplateManager templates = ModHelpersMod.Instance.CustomEntMngr.TemplateMngr;
			int count = components.Count;

			foreach( var kv in templates.Templates ) {
				int id = kv.Key;
				CustomEntityTemplate template = kv.Value;

				int other_count = template.Components.Count;
				bool found = true;

				for( int i = 0; i < count; i++ ) {
					if( i >= other_count || components[i].GetType() != template.Components[i].GetType() ) {
						found = false;
						break;
					}
				}

				if( found ) {
					return id;
				}
			}

			return -1;
		}



		////////////////

		private int LatestEntityID = 0;

		internal readonly IDictionary<int, CustomEntityTemplate> Templates = new Dictionary<int, CustomEntityTemplate>();


		////////////////

		internal CustomEntityTemplateManager() {
			Promises.AddModUnloadPromise( () => {
				this.LatestEntityID = 0;
				this.Templates.Clear();
			} );
		}
	}
}
