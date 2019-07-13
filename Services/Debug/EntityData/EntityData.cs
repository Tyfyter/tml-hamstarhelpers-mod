﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;


namespace HamstarHelpers.Services.Debug.EntityData {
	/// <summary>
	/// Provides functions for acquiring and dumping entity (NPC, Projectile, Item) data fields to file.
	/// </summary>
	public class EntityData {
		private static readonly object MyFileLock = new object();



		////////////////

		/// <summary>
		/// Generates a table of field and property defaults of a given NPC type.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetNpcData( int npcType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( NPC ).GetFields();
			var props = typeof( NPC ).GetProperties();

			NPC npc = new NPC();
			npc.SetDefaults( npcType );

			foreach( var field in fields ) {
				data[ field.Name ] = field.GetValue( npc );
			}
			foreach( var prop in props ) {
				data[ prop.Name ] = prop.GetValue( npc );
			}

			return data;
		}

		/// <summary>
		/// Generates a table of field and property defaults of a given Item type.
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetItemData( int itemType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( Item ).GetFields();
			var props = typeof( Item ).GetProperties();

			Item item = new Item();
			item.SetDefaults( itemType );

			foreach( var field in fields ) {
				data[ field.Name ] = field.GetValue( item );
			}
			foreach( var prop in props ) {
				data[ prop.Name ] = prop.GetValue( item );
			}

			return data;
		}

		/// <summary>
		/// Generates a table of field and property defaults of a given Projectile type.
		/// </summary>
		/// <param name="projType"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetProjectileData( int projType ) {
			var data = new Dictionary<string, object>();
			var fields = typeof( Projectile ).GetFields();
			var props = typeof( Projectile ).GetProperties();

			Projectile proj = new Projectile();
			proj.SetDefaults( projType );

			foreach( var field in fields ) {
				data[ field.Name ] = field.GetValue( proj );
			}
			foreach( var prop in props ) {
				data[ prop.Name ] = prop.GetValue( proj );
			}

			return data;
		}


		////////////////

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given NPC type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllNpcData() {
			var data = new Dictionary<int, IDictionary<string, object>>();
			
			for( int i = 0; i < Main.npcTexture.Length; i++ ) {
				data[ i ] = EntityData.GetNpcData( i );
			}
			return data;
		}

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given Item type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllItemData() {
			var data = new Dictionary<int, IDictionary<string, object>>();

			for( int i = 0; i < Main.itemTexture.Length; i++ ) {
				data[i] = EntityData.GetItemData( i );
			}
			return data;
		}

		/// <summary>
		/// Generates a table of tables of field and property defaults of each given Projectile type.
		/// </summary>
		/// <returns></returns>
		public static IDictionary<int, IDictionary<string, object>> GetAllProjectileData() {
			var data = new Dictionary<int, IDictionary<string, object>>();

			for( int i = 0; i < Main.projectileTexture.Length; i++ ) {
				data[i] = EntityData.GetProjectileData( i );
			}
			return data;
		}


		////////////////

		/// <summary>
		/// Dumps all NPC data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllNpcDataToJson() {
			string json = JsonConvert.SerializeObject( EntityData.GetAllNpcData() );
			
			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All NPCs.json", json );
			}
		}

		/// <summary>
		/// Dumps all Item data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllItemDataToJson() {
			string json = JsonConvert.SerializeObject( EntityData.GetAllItemData() );

			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All Items.json", json );
			}
		}

		/// <summary>
		/// Dumps all Projectile data to a JSON file in the ModLoader folder.
		/// </summary>
		public static void DumpAllProjectileDataToJson() {
			string json = JsonConvert.SerializeObject( EntityData.GetAllProjectileData() );

			lock( EntityData.MyFileLock ) {
				File.WriteAllText( Main.SavePath + Path.DirectorySeparatorChar + "All Projectiles.json", json );
			}
		}
	}
}
