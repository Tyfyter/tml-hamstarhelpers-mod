﻿using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal PacketProtocolDataConstructorLock() { }
	}




	/// <summary>
	/// Provides a way to automatically ensure order of fields for transmission.
	/// </summary>
	public abstract partial class PacketProtocolData {
		internal static PacketProtocolData CreateData( Type protocol_type ) {
			//var item = (PacketProtocolData)Activator.CreateInstance( field_type, true );
			return (PacketProtocolData)Activator.CreateInstance( protocol_type, BindingFlags.NonPublic | BindingFlags.Instance,
				null, new object[] { HamstarHelpersMod.Instance.PacketProtocolCtorLock }, null );
		}


		////////////////

		private IOrderedEnumerable<FieldInfo> _OrderedFields = null;

		internal IOrderedEnumerable<FieldInfo> OrderedFields {
			get {
				if( this._OrderedFields == null ) {
					Type mytype = this.GetType();
					FieldInfo[] fields = mytype.GetFields( BindingFlags.Public | BindingFlags.Instance );

					this._OrderedFields = fields.OrderByDescending( x => x.Name );  //Where( f => f.FieldType.IsPrimitive )
				}
				return this._OrderedFields;
			}
		}


		////////////////

		/// <summary>
		/// Implements internal low level data reading for packet receipt.
		/// </summary>
		/// <param name="reader">Binary data reader.</param>
		protected virtual void ReadStream( BinaryReader reader ) {
			PacketProtocolData.ReadStreamIntoContainer( reader, this );
		}


		////////////////

		/// <summary>
		/// Implements low level stream output for packet output.
		/// </summary>
		/// <param name="writer">Binary data writer.</param>
		protected virtual void WriteStream( BinaryWriter writer ) {
			PacketProtocolData.WriteStreamFromContainer( writer, this );
		}
	}
}