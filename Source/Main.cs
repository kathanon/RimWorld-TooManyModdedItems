using HarmonyLib;
using RimWorld;
using RimWorld.SketchGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TooManyModdedItems
{
    public class Main : HugsLib.ModBase {
        public override string ModIdentifier => Strings.ID;

        public static Database DB;

        public override void DefsLoaded() {
            //Options.Setup(Settings);
            DB = new Database();
            MainDialog.Instance.Setup();
        }

        //public override void MapLoaded(Map map) => Ticker.VisitMap(map);

        //public override void MapDiscarded(Map map) => Ticker.Rebuild();

        //public override void Tick(int currentTick) => Ticker.Tick(currentTick);
    }
}
