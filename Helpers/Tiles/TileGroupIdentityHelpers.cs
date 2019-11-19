﻿using HamstarHelpers.Classes.DataStructures;
using System;
using System.Collections.Generic;
using Terraria.ID;


namespace HamstarHelpers.Helpers.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile group identification.
	/// </summary>
	public class TileGroupIdentityHelpers {
		/// <summary>
		/// Tile types that typically comprise "earth".
		/// </summary>
		public static IReadOnlySet<int> VanillaEarthTiles { get; } = new ReadOnlySet<int>( new HashSet<int> {
			TileID.Stone,
			TileID.Dirt,
			TileID.ClayBlock,
			TileID.Mud,
			TileID.Grass,
			TileID.JungleGrass,
			TileID.MushroomGrass,
			TileID.BlueMoss,
			TileID.BrownMoss,
			TileID.GreenMoss,
			TileID.LavaMoss,
			TileID.LongMoss,
			TileID.PurpleMoss,
			TileID.RedMoss,
			TileID.Granite,
			TileID.Marble,
			///
			TileID.Ebonstone,
			TileID.HallowedGrass,
			TileID.CorruptGrass,
			TileID.Crimstone,
			TileID.FleshGrass,
			///
			TileID.SnowBlock,
			TileID.IceBlock,
			TileID.HallowedIce,
			TileID.FleshIce,
			TileID.CorruptIce,
			TileID.BreakableIce,
			///
			TileID.Sand,
			TileID.Pearlsand,
			TileID.Ebonsand,
			TileID.Crimsand,
			TileID.Sandstone,
			TileID.HallowSandstone,
			TileID.CorruptSandstone,
			TileID.CrimsonSandstone,
			TileID.HardenedSand,
			TileID.HallowHardenedSand,
			TileID.CorruptHardenedSand,
			TileID.CrimsonHardenedSand,
		} );
	}
}