using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interception.enums {
    public enum e_keycode {
        space = 0,
        unknown0 = 1, // not used at all i guess
        unknown1 = 2, // not used at all i guess
        x = 3, // depends on settings
        z = 4, // depends on settings
        shift1 = 5, // depends on settings
        q = 6, // depends on settings
        e = 7, // depends on settings
        b = 8, // works only if player has a gun with equipped tactical, also on_key_up getting called instantly
        shift0 = 9, 
        comma = 10,
        period = 11,
        slash = 12,
        semicolon = 13,
        quote = 14,
        unknown = 15 // not used
    }
}
