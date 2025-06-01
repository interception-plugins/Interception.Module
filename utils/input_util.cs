using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;

using interception.enums;

namespace interception.utils {
    public static class input_util {
        public const int DEFAULT_KEYS = 10;
        
        public static int map_key(e_keycode keycode) {
            if (keycode < 0 || keycode >= e_keycode.unknown)
                throw new Exception("unknown key (did game just updated?)");
            return (int)keycode;
        }

        public static e_keycode map_key(int index) {
            if (!Enum.IsDefined(typeof(e_keycode), index))
                throw new Exception("unknown key (did game just updated?)");
            return (e_keycode)index;
        }

        public static int map_plugin_key(e_keycode keycode) {
            if (keycode < e_keycode.comma || keycode >= e_keycode.unknown)
                throw new Exception("unknown key (did game just updated?)");
            return (int)(keycode - DEFAULT_KEYS);
        }

        public static e_keycode map_plugin_key(int index) {
            if (index < 0 || index >= ControlsSettings.NUM_PLUGIN_KEYS || !Enum.IsDefined(typeof(e_keycode), index + DEFAULT_KEYS))
                throw new Exception("unknown key (did game just updated?)");
            return (e_keycode)(index + DEFAULT_KEYS);
        }
    }
}
