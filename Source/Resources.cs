using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TooManyModdedItems {
    [StaticConstructorOnStartup]
    public static class Resources {
        public static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("Icon");
    }
}
