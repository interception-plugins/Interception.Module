using System;

using UnityEngine;
using SDG.Unturned;

using interception.utils;
using interception.enums;

namespace interception.input {
    public delegate void on_key_down_callback(e_keycode keycode);
    public delegate void on_key_up_callback(e_keycode keycode);

    public sealed class player_input_component : MonoBehaviour {
        Player player;
        bool[] last_key_states;

        public on_key_down_callback on_key_down;
        public on_key_up_callback on_key_up;

        public void init(Player player) {
            this.player = player;
            last_key_states = new bool[input_util.DEFAULT_KEYS + ControlsSettings.NUM_PLUGIN_KEYS];
            on_key_down = delegate (e_keycode _) { };
            on_key_up = delegate (e_keycode _) { };
        }

        void OnDestroy() {
            on_key_down = null;
            on_key_up = null;
        }
        
        void FixedUpdate() {
            var len = last_key_states.Length;
            for (int i = 0; i < len; i++) {
                if (last_key_states[i] != player.input.keys[i]) {
                    var key = input_util.map_key(i);
                    if (!last_key_states[i]) {
                        last_key_states[i] = true;
                        if (on_key_down != null)
                            on_key_down(key);
                        input_manager.trigger_on_key_down_global(player, key);
                    }
                    else {
                        last_key_states[i] = false;
                        if (on_key_up != null)
                            on_key_up(key);
                        input_manager.trigger_on_key_up_global(player, key);
                    }
                }
            }
        }
    }
}
