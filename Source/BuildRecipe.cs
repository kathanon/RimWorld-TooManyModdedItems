using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class BuildRecipe : Recipe {
        public readonly ThingDef Def;

        public readonly Category<Ingredient> Material   = new Category<Ingredient>("Materials:");
        public readonly Category<Research>   Researches = new Category<Research>("Required research:");

        public BuildRecipe(ThingDef def) {
            Def = def;
        }

        protected override List<Category> GetCategories() {
            var list = new List<Category> { Material };
            if (Researches.Any) {
                list.Add(Researches);
            }
            return list;
        }
    }
}
