using System;

using UnityEngine;
using SDG.Unturned;

namespace interception.extensions {
    public static class Color_ex {
        public static string to_hex(this Color c) {
            return Palette.hex((Color32)c);
        }
    }
}
