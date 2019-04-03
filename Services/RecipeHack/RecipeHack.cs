﻿using HamstarHelpers.Helpers.RecipeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Services.RecipeHack {
	public partial class RecipeHack {
		public static void RegisterIngredientSource( string sourceName, Func<Player, IEnumerable<Item>> itemSource ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources[ sourceName ] = itemSource;
		}

		public static void UnregisterIngredientSource( string sourceName ) {
			var mymod = ModHelpersMod.Instance;
			mymod.RecipeHack.IngredientOutsources.Remove( sourceName );
		}


		////////////////

		public static IEnumerable<Item> GetOutsourcedItems( Player player ) {
			return ModHelpersMod.Instance.RecipeHack.IngredientOutsources.Values
				.SelectMany( src => src( player ) );
		}


		////////////////

		public static IList<int> GetAvailableRecipesOfIngredients( Player player, IEnumerable<Item> ingredients ) {
			int[] _;
			IDictionary<int, int> __;
			IList<int> addedRecipeIndexes = new List<int>();
			ISet<int> possibleRecipeIdxs = new HashSet<int>();

			foreach( Item ingredient in ingredients ) {
				IEnumerable<int> ingredientRecipeIdxs = RecipeIdentityHelpers.GetRecipeIndicesOfItem( ingredient.netID );
				possibleRecipeIdxs.UnionWith( ingredientRecipeIdxs );
			}

			foreach( int recipeIdx in possibleRecipeIdxs ) {
				Recipe recipe = Main.recipe[recipeIdx];
				if( recipe.createItem.type == 0 ) { continue; } // Just in case?

				if( RecipeHelpers.GetRecipeFailReasons( player, recipe, out _, out __, ingredients ) == 0 ) {
					addedRecipeIndexes.Add( recipeIdx );
				}
			}

			return addedRecipeIndexes;
		}
	}
}
