using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TooManyModdedItems {
    public class Item : RowEntry {
        public readonly ThingDef Def;
        public readonly List<Recipe> Recipes = new List<Recipe>();

        public Item(ThingDef def) {
            Def = def;
        }

        public bool ListItem => Recipes.Count > 0;

        public override ThingDef IconThing => Def;

        public override string Label => Def.LabelCap;

        public override Item ClickedItem => this;
    }
}
