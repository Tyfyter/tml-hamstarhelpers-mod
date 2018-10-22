﻿using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public static IDictionary<string, string> Tags {
			get {
				var dict = new Dictionary<string, string> {
					{ "Core Game",              "Mechanics: Adds a \"bullet hell\" mode, adds a stamina bar, removes mining, etc." },
					{ "Combat",                 "Mechanics: Adds weapon reloading, dual-wielding, changes mob behaviors, etc." },
					{ "Movement",               "Mechanics: Gives super speed, adds dodging mechanics, adds unlimited flight, etc." },
					{ "Mining",                 "Mechanics: Adds mining tools or makes specific types of excavation faster." },
					{ "Building",               "Mechanics: Add fast building options, measuring tools, world editing, etc." },
					{ "Traveling",              "Mechanics: Adds fast travel options, new minecarts/travel-mounts, etc." },
					{ "Item Storage",           "Mechanics: Affects player inventory, centralizes chests, adds piggy banks, etc." },
					{ "Item Equiping",          "Mechanics: Dual wielding, additional accessory slots, equipment management, etc." },
					{ "Item Stats",             "Mechanics: Adjusts item damage, defense, price, etc." },
					{ "Item Behavior",          "Mechanics: Changes item projectile type or quantity, item class, equipability, etc." },
					{ "Player Class(es)",       "Mechanics: Adds new player 'classes'; custom damage types or special attacks/defenses." },
					{ "Player Stats",           "Mechanics: Modifies player attack, defense, and other intrinsic elements." },
					{ "NPC Stats",              "Mechanics: Modifies NPC attack, defense, and other intrinsic elements." },
					{ "NPC Behavior",           "Mechanics: Modifies NPC AIs or state for new or altered behaviors." },

					{ "Creativity",             "Adds decorations, adds scenery or colors FX, adds tools for building, etc." },
					{ "Quests",                 "Adds goals for player to progress the game or gain profit from." },
					{ "Game Mode(s)",           "New game rules; added end goals, progression, setting, session, etc." },
					{ "Story or Lore",          "Implements elements of story telling or universe lore." },
					{ "Informational",          "Adds game state reports (time, weather, etc.), reports game stats, etc." },
					{ "Specialized",            "Focuses on one main specific, well-defined function." },
					{ "Replacements",           "Primarily meant as an alternative to something the game already provides." },
					{ "Multi-faceted",          "Does more than one thing, whether focusing mainly on one thing or not." },
					//{ "Visuals",                "Implements new or improved sprites, adds new background details, etc." },
					{ "Special FX",             "Adds gore effects, adds motion blurs, improves particle effects, etc." },
					{ "Affects World",          "Adds set pieces, alters biome shapes, adds new types of growth, etc." },
					{ "Affects Game State",     "Altars shop prices, activates invasion events, changes weather, etc." },
					{ "Esoteric",               "Does something uncommon or unexpected. Likely one-of-a-kind." },
					{ "Adds Convenience",       "Reduces annoyances; auto-trashes junk items, centralizes storage, etc." },
					{ "MP Compatible",          "Built for multiplayer." },
					{ "PvP",                    "Player vs player." },
					{ "Coop",                   "Requires or involves direct player cooperation." },
					{ "Teams",                  "Requires or involves teams of players." },

					////

					{ "Open Source",            "State: Freely available source code." },
					{ "Unmaintained",           "State: No longer receives version updates." },
					{ "Unfinished",             "State: Has missing or partially-working features." },
					{ "Non-functional",         "State: Does not work for its intended purpose." },
					{ "Buggy",                  "State: Does unexpected or erroneous things." },
					//{ "Made By Team",			"" },
					//{ "Simplistic",			"" },
					//{ "Minimalistic",			"" },
					//{ "Shows Effort",			"" },
					//{ "Polished",				"" },

					{ "Music",                  "Content: Adds or edits music." },
					{ "Rich Art",               "Content: Adds extensive or detailed art for new or existing game entities." },
					{ "Sounds",                 "Content: Adds or edits sound effects or ambience." },
					{ "Item Sets",              "Content: Adds or edits discrete (often themed) sets of items." },
					{ "Weapons",                "Content: Adds or edits weapons." },
					{ "Hostile NPCs",           "Content: Adds or edits hostile NPCs (monsters)." },
					{ "Town NPCs",              "Content: Adds or edits town NPCs (villagers)." },
					{ "Critters",               "Content: Adds or edits (passive) biome fauna." },
					{ "Bosses",                 "Content: Adds or edits boss monsters." },
					{ "Invasions",              "Content: Adds or edits invasion events." },
					{ "Blocks",                 "Content: Adds or edits new block types." },
					{ "Wiring",                 "Content: Adds or edits tools and toys for use for wiring." },
					{ "Decorative",             "Content: Adds or edits decorative objects (e.g. furniture for houses)." },
					{ "Fishing",                "Content: Adds or edits tools or mechanics for fishing." },
					{ "Buffs & Pots.",          "Content: Adds or edits buffs and potions." },
					{ "Accessories",            "Content: Adds or edits player accessories." },
					{ "Biomes",                 "Content: Adds or edits world biomes." },
					{ "Vanity",                 "Content: Adds or edits player vanity items or options." },
					{ "Ores",                   "Content: Adds mineable ores (and probably matching equipment tiers)." },

					{ "Dark",                   "Theme: Gloomy, edgy, or just plain poor visibility." },
					{ "Silly",                  "Theme: Light-hearted, immersion-breaking, or just plain absurd." },
					{ "Fantasy",                "Theme: Elements of mythologies, swords & sorcery, and maybe a hobbit or 2." },
					{ "Medieval",               "Theme: Chivalry, castles, primitive technology, melee fighting, archery, etc." },
					{ "Military",               "Theme: Guns and stuff." },
					{ "Nature",                 "Theme: Birds, bees, rocks, trees, etc." },
					{ "Sci-Fi",                 "Theme: Robots, lasers, flying machines, etc." },
					{ "Civilized",              "Theme: NPC interactions, town workings, player living spaces, etc." },
					//{ "Mixed",				"Theme: Mashup of genres, but not purely in a silly way." },
					//{ "Where: Surface",         "" },
					//{ "Where: Underground",		"" },
					//{ "Where: Ocean",			"" },
					//{ "Where: Dungeon",			"" },
					//{ "Where: Jungle",			"" },
					//{ "Where: Hell",			"" },
					//{ "Where: Evil Biome",		"" },
					//{ "Where: Hallow Biome",	"" },

					{ "From Beginning",         "When: Before any boss kills or invasion events." },
					{ "Bosses Begun",           "When: Player has begun killing bosses. World generally accessible." },
					{ "Post-BoC & EoW",         "When: Corruption/crimson conquered. Underworld accessible." },
					{ "Hard Mode",              "When: Wall of Flesh conquered, large hallowed and evil biomes, harder monsters." },
					{ "Post-Mech bosses",       "When: Mech bosses conquered, restless jungle." },
					{ "Post-Plantera",          "When: Plantera conquered. Temple accessible." },
					{ "Post-Moonlord",          "When: Moon Lord killed. Only mods can pose a challenge now." },
					{ "Contextual",             "When: Kicks in when a specific event or gameplay trigger occurs." },
			
					////
			
					{ "Cheat-like",             "Significantly reduces or removes some game challenges; may be 'unfair'." },
					{ "Challenge",              "Increases difficulty of something, e.g. for player bragging rights." },
					{ "Nerfs",                  "Decreases difficulty of something, e.g. to make beating bosses easier." },
					{ "Vanilla Balanced",       "Balanced around plain Terraria; progress will not happen faster than usual." },
					{ "Loosely Balanced",       "Inconsistent or vague attempt to maintain balance, vanilla or otherwise." },
					{ "Plus Balanced",          "Balanced in excess of vanilla; expect sequence breaks (e.g. killing powerful bosses early)." },
					{ "Needs New World",        "Playing from the beginning is difficult, problematic, or just impossible." },
					{ "Needs New Player",       "Character must begin as a blank slate, similarly." },
					{ "Mod Interacting",        "Supplies data, alters behavior, provides APIs, or manages other mods." },
					{ "Mod Collab",             "May be paired with (an)other mod(s) to create a more-than-sum-of-parts result." },
					{ "Server Use",             "Affects servers. Admin tools, scheduled events, game rule changes, etc." },
					{ "May Lag",                "May use system resources or network bandwidth heavily. Good computer recommended." },
					{ "Adds UI",                "Adds user interface components for mod functions." },
					{ "Configurable",           "Provides options for configuring game settings (menu, config file, commands, etc.)." },
					{ "Technical",              "May require a brain." },
					{ "Rated R",                "Guess." },
				};

				if( !ModHelpersMod.Instance.Config.DisableJudgmentalTags ) {
					dict[ "Misleading Info" ] =		"Judgmental: Not what it says on the tin or contains missing information.";
					dict[ "Unoriginal Content" ] =	"Judgmental: Contains stolen or extensively-derived content.";
				}

				return dict;
			}
		}
	}
}