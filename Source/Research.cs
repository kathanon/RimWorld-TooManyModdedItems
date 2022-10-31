using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    public class Research : RowEntry {
        public readonly ResearchProjectDef Def;

        public Research(ResearchProjectDef def) {
            Def = def;
        }

        public override ThingDef IconThing => null;

        public override string Label => Def.LabelCap;

        public override Item ClickedItem => null;
    }
}
