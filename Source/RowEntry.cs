using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TooManyModdedItems {
    public abstract class RowEntry {
        public const float Gap = 4f;

        public abstract ThingDef IconThing { get; }

        public abstract string Label { get; }

        public abstract Item ClickedItem { get; }

        public Item DoEntry(Rect r, bool selected) {
            bool wrap = Text.WordWrap;
            Text.WordWrap = false;
            if (selected) {
                Widgets.DrawBox(r);
            }

            var icon = IconThing;
            if (icon != null) {
                Widgets.ThingIcon(r.LeftPartPixels(r.height), icon);
            }

            Rect labelRect = r.RightPartPixels(r.width - r.height - Gap);
            Widgets.Label(labelRect, Label);
            Text.WordWrap = wrap;

            var clicked = ClickedItem;
            return (clicked != null && 
                    clicked.ListItem && 
                    Widgets.ButtonInvisible(r)) ? clicked : null;
        }
    }
}
