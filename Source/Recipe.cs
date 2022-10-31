using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TooManyModdedItems {
    public abstract class Recipe {
        public const float CategoryLabelSize =  22f;
        public const float CategoryGap       =  8f;
        public const float CategorySize      =  CategoryGap + CategoryLabelSize;
        public const float Gap               =  RowEntry.Gap;
        public const float IconSize          =  MainDialog.IconSize;
        public const float ItemHeight        =  MainDialog.ItemHeight;
        public const float ScrollMargin      =  MainDialog.ScrollMargin;

        private Vector2 scrollPos;

        public Item DoRecipeDetails(Rect rect) {
            Item next = null;
            float height = Categories.Count * CategorySize + (TotalEntries - 1) * ItemHeight + IconSize;
            bool scroll = height > rect.height;
            if (scroll) {
                Rect viewRect = new Rect(0f, 0f, rect.width - ScrollMargin, height);
                Widgets.BeginScrollView(rect, ref scrollPos, viewRect);
                rect = viewRect;
            }
            foreach (var category in Categories) {
                next = category.DoGUI(ref rect) ?? next;
            }
            if (scroll) {
                Widgets.EndScrollView();
            }
            return next;
        }

        public virtual void AddWorkbench(Item bench) => throw new NotImplementedException();

        public virtual void PostInit() { }

        private List<Category> categories;

        protected List<Category> Categories {
            get {
                if (categories == null) {
                    categories = GetCategories();
                }
                return categories;
            }
        }

        protected abstract List<Category> GetCategories();

        private int TotalEntries => Categories.Sum(c => c.Count);

        public abstract class Category {
            public readonly string Title;

            public Category(string title) {
                Title = title;
            }

            public abstract int Count { get; }

            public bool Any => Count > 0;

            public abstract Item DoGUI(ref Rect rect);
        }

        public class Category<T> : Category where T : RowEntry {
            public readonly List<T> Entries;

            public Category(string title) : base(title) {
                Entries = new List<T>();
            }

            public override int Count => Entries.Count;

            public void Add(T item) {
                Entries.Add(item);
            }

            public override Item DoGUI(ref Rect rect) {
                Item next = null;
                Rect row = rect.TopPartPixels(IconSize);
                row.y += CategoryGap;
                Widgets.Label(row, Title);
                row.y += CategoryLabelSize;
                foreach (var entry in Entries) {
                    next = entry.DoEntry(row, false) ?? next;
                    row.y += ItemHeight;
                }
                rect.yMin = row.y;
                return next;
            }
        }
    }
}
