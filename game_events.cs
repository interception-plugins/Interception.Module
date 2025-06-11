using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using Steamworks;
using UnityEngine;

using interception.input;
using interception.zones;
using interception.time;

namespace interception {
    internal static class game_events {
        static void on_effect_button_clicked() {

        }

        static void on_effect_input_commited() {

        }

        static void on_region_updated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte index, ref bool canIncrementIndex) {
            var old_coords = new RegionCoordinate(old_x, old_y);
            var new_coords = new RegionCoordinate(new_x, new_y);
            if (zone_manager.regions_to_check.ContainsKey(old_coords))
                if (zone_manager.regions_to_check[old_coords].ContainsKey(player.channel.owner.playerID.steamID.m_SteamID))
                    zone_manager.regions_to_check[old_coords].Remove(player.channel.owner.playerID.steamID.m_SteamID);
            if (zone_manager.regions_to_check.ContainsKey(new_coords))
                if (!zone_manager.regions_to_check[new_coords].ContainsKey(player.channel.owner.playerID.steamID.m_SteamID))
                    zone_manager.regions_to_check[new_coords].Add(player.channel.owner.playerID.steamID.m_SteamID, player);

        }

        static void on_server_connected(CSteamID csid) {
            Player p = PlayerTool.getPlayer(csid);
            if (p == null) return;
            p.gameObject.AddComponent<player_input_component>().init(p);
            p.movement.onRegionUpdated += on_region_updated;
        }

        //static void on_player_created(Player p) {
        //    p.gameObject.AddComponent<player_input_component>().init(p);
        //    p.movement.onRegionUpdated += on_region_updated;
        //}

        static void on_server_disconnected(CSteamID csid) {
            Player p = PlayerTool.getPlayer(csid);
            if (p == null) return;
            var coords = new RegionCoordinate(p.movement.region_x, p.movement.region_y);
            if (zone_manager.regions_to_check.ContainsKey(coords))
                if (zone_manager.regions_to_check[coords].ContainsKey(p.channel.owner.playerID.steamID.m_SteamID))
                    zone_manager.regions_to_check[coords].Remove(p.channel.owner.playerID.steamID.m_SteamID);
            p.movement.onRegionUpdated -= on_region_updated;
        }
        
        static void on_post_level_loaded(int level) {
            main.instance.module_game_object.AddComponent<time_component>();
        }

        public static void init() {
            Provider.onServerConnected += on_server_connected;
            //Player.onPlayerCreated += on_player_created;
            Provider.onServerDisconnected += on_server_disconnected;
            Level.onPostLevelLoaded += on_post_level_loaded;
        }

        public static void uninit() {
            Level.onPostLevelLoaded -= on_post_level_loaded;
            Provider.onServerDisconnected -= on_server_disconnected;
            //Player.onPlayerCreated -= on_player_created;
            Provider.onServerConnected -= on_server_connected;
        }
    }
}
