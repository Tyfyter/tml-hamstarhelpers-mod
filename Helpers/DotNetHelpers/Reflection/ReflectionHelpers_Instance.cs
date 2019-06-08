﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers.Reflection {
	public partial class ReflectionHelpers {
		public static ReflectionHelpers Instance => ModHelpersMod.Instance.ReflectionHelpers;



		////////////////

		private IDictionary<string, IDictionary<string, IList<Type>>> AssClassTypeMap = new ConcurrentDictionary<string, IDictionary<string, IList<Type>>>();
		private IDictionary<string, IDictionary<string, MemberInfo>> FieldPropMap = new ConcurrentDictionary<string, IDictionary<string, MemberInfo>>();



		////////////////
		
		internal ReflectionHelpers() { }


		////////////////
		
		internal MemberInfo GetCachedInfoMember( Type classType, string fieldOrPropName ) {
			string className = classType.FullName;
			MemberInfo result;

			if( !this.FieldPropMap.ContainsKey( className ) ) {
				this.FieldPropMap[className] = new Dictionary<string, MemberInfo>();
			}

			if( !this.FieldPropMap[className].ContainsKey( fieldOrPropName ) ) {
				result = (MemberInfo)classType.GetField( fieldOrPropName, ReflectionHelpers.MostAccess );
				if( result == null ) {
					result = (MemberInfo)classType.GetProperty( fieldOrPropName, ReflectionHelpers.MostAccess );
				}
				if( result == null ) {
					return null;
				}

				this.FieldPropMap[className][fieldOrPropName] = result;
			}

			return this.FieldPropMap[className][fieldOrPropName];
		}
	}
}