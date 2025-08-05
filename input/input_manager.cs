using System;

using SDG.Unturned;

using interception.enums;
using interception.utils;

namespace interception.input {
    public delegate void on_key_down_global_callback(Player player, e_keycode keycode);
    public delegate void on_key_up_global_callback(Player player, e_keycode keycode);

    public static class input_manager {
        public static on_key_down_global_callback on_key_down_global;
        public static on_key_up_global_callback on_key_up_global;

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

        internal static void remove_component_from_online_players() {
            var len = Provider.clients.Count;
            for (int i = 0; i < len; i++) {
                if (Provider.clients[i] == null || Provider.clients[i].player == null || Provider.clients[i].player.gameObject == null) continue;
                var comp = Provider.clients[i].player.gameObject.GetComponent<player_input_component>();
                if (comp != null)
                    UnityEngine.Object.Destroy(comp);
            }
        }
    }
}
