﻿using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	class TagDefinition {
		public string Tag;
		public string Category;
		public string Description;

		public TagDefinition( string tag, string category, string description ) {
			this.Tag = tag;
			this.Category = category;
			this.Description = description;
		}
	}




	/// @private
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		public readonly static TagDefinition[] Tags;

		static TagsMenuContextBase() {
			TagsMenuContextBase.Tags = TagsMenuContextBase.GetTags();
		}



		////////////////

		private static TagDefinition[] GetTags() {
			Func<string, string, string, TagDefinition> m =
				( tag, category, desc ) => new TagDefinition( tag, category, desc );

			var list = new List<TagDefinition> {
				//m( "Core Game",             "Mechanics: Adds a \"bullet hell\" mode, adds a stamina bar, removes mining, etc."),
				m( "Combat",                "Mechanics",	"Adds weapon reloading, dual-wielding, monster AI changes, etc."),
				m( "Crafting",              "Mechanics",    "Adds bulk crafting, UI-based crafting, item drag-and-drop crafting, etc."),
				m( "Movement",              "Mechanics",    "Gives super speed, adds dodging mechanics, adds unlimited flight, etc."),
				m( "Mining",                "Mechanics",    "Adds fast tunneling, area mining, shape cutting, etc."),
				m( "Building",              "Mechanics",    "Add fast building options, measuring tools, world editing, etc."),
				m( "Traveling",             "Mechanics",    "Adds fast travel options, new types of minecarts/travel-mounts, etc."),
				m( "Misc. Interactions",    "Mechanics",    "Adds new block interactions, fishing mechanics, NPC dialogues, etc."),
				m( "Item Storage",          "Mechanics",    "Affects player inventory, centralizes chests, adds piggy banks, etc." ),
				m( "Item Equiping",         "Mechanics",    "Dual wielding, additional accessory slots, equipment management, etc."),
				m( "Item Stats",            "Mechanics",    "Adjusts item damage, defense, price, etc." ),
				m( "Item Behavior",         "Mechanics",    "Changes item projectile type or quantity, item class, equipability, etc." ),
				m( "Player State",          "Mechanics",	"Applies (de)buff-like effects; resistances/weaknesses, terrain access/hindrance, etc." ),
				m( "Player Class(es)",      "Mechanics",    "Adds or edits player 'classes'; custom damage types or abilities/strengths."),
				m( "Player Stats",          "Mechanics",    "Modifies player attack, defense, and other intrinsic elements (minion slots, etc.)."),
				m( "NPC Stats",             "Mechanics",    "Modifies NPC attack, defense, and other intrinsic elements."),
				m( "NPC Behavior",          "Mechanics",    "Modifies NPC AIs or state for new or altered behaviors."),

				m( "Informational",         "General",	"Adds game state reports (time, weather, progress, scores, etc.)."),
				m( "Specialized",           "General",	"Focuses on one main specific, well-defined function."),
				m( "Technical",             "General",	"May require a brain."),
				//{ "Multi-faceted",         "General",	"Does more than one thing, whether focusing mainly on one thing or not."),
				m( "Replacements",          "General",	"Primarily meant as an alternative to something the game already provides."),
				//{ "Esoteric",              "General",	"Does something uncommon or unexpected. Likely one-of-a-kind."),
				//{ "Visuals",               "General",	"Implements new or improved sprites, adds new background details, etc."),
				m( "MP Compatible",         "MP",	"Built for multiplayer."),
				m( "PvP",                   "MP",	"Player vs player (multiplayer)."),
				m( "Coop",                  "MP",	"Requires or involves direct player-to-player cooperation (multiplayer)."),
				m( "Teams",                 "MP",	"Requires or involves teams of players (multiplayer)."),
				m( "Server Use",            "MP",	"Player management tools, permissions, game rule changes, scheduled events, etc."),

				////
					
				m( "Open Source",           "State",		"Freely available source code."),
				m( "Has Documentation",     "State",        "Has an associated wiki or other comprehensive information source."),
				m( "Unmaintained",          "State",        "No longer receives version updates."),
				m( "Unfinished",            "State",        "Has missing or partially-working features."),
				m( "Buggy",                 "State",        "Does unexpected or erroneous things."),
				m( "Non-functional",        "State",        "Does not work (for its main use)."),
				//{ "Made By Team",			"" },
				//{ "Simplistic",			"" },
				//{ "Minimalistic",			"" },
				//{ "Shows Effort",			"" },
				//{ "Polished",				"" },
					
				////
				
				m( "Game Mode(s)",          "General",	"New game rules; added end goals, progression, setting, session, etc."),
				m( "Changes Genre",         "General",  "Adds linear progression, includes a SHMUP sequence, removes combat gameplay, etc."),
				m( "Quests",                "General",  "Adds goals for player to progress the game or gain profit from."),
				m( "Creativity",            "General",  "Emphasizes building or artistic expression (as opposed to fighting and adventuring)."),
				m( "Adds Convenience",      "General",  "Reduces annoyances; auto-trashes junk items, centralizes storage, etc."),
				m( "Cheat-like",            "General",  "Significantly reduces or removes some game challenges; may be 'unfair'."),
				m( "Challenge",             "General",  "Increases difficulty of specific elements: Time limits, harder boss AI, etc."),
				//m( "Easings",               "General",	"Decreases difficulty of specific elements: Stronger weapons, added player defense, etc."),
				m( "Restrictions",          "General",  "Limits or removes elements of the game; may make things easier or harder."),
				//m( "Vanilla Balanced",      "General",	"Balanced around plain Terraria; progress will not happen faster than usual."),
				m( "Loosely Balanced",      "General",  "Inconsistent or vague attempt to maintain balance, vanilla or otherwise."),
				m( "Plus Balanced",         "General",  "Balanced in excess of vanilla; expect sequence breaks (e.g. killing powerful bosses early)."),
				m( "Spoilers",              "General",  "Reveals information in advance about game or story elements, especially prematurely."),
				m( "Needs New World",       "General",  "Playing an existing world is difficult, problematic, or just impossible."),
				m( "Needs New Player",      "General",  "Character must begin as a blank slate, similarly."),
				m( "Affects World",         "General",  "Adds set pieces, alters biome shapes, adds new types of growth, etc."),
				m( "Affects Game State",    "General",  "Alters shop prices, activates invasion events, changes weather, etc."),
				m( "Mod Interacting",       "General",  "Supplies data, alters behavior, provides APIs, or manages other mods."),
				m( "Mod Collab",            "General",  "May be specifically paired with (an)other mod(s) to create a more-than-sum-of-parts result."),
				m( "May Lag",               "General",  "May use system resources or network bandwidth heavily. Good computer recommended."),
				m( "Adds UI",               "General",  "Adds user interface components for mod functions."),
				m( "Configurable",          "General",  "Provides options for configuring game settings (menu, config file, commands, etc.)."),
				m( "Misleading Info",       "General",  "Contains bad or missing information (e.g. poor mod description, no homepage, etc.)."),
				//{ "Rated R",               "General",	"Guess." },

				m( "Accesses System",       "Priviledge",	"Accesses files, opens programs, uses system functions, etc."),
				m( "Accesses Web",          "Priviledge",	"Makes web requests to send or receive data."),
				//{ "Injects Code",           make("Priviledge",	"Uses Reflection, swaps methods, or invokes libraries that do these."),
				
				////

				//{ "Item Sets",             "Content",	"Adds or edits discrete (often themed) sets or types of items."),
				m( "Weapons",               "Content",	"Adds or edits weapon items."),
				m( "Tools",                 "Content",  "Adds or edits tool items."),
				m( "Armors",                "Content",  "Adds or edits armor items in particular."),
				m( "Buffs & Pots.",         "Content",  "Adds or edits buffs and potion items."),
				m( "Accessories",           "Content",  "Adds or edits player accessory items (includes wings)."),
				m( "Mounts & Familiars",    "Content",  "Adds or edits player mounts or gameplay-affecting 'pets'."),
				m( "Vanity",                "Content",  "Adds or edits player vanity items, dyes, or non-gameplay pets."),
				m( "Ores",                  "Content",  "Adds mineable ores (and probably matching equipment tiers)."),
				m( "Recipes",               "Content",  "Adds or edits recipes beyond the expected minimum, or provides recipe information."),
				m( "Hostile NPCs",          "Content",  "Adds or edits hostile NPCs (monsters)."),
				m( "Town NPCs",             "Content",  "Adds or edits town NPCs (villagers)."),
				m( "Critters",              "Content",  "Adds or edits (passive) biome fauna."),
				m( "Bosses",                "Content",  "Adds or edits boss monsters."),
				m( "Invasions & Events",    "Content",  "Adds or edits invasions or game events."),
				m( "Fishing",               "Content",  "Adds or edits tools for fishing or types of fish."),
				m( "Blocks",                "Content",  "Adds or edits new block types."),
				m( "Biomes",                "Content",  "Adds or edits world biomes."),
				m( "Decorative",            "Content",  "Adds or edits decorative objects (e.g. furniture for houses)."),
				m( "Wiring",                "Content",  "Adds or edits tools and toys for use for wiring."),
				m( "Music",                 "Content",  "Adds or edits music."),
				m( "Rich Art",              "Content",  "Adds extensive or detailed art for new or existing content."),
				m( "Sounds",                "Content",  "Adds or edits sound effects or ambience."),
				m( "Story or Lore",         "Content",  "Implements elements of story telling or universe lore."),
				m( "Special FX",            "Content",  "Adds gore effects, adds motion blurs, improves particle effects, etc."),

				m( "Dark",                  "Theme",    "Gloomy, edgy, or just plain poor visibility."),
				m( "Silly",                 "Theme",    "Light-hearted, immersion-breaking, or just plain absurd."),
				m( "Fantasy",               "Theme",    "Elements of mythologies, swords & sorcery, and maybe a hobbit or 2."),
				m( "Medieval",              "Theme",    "Chivalry, castles, primitive technology, melee fighting, archery, etc."),
				m( "Military",              "Theme",    "Guns and stuff."),
				m( "Nature",                "Theme",    "Birds, bees, rocks, trees, etc."),
				m( "Sci-Fi",                "Theme",    "Robots, lasers, flying machines, etc."),
				m( "Civilized",             "Theme",    "NPC interactions, town workings, player living spaces, etc."),
				//{ "Mixed",				"Theme",    "Mashup of genres, but not purely in a silly way."),
				//{ "Where: Surface",       "" },
				//{ "Where: Underground",	"" },
				//{ "Where: Ocean",			"" },
				//{ "Where: Dungeon",		"" },
				//{ "Where: Jungle",		"" },
				//{ "Where: Hell",			"" },
				//{ "Where: Evil Biome",	"" },
				//{ "Where: Hallow Biome",	"" },

				m( "The Beginning",         "When", "Focuses on time before any boss kills or invasion events."),
				m( "Bosses Begun",          "When", "Focuses on time after player has begun killing bosses (world now mostly accessible)."),
				m( "Post-BoC & EoW",        "When", "Focuses on time after corruption/crimson conquered. Underworld accessible."),
				m( "Hard Mode",             "When", "Focuses on time after Wall of Flesh conquered (hallowed+evil biomes, harder monsters)."),
				m( "Post-Mech bosses",      "When", "Focuses on time after Mech bosses conquered (restless jungle)."),
				m( "Post-Plantera",         "When", "Focuses on time after Plantera conquered. Temple accessible."),
				m( "Post-Moonlord",         "When", "Focuses on time after Moon Lord killed."),
				m( "Contextual",            "When", "Concerns with a specific, discrete event or (non-boss) game context.")
			};

			if( !ModHelpersMod.Instance.Config.DisableJudgmentalTags ) {
				list.Add( m( "Unimaginative",		"Judgmental",	"Nothing special; exceedingly common, generic, or flavorless.") );
				list.Add( m( "Low Effort",			"Judgmental",	"Evident lack of effort involved.") );
				list.Add( m( "Unoriginal Content",	"Judgmental",	"Contains stolen or extensively-derived content.") );
			}

			return list.ToArray();
		}
	}
}
