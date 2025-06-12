using System;

using UnityEngine;
using SDG.Unturned;

namespace interception.extensions {
    public static class Color32_ex {
        public static string to_hex(this Color32 c) {
            return Palette.hex(c);
        }
    }
}
