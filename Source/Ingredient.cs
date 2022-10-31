using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class Ingredient : RowEntry {
        private readonly string label;
        private readonly ThingDef icon;
        private readonly Item target;

        public Ingredient(IngredientCount ingredientCount, RecipeDef recipe, Database db) {
            label = ingredientCount.SummaryFor(recipe);
            if (ingredientCount.IsFixedIngredient) {
                icon = ingredientCount.FixedIngredient;
                target = db[icon];
            } else {
                target = null;
                icon = null; // TODO: category icons?
            }
        }

        public Ingredient(ThingDefCountClass thingCount, Item target) {
            label = thingCount.Summary;
            icon = thingCount.thingDef;
            this.target = target;
        }

        public Ingredient(ThingDef stuffFrom) {
            string stuff = stuffFrom.stuffCategorySummary;
            if (stuff.NullOrEmpty() && !stuffFrom.stuffCategories.NullOrEmpty()) {
                stuff = stuffFrom.stuffCategories
                    .Select(c => c.noun)
                    .Where(n => !n.NullOrEmpty())
                    .ToCommaListOr(true);
            }
            label = $"{stuffFrom.CostStuffCount}x {stuff}";
            icon = null;
            target = null;
        }

        public override ThingDef IconThing => icon;

        public override string Label => label;

        public override Item ClickedItem => target;
    }
}
