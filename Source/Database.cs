using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class Database {
        private Dictionary<ThingDef, Item> items;
        private Dictionary<RecipeDef, Recipe> recipes;
        
        public readonly List<Item> Items = new List<Item>();

        public Item this[ThingDef thing] => items[thing];
        public Recipe this[RecipeDef recipe] => recipes[recipe];

        public Database() {
            items = new Dictionary<ThingDef, Item>();
            foreach (var def in DefDatabase<ThingDef>.AllDefs) {
                if (def.label != null) {
                    var item = new Item(def);
                    items[def] = item;
                    Items.Add(item);
                }
            }

            recipes = new Dictionary<RecipeDef, Recipe>();
            foreach (var def in DefDatabase<RecipeDef>.AllDefs) {
                // TODO: Handle specialProducts as well
                if (!def.products.NullOrEmpty()) {
                    var recipe = new CraftRecipe(def);
                    recipes[def] = recipe;
                    foreach (var ingredient in def.ingredients.OrEmpty()) {
                        recipe.Ingredients.Add(new Ingredient(ingredient, def, this));
                    }
                    foreach (var thing in def.products.OrEmpty()) {
                        if (items.ContainsKey(thing.thingDef)) {
                            var item = items[thing.thingDef];
                            item.Recipes.Add(recipes[def]);
                            recipe.Products.Add(new Product(thing, item));
                        }
                    }
                    foreach (var thing in def.recipeUsers.OrEmpty()) {
                        if (items.ContainsKey(thing)) {
                            recipe.Workbenches.Add(items[thing]);
                        }
                    }
                    foreach (var res in def.researchPrerequisites.And(def.researchPrerequisite)) {
                        recipe.Researches.Add(new Research(res));
                    }
                }
            }

            foreach (var item in items.Values) {
                foreach (var recipe in item.Def.recipes.OrEmpty()) {
                    if (recipes.ContainsKey(recipe)) {
                        recipes[recipe].AddWorkbench(item);
                    }
                }

                var def = item.Def;
                if (!def.CostList.NullOrEmpty() || def.CostStuffCount > 0) {
                    var recipe = new BuildRecipe(def);
                    item.Recipes.Add(recipe);
                    if (def.CostStuffCount > 0) {
                        recipe.Material.Add(new Ingredient(def));
                    }
                    foreach (var material in def.CostList.OrEmpty()) {
                        recipe.Material.Add(new Ingredient(material, this[material.thingDef]));
                    }
                    foreach (var res in def.researchPrerequisites.OrEmpty()) {
                        recipe.Researches.Add(new Research(res));
                    }
                }
            }

            foreach (var recipe in recipes.Values) {
                recipe.PostInit();
            }
        }
    }

    public static class Extensions {
        public static IEnumerable<T> Empty<T>() { yield break; }

        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> items) => items ?? Empty<T>();

        public static IEnumerable<T> And<T>(this IEnumerable<T> items, T item) {
            if (items != null) {
                foreach (var it in items) yield return it;
            }
            if (item != null) yield return item;
        }
    }
}
