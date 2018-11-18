﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	internal class SerializedCustomEntity : CustomEntity {
		public string MyTypeName;


		////////////////

		protected SerializedCustomEntity( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }
		
		internal SerializedCustomEntity( PacketProtocolDataConstructorLock ctor_lock, CustomEntity ent ) : base( ctor_lock ) {
			this.OwnerPlayerUID = ent.OwnerPlayerUID;
			this.OwnerPlayerWho = ent.OwnerPlayerWho;
			this.Core = ent.Core;
			this.Components = ent.Components;
			this.MyTypeName = ent.GetType().Name;
		}

		////////////////

		internal CustomEntity Convert() {
			Type ent_type = Type.GetType( this.MyTypeName );

			if( !ent_type.IsSubclassOf( typeof( CustomEntity ) ) ) {
				throw new HamstarException( ent_type.Name + " is not a valid CustomEntity." );
			}

			var args = this.OwnerPlayerWho == -1 ?
				new object[] { this.Core, this.Components } :
				new object[] { this.OwnerPlayerUID, this.Core, this.Components };

			return (CustomEntity)Activator.CreateInstance( ent_type,
				BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				args,
				null );
		}
	}




	internal class CustomEntityConverter : JsonConverter {
		public override bool CanWrite => false;

		public override bool CanConvert( Type obj_type ) {
			return obj_type == typeof( SerializedCustomEntity );
		}


		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer ) {
			string type_name = null;
			JToken raw_ent_data = null;
			string player_uid = null;
			CustomEntityCore core = null;
			string[] comp_names = null;
			IList<CustomEntityComponent> components = new List<CustomEntityComponent>();

			IEnumerable<Type> all_comp_types = ReflectionHelpers.GetAllAvailableSubTypes( typeof( CustomEntityComponent ) );
			IDictionary<string, Type> all_comp_type_map = all_comp_types.ToDictionary( t => t.Name, t => t );

			JObject jo = JObject.Load( reader );
			
			try {
				type_name = jo["MyTypeName"].ToObject<String>();
				raw_ent_data = jo["MyEntity"];

				//int type_id = jo["TypeID"].ToObject<Int32>();
				player_uid = raw_ent_data["OwnerPlayerUID"].ToObject<String>();
				core = raw_ent_data["Core"].ToObject<CustomEntityCore>();
				comp_names = raw_ent_data["ComponentNames"].ToObject<string[]>();
				JToken raw_components = raw_ent_data["Components"];
				
				Type[] comp_types = new Type[comp_names.Length];
				int i;
				
				for( i = 0; i < comp_names.Length; i++ ) {
					if( !all_comp_type_map.ContainsKey( comp_names[i] ) ) {
						return null;
					}
					comp_types[i] = all_comp_type_map[comp_names[i]];
				}
				
				i = 0;
				foreach( JObject obj in raw_components ) {
					Type comp_type = comp_types[i];
					object comp = obj.ToObject( comp_type, serializer );

					components.Add( (CustomEntityComponent)comp );
					i++;
				}
				
				Type ent_type = CustomEntityManager.GetTypeByName( type_name );
				if( ent_type == null ) {
					return null;
				}

				return CustomEntity.CreateRaw( ent_type, core, components, player_uid );
				//return (CustomEntity)Activator.CreateInstance( ent_type,
				//	BindingFlags.NonPublic | BindingFlags.Instance,
				//	null,
				//	new object[] { core, components, player_uid },
				//	null );
			} catch( Exception e ) {
				LogHelpers.Log( "!ModHelpers.CustomEntity.ReadJson - " + e.Message );
				return null;
			}
		}


		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
			throw new NotImplementedException();
		}
	}




	public abstract partial class CustomEntity : PacketProtocolData {
		internal static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.None,
			ContractResolver = new CustomEntityCoreContractResolver(),
			Converters = new List<JsonConverter> { new CustomEntityConverter() }
		};


		////////////////

		protected override void WriteStream( BinaryWriter writer ) {
			if( Main.netMode != 1 ) {
				this.RefreshOwnerWho();
			}

			CustomEntityCore core = this.Core;
			byte owner_who = this.OwnerPlayerWho == -1 ? (byte)255 : (byte)this.OwnerPlayerWho;

			writer.Write( (ushort)CustomEntityManager.GetIdByTypeName(this.GetType().Name) );
			writer.Write( (byte)owner_who );
//LogHelpers.Log( "WRITE id: "+this.ID+", name: "+core.DisplayName+", templates: "+ CustomEntityTemplates.TotalEntityTemplates());
//LogHelpers.Log( "WRITE2 who: "+core.whoAmI+", component count: "+this.Components.Count );

			writer.Write( (ushort)core.whoAmI );
			writer.Write( (string)core.DisplayName );
			writer.Write( (float)core.position.X );
			writer.Write( (float)core.position.Y );
			writer.Write( (short)core.direction );
			writer.Write( (ushort)core.width );
			writer.Write( (ushort)core.height );
			writer.Write( (float)core.velocity.X );
			writer.Write( (float)core.velocity.Y );

			for( int i=0; i<this.Components.Count; i++ ) {
				this.Components[i].WriteStreamForwarded( writer );
			}
//LogHelpers.Log( "WRITE "+this.ToString()+" pos:"+ core.position );
		}


		protected override void ReadStream( BinaryReader reader ) {
			int type_id = (int)(ushort)reader.ReadUInt16();
			byte owner_who = reader.ReadByte();
			int who = (ushort)reader.ReadUInt16();
			string display_name = (string)reader.ReadString();
			var pos = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
			};
			int dir = (short)reader.ReadInt16();
			int wid = (ushort)reader.ReadUInt16();
			int hei = (ushort)reader.ReadUInt16();
			var vel = new Vector2 {
				X = (float)reader.ReadSingle(),
				Y = (float)reader.ReadSingle()
			};

			Type ent_type = CustomEntityManager.GetTypeById( type_id );
			if( ent_type == null ) {
				throw new HamstarException( "!ModHelpers.CustomEntity.ReadStream - Invalid entity type id " + type_id );
			}

			Player plr = owner_who == (byte)255 ? null : Main.player[owner_who];

			var components = new List<CustomEntityComponent>();
			var core = new CustomEntityCore( display_name, wid, hei, pos, dir ) {
				whoAmI = who,
				velocity = vel
			};
			
//LogHelpers.Log( "READ id: "+this.ID+", name: "+core.DisplayName+", who: "+core.whoAmI+", total templates: "+ CustomEntityTemplates.TotalEntityTemplates());
//LogHelpers.Log( "READ2 new_ent: "+(new_ent==null?"null":"not null")+", component count: "+(new_ent==null?"null2":""+new_ent.Components.Count) );
			
			for( int i = 0; i < components.Count; i++ ) {
				components[i].ReadStreamForwarded( reader );
			}

//LogHelpers.Log( "READ "+new_ent.ToString()+" pos:"+new_ent.Core.position );
			this.CopyChangesFrom( core, components, plr );
		}
	}
}
