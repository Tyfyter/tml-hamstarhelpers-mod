﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Libraries.Recipes {
	/// <summary>
	/// Assorted static "helper" functions pertaining to recipe finding.
	/// </summary>
	public partial class RecipeFinderLibraries {
		/// <summary>
		/// Gets the `Main.recipe` indexes of each recipe that crafts a given item.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesOfItem( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new ModHelpersException( "Invalid item type" );
			}

			var mymod = ModHelpersMod.Instance;
			IDictionary<int, ISet<int>> recipeIdxLists = mymod.RecipeFinderHelpers.RecipeIndexesByItemNetID;
			
			lock( RecipeFinderLibraries.MyLock ) {
				if( recipeIdxLists.Count == 0 ) {
					mymod.RecipeFinderHelpers.CacheItemRecipes();
				}
				return recipeIdxLists.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
		
		/// <summary>
		/// Gets each `Recipe` that crafts a given item.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static IList<Recipe> GetRecipesOfItem( int itemNetID ) {
			return RecipeFinderLibraries.GetRecipeIndexesOfItem( itemNetID )
				.Select( idx=>Main.recipe[idx] )
				.ToList();
		}


		////////////////

		/// <summary>
		/// Gets each `Main.recipe` index of each recipe that uses a given item as an ingredient.
		/// </summary>
		/// <param name="itemNetID"></param>
		/// <returns></returns>
		public static ISet<int> GetRecipeIndexesUsingIngredient( int itemNetID ) {
			if( itemNetID == 0 ) {
				throw new ModHelpersException( "Invalid item type" );
			}

			var mymod = ModHelpersMod.Instance;
			IDictionary<int, ISet<int>> recipeIdxSets = mymod.RecipeFinderHelpers.RecipeIndexesOfIngredientNetIDs;

			lock( RecipeFinderLibraries.MyLock ) {
				if( recipeIdxSets.Count == 0 ) {
					mymod.RecipeFinderHelpers.CacheIngredientRecipes();
				}
				return recipeIdxSets.GetOrDefault( itemNetID )
					?? new HashSet<int>();
			}
		}
	}
}
