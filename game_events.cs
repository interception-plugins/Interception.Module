using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using Steamworks;
using UnityEngine;

using interception.input;

namespace interception {
    internal static class game_events {
        static void on_effect_button_clicked() {

        }

        static void on_effect_input_commited() {

        }

        static void on_server_connected(CSteamID csid) {
            Player p = PlayerTool.getPlayer(csid);
            if (p == null) return;
            p.transform.gameObject.AddComponent<player_input_component>().init(p);
        }
        
        public static void init() {
            Provider.onServerConnected += on_server_connected;
        }

        public static void uninit() {
            Provider.onServerConnected -= on_server_connected;
        }
    }
}
