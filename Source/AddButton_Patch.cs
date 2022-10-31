using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TooManyModdedItems {
    [HarmonyPatch]
    public static class AddButton_Patch {
        private static readonly SoundDef mouseOver = SoundDefOf.Mouseover_ButtonToggle;

        [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
        [HarmonyPostfix]
        public static void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView) {
            if (row == null)
                return;

            bool show = MainDialog.Show;
            string label = show ? "Hide item catalog" : "Show item catalog";
            row.ToggleableIcon(ref show, Resources.Icon, label, mouseOver);
            MainDialog.Show = show;
        }
    }
}
