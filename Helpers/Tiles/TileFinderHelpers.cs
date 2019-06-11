﻿using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Tiles {
	public static class TileFinderHelpers {
		public static bool HasNearbySolid( int tileX, int tileY, int squareRadius, bool isPlatformSolid = false, bool isActuatedSolid = false ) {
			int minX = Math.Max( tileX - squareRadius, 0 );
			int maxX = Math.Min( tileX + squareRadius, Main.maxTilesX - 1 );
			int minY = Math.Max( tileY - squareRadius, 0 );
			int maxY = Math.Min( tileY + squareRadius, Main.maxTilesY - 1 );

			for( int i = minX; i <= maxX; i++ ) {
				for( int j = minY; j <= maxY; j++ ) {
					if( TileHelpers.IsSolid( Main.tile[i, j], isPlatformSolid, isActuatedSolid ) ) {
						return true;
					}
				}
			}

			return false;
		}


		public static bool FindNearbyRandomAirTile( int tileX, int tileY, int radius, out int toX, out int toY ) {
			Tile tile = null;
			int wtf = 0;
			bool isBlocked = false;
			toX = 0;
			toY = 0;

			if( tileX + radius <= 0 || tileX - radius >= Main.mapMaxX ) { return false; }
			if( tileY + radius <= 0 || tileY - radius >= Main.mapMaxY ) { return false; }

			do {
				do { toX = Main.rand.Next( -radius, radius ) + tileX; }
				while( toX <= 0 || toX >= Main.mapMaxX );
				do { toY = Main.rand.Next( -radius, radius ) + tileY; }
				while( toY <= 0 || toY >= Main.mapMaxY );

				//tile = Main.tile[toX, toY];
				tile = Framing.GetTileSafely( toX, toY );
				if( wtf++ > 100 ) {
					return false;
				}

				bool _;
				isBlocked = TileHelpers.IsSolid( tile, true, true ) ||
					TileWallHelpers.IsDungeon( tile, out _ ) ||
					TileHelpers.IsWire( tile ) ||
					tile.lava();
			} while( isBlocked && ((tile != null && tile.type != 0) || Lighting.Brightness( toX, toX ) == 0) );

			return true;
		}


		////

		public static IDictionary<int, int> GetPlayerRangeTilesAt( int midTileX, int midTileY ) {
			var tiles = new Dictionary<int, int>();

			float minScreenTileWidth = 50f;
			float minScreenTileHeight = 37.5f;
			float offscreenTiles = 23f;
			/*if( Lighting.lightMode < 2 ) {
				offscreenTiles = 40;
			} else {
				offscreenTiles = 23;
			}*/

			int leftTileX = midTileX - (int)((offscreenTiles + minScreenTileWidth) * 0.5f) - 1;
			int topTileY = midTileY - (int)((offscreenTiles + minScreenTileHeight) * 0.5f) - 1;
			int rightTileX = midTileX + (int)((offscreenTiles + minScreenTileWidth) * 0.5f) + 1;
			int botTileY = midTileY + (int)((offscreenTiles + minScreenTileHeight) * 0.5f) + 1;
			leftTileX = Utils.Clamp<int>( leftTileX, 5, Main.maxTilesX - 1 );
			topTileY = Utils.Clamp<int>( topTileY, 5, Main.maxTilesY - 1 );
			rightTileX = Utils.Clamp<int>( rightTileX, 5, Main.maxTilesX - 1 );
			botTileY = Utils.Clamp<int>( botTileY, 5, Main.maxTilesY - 1 );

			//int width = lastLightX - firstLightX;
			//int height = lastLightY - firstLightY;
			//
			//int biomeOffsetX = Math.Abs( ( width - Main.zoneX ) / 2 );
			//int biomeOffsetY = Math.Abs( ( height - Main.zoneY ) / 2 );
/*LogHelpers.Log("firstLightX:"+firstLightX+", firstLightY:"+firstLightY
	+", lastLightX:"+lastLightX+", lastLightY:"+lastLightY
	+", width:"+width+", height:"+height
	+", biomeOffsetX:"+biomeOffsetX+", biomeOffsetY:"+biomeOffsetX);*/

			Tile tile;
			for( int x = leftTileX; x < rightTileX; x++ ) {
				for( int y = topTileY; y < botTileY; y++ ) {
					tile = Main.tile[x, y];
					if( tile == null || !tile.active() ) { continue; }
					
					if( !tiles.ContainsKey( tile.type ) ) {
						tiles[ tile.type ] = 1;
					} else {
						tiles[ tile.type ] += 1;
					}
				}
			}

			return tiles;
		}
	}
}