﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNetHelpers {
	public partial class ReflectionHelpers {
		private MemberInfo GetCachedInfoMember( Type classType, string fieldOrPropName ) {
			string className = classType.FullName;
			MemberInfo result;

			if( !this.FieldPropMap.ContainsKey( className ) ) {
				this.FieldPropMap[ className ] = new Dictionary<string, MemberInfo>();
			}

			if( !this.FieldPropMap[className].ContainsKey( fieldOrPropName ) ) {
				result = (MemberInfo)classType.GetField( fieldOrPropName, ReflectionHelpers.MostAccess );
				if( result == null ) {
					result = (MemberInfo)classType.GetProperty( fieldOrPropName, ReflectionHelpers.MostAccess );
				}
				if( result == null ) {
					return null;
				}

				this.FieldPropMap[ className ][ fieldOrPropName ] = result;
			}

			return this.FieldPropMap[ className ][ fieldOrPropName ];
		}
	}
}
