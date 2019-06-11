﻿using HamstarHelpers.Components.DataStructures;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Helpers.Buffs {
	public partial class BuffIdentityHelpers {
		public static ReadOnlyDictionaryOfSets<string, int> NamesToIds {
			get { return ModHelpersMod.Instance.BuffIdentityHelpers._NamesToIds; }
		}
	}
}