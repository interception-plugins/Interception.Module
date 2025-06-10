using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;

using interception.enums;
using interception.utils;

namespace interception.input {
    public delegate void on_key_down_global_callback(Player player, e_keycode keycode);
    public delegate void on_key_up_global_callback(Player player, e_keycode keycode);

    public static class input_manager {
        public static on_key_down_global_callback on_key_down_global = delegate (Player player, e_keycode keycode) { };
        public static on_key_up_global_callback on_key_up_global = delegate (Player player, e_keycode keycode) { };

        public static bool is_key_pressed(Player player, e_keycode keycode) {
            return player.input.keys[input_util.map_key(keycode)];
        }

        internal static void trigger_on_key_down_global(Player player, e_keycode keycode) {
            if (on_key_down_global != null)
                on_key_down_global(player, keycode);
        }

        internal static void trigger_on_key_up_global(Player player, e_keycode keycode) {
            if (on_key_up_global != null)
                on_key_up_global(player, keycode);
        }
    }
}
