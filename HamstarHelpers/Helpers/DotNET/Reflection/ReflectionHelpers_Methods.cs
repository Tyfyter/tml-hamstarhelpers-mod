﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Linq;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers {
		/// <summary>
		/// Invokes a method, first validating the supplied parameters for type consistency.
		/// </summary>
		/// <param name="method">Reflected method to invoke.</param>
		/// <param name="methodContext">Object instance containing of the given method. Use `null` for static methods.</param>
		/// <param name="args">Parameters to pass to the given method.</param>
		/// <returns>Results from invoking the method.</returns>
		public static object SafeCall( MethodInfo method, object methodContext, object[] args ) {
			var paramInfos = method.GetParameters();
			
			if( args.Length != paramInfos.Length ) {
				throw new ModHelpersException( "Mismatched input argument quantity. (for call " + method.Name + ")" );
			}
			
			for( int i = 0; i < paramInfos.Length; i++ ) {
				Type paramType = paramInfos[i].ParameterType;
				Type argType = args[i].GetType();

				if( args[i] == null ) {
					if( !paramType.IsClass || paramInfos[i].GetCustomAttribute<NullableAttribute>() == null ) {
						throw new ModHelpersException( "Invalid param "+paramInfos[i].Name+" (#"+i+"): Expected "+paramType.Name+", found null (for call "+method.Name+")" );
					}
				} else if( argType.Name != paramType.Name && !argType.IsSubclassOf( paramType ) ) {
					throw new ModHelpersException( "Invalid param " + paramInfos[i].Name+" (#"+i+"): Expected "+paramType.Name+", found "+argType.Name+" (for call "+method.Name+")" );
				}
			}

			return method.Invoke( methodContext, args );
		}

		////////////////

		/// <summary>
		/// Invokes a method of a given class.
		/// </summary>
		/// <typeparam name="T">Invoked method's return value type.</typeparam>
		/// <param name="instance">Class instance of class of method.</param>
		/// <param name="methodName">Method's name.</param>
		/// <param name="args">Method's arguments (will be validated before invoking).</param>
		/// <param name="returnVal">Return value of method.</param>
		/// <returns>`true` if method found and invoked successfully.</returns>
		public static bool RunMethod<T>( Object instance, string methodName, object[] args, out T returnVal ) {
			if( instance == null ) {
				returnVal = default( T );
				return false;
			}
			return ReflectionHelpers.RunMethod<T>( instance.GetType(), instance, methodName, args, out returnVal );
		}
		
		/// <summary>
		/// Invokes a method of a given class. May invoke static methods if the given `instance` parameter is `null`.
		/// </summary>
		/// <typeparam name="T">Invoked method's return value type.</typeparam>
		/// <param name="classType">Class type of method.</param>
		/// <param name="instance">Class instance of class of method. Use `null` for static methods.</param>
		/// <param name="methodName">Method's name.</param>
		/// <param name="args">Method's arguments (will be validated before invoking).</param>
		/// <param name="returnVal">Return value of method.</param>
		/// <returns>`true` if method found and invoked successfully.</returns>
		public static bool RunMethod<T>( Type classType, Object instance, string methodName, object[] args, out T returnVal ) {
			return ReflectionHelpers.RunMethod<T>( classType, instance, methodName, args, new Type[] { }, out returnVal );
		}

		/// <summary>
		/// Invokes a method of a given class. May invoke static methods if the given `instance` parameter is `null`.
		/// </summary>
		/// <typeparam name="T">Invoked method's return value type.</typeparam>
		/// <param name="classType">Class type of method.</param>
		/// <param name="instance">Class instance of class of method. Use `null` for static methods.</param>
		/// <param name="methodName">Method's name.</param>
		/// <param name="args">Method's arguments (will be validated before invoking).</param>
		/// <param name="generics">Generic type parameters (if applicable).</param>
		/// <param name="returnVal">Return value of method.</param>
		/// <returns>`true` if method found and invoked successfully.</returns>
		public static bool RunMethod<T>(
					Type classType,
					Object instance,
					string methodName,
					object[] args,
					Type[] generics,
					out T returnVal ) {
			returnVal = default( T );

			Type[] paramTypes = args?.SafeSelect( o => o.GetType() ).ToArray()
				?? new Type[] { };

			/*for( int i=0; i<paramTypes.Length; i++ ) {
				Type p = paramTypes[i];
				if( p != typeof(ReflectionParameter) ) {
					continue;
				}

				var refP = args[i] as ReflectionParameter;
				//Type.MakeGenericSignatureType
			}*/

			MethodInfo method = classType.GetMethod(
				name: methodName,
				bindingAttr: ReflectionHelpers.MostAccess,
				binder: null,
				types: paramTypes,
				modifiers: null
			);

			if( method == null ) {
				if( classType.BaseType != null && classType.BaseType != typeof(object) ) {
					return ReflectionHelpers.RunMethod<T>(
						classType: classType.BaseType,
						instance: instance,
						methodName: methodName,
						args: args,
						generics: generics,
						returnVal: out returnVal
					);
				}
				return false;
			}

			if( generics.Length > 0 ) {
				method = method.MakeGenericMethod( generics );
			}

			returnVal = (T)ReflectionHelpers.SafeCall( method, instance, args );
			return true;
		}
	}
}