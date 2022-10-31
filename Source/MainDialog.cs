using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TooManyModdedItems {
    public class MainDialog : Window {
        public const float Width             = 250f;
        public const float Height            = 350f;
        public const float MarginSize        =  10f;
        public const float Gap               =   4f;
        public const float ScrollMargin      =  18f;
        public const float ItemHeight        =  26f;
        public const float IconSize          =  24f;
        public const float StepButtonsIndent =  48f;
        public const float IndexLabelSize    =  48f;

        public static readonly MainDialog Instance = new MainDialog();

        private readonly List<Item> items    = new List<Item>();
        private readonly List<Item> filtered = new List<Item>();
        private readonly QuickSearchWidget search = new QuickSearchWidget();
        private Vector2 scrollPos;

        internal void Setup() {
            items.Clear();
            items.AddRange(Main.DB.Items.Where(it => it.ListItem));
            items.SortBy(t => t.Def.label);
        }

        private bool visible;
        private Item item;
        private int recipeIndex;

        private Action<Rect> currentGUI;

        public MainDialog() {
            closeOnAccept = false;
            closeOnCancel = false;
            preventCameraMotion = false;
            draggable = true;
            resizeable = true;
            focusWhenOpened = true;
            shadowAlpha = .7f;
            soundAppear = null;
            soundClose = null;
            windowRect = new Rect(UI.screenWidth - Width, 0f, Width, Height);
            currentGUI = SelectGUI;
        }

        public static bool Show {
            get => Instance.visible;
            set {
                if (Instance.visible != value) {
                    if (value) {
                        Find.WindowStack.Add(Instance);
                    } else {
                        Instance.Close();
                    }
                }
                Instance.visible = value;
            }
        }

        protected override void SetInitialSizeAndPosition() { }

        protected override float Margin => MarginSize;

        private void UpdateFiltered() {
            filtered.Clear();
            filtered.AddRange(items.Where(Matches));
        }

        private bool Matches(Item it) => search.filter.Matches(it.Def);

        public override void DoWindowContents(Rect inRect) {
            Text.Font = GameFont.Small;
            currentGUI(inRect);
        }

        private void Select(Item next) {
            // TODO: record previous to allow "back"
            item = next;
        }

        private void SelectGUI(Rect rect) {
            Rect searchRect = rect.TopPartPixels(QuickSearchWidget.WidgetHeight);
            search.OnGUI(searchRect, UpdateFiltered);

            var list = search.filter.Active ? filtered : items;
            rect.yMin += QuickSearchWidget.WidgetHeight + Gap;
            Rect view = rect.AtZero();
            view.width -= ScrollMargin;
            view.height = (list.Count - 1) * ItemHeight + IconSize;
            int first = (int) (scrollPos.y / ItemHeight);
            int last = Math.Min(
                first + (int) (rect.height / ItemHeight) + 1, 
                list.Count - 1);

            Widgets.BeginScrollView(rect, ref scrollPos, view);
            Rect itemRow = view.TopPartPixels(IconSize);
            itemRow.y = first * ItemHeight;
            for (int i = first; i <= last; i++) {
                var cur = list[i];
                bool selected = item == cur;
                if (cur.DoEntry(itemRow, selected) != null) {
                    if (!selected) {
                        Select(cur);
                        recipeIndex = 0;
                    }
                    currentGUI = RecipieGUI;
                }
                itemRow.y += ItemHeight;
            }
            Widgets.EndScrollView();
        }

        private void RecipieGUI(Rect rect) {
            var row = rect.TopPartPixels(IconSize);
            if (item.DoEntry(row, true) != null) {
                currentGUI = SelectGUI;
            }
            row.y += IconSize + Gap;

            DoRecipeStepButtons(row);
            row.y += IconSize + Gap;

            rect.yMin = row.y;
            var next = item.Recipes[recipeIndex].DoRecipeDetails(rect);
            if (next != null) Select(next);
        }

        private void DoRecipeStepButtons(Rect r) {
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleCenter;

            r.x += StepButtonsIndent;

            bool prevActive = recipeIndex > 0;
            r.width = IconSize;
            if (StepButton(r, "<", prevActive)) {
                recipeIndex--;
            }
            r.x += r.width + Gap;

            r.width = IndexLabelSize;
            string indexStr = $"{recipeIndex + 1} / {item.Recipes.Count}";
            Widgets.Label(r, indexStr);
            r.x += r.width + Gap;

            r.width = IconSize;
            bool nextActive = recipeIndex < item.Recipes.Count - 1;
            if (StepButton(r, ">", nextActive)) {
                recipeIndex++;
            }
            r.x += r.width + Gap;

            if (StepButton(r, "^", true)) {
                currentGUI = SelectGUI;
            }

            Text.Anchor = anchor;
        }

        private bool StepButton(Rect rect, string label, bool active) {
            var font = Text.Font;
            var color = GUI.color;
            Text.Font = GameFont.Medium;
            if (!active) {
                var dimmed = color * 0.4f;
                dimmed.a = color.a;
                GUI.color = dimmed;
            }
            bool res = Widgets.ButtonText(rect, label, doMouseoverSound: active, active: active);
            Text.Font = font;
            GUI.color = color;
            return res;
        }
    }
}
