using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class Product : RowEntry {
        private readonly ThingDefCountClass thingCount;
        private readonly Item item;

        public Product(ThingDefCountClass thingCount, Item item) {
            this.thingCount = thingCount;
            this.item = item;
        }

        public override ThingDef IconThing => thingCount.thingDef;

        public override string Label => thingCount.Summary;

        public override Item ClickedItem => item;
    }
}
