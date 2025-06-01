using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SDG.Unturned;

using interception.utils;

namespace interception.input {
    public sealed class player_input_component : MonoBehaviour {
        Player player;
        bool[] last_key_states;

        public void init(Player player) {
            this.player = player;
            last_key_states = new bool[15];
        }

        void OnDestroy() {
            Console.WriteLine("destroy");
        }
        // im 2 drunk lmao 0_o
        void FixedUpdate() {
            for (int i = 0; i < input_util.DEFAULT_KEYS + ControlsSettings.NUM_PLUGIN_KEYS; i++) {
                if (last_key_states[i] != true != player.input.keys[i]) {
                    if (!last_key_states[i]) {
                        // todo on_key_down
                        last_key_states[i] = false;
                    }
                    else {
                        // todo on_key_up
                        last_key_states[i] = true;
                    }
                }
            }
        }
    }
}
