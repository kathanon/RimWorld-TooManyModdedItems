using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class CraftRecipe : Recipe {
        public readonly RecipeDef Def;

        public readonly Category<Ingredient> Ingredients = new Category<Ingredient>("Ingredients:");
        public readonly Category<Product>    Products    = new Category<Product>("Products:");
        public readonly Category<Item>       Workbenches = new Category<Item>("Craft at:");
        public readonly Category<Research>   Researches  = new Category<Research>("Required research:");

        public CraftRecipe(RecipeDef def) {
            Def = def;
        }

        public override void AddWorkbench(Item bench) => Workbenches.Add(bench);
        public override void PostInit() => Workbenches.Entries.SortBy(t => t.Def.label);

        protected override List<Category> GetCategories() {
            var list = new List<Category> { Ingredients, Products, Workbenches };
            if (Researches.Any) {
                list.Add(Researches);
            }
            return list;
        }
    }
}
