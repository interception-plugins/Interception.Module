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
using interception.ui;

namespace interception {
    public delegate void on_player_teleported_global_callback(Player player, Vector3 point);
    public delegate void on_player_region_updated_global_callback(Player player, RegionCoordinate old_xy, RegionCoordinate new_xy, byte index, ref bool canIncrementIndex);

    public static class game_events {
        public static on_player_teleported_global_callback on_player_teleported_global;
        public static on_player_region_updated_global_callback on_player_region_updated_global;

        static void on_effect_button_clicked(Player p, string b) {
            ui_manager.on_effect_button_clicked(p.channel.owner.transportConnection, b);
        }

        static void on_effect_input_commited(Player p, string b, string t) {
            ui_manager.on_effect_text_commited(p.channel.owner.transportConnection, b, t);
        }

        // this triggers 6 times in a row thx nelson
        static void on_region_updated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte index, ref bool canIncrementIndex) {
            var old_coords = new RegionCoordinate(old_x, old_y);
            var new_coords = new RegionCoordinate(new_x, new_y);
            if (zone_manager.regions_to_check.ContainsKey(old_coords))
                if (zone_manager.regions_to_check[old_coords].ContainsKey(player.channel.owner.playerID.steamID.m_SteamID))
                    zone_manager.regions_to_check[old_coords].Remove(player.channel.owner.playerID.steamID.m_SteamID);
            if (zone_manager.regions_to_check.ContainsKey(new_coords))
                if (!zone_manager.regions_to_check[new_coords].ContainsKey(player.channel.owner.playerID.steamID.m_SteamID))
                    zone_manager.regions_to_check[new_coords].Add(player.channel.owner.playerID.steamID.m_SteamID, player);
            if (on_player_region_updated_global != null)
                on_player_region_updated_global(player, old_coords, new_coords, index, ref canIncrementIndex);
        }

        static void on_player_teleported(Player player, Vector3 point) {
            if (on_player_teleported_global != null)
                on_player_teleported_global(player, point);
        }

        static void on_server_connected(CSteamID csid) {
            Player p = PlayerTool.getPlayer(csid);
            if (p == null) return;
            p.gameObject.AddComponent<player_input_component>().init(p);
            p.movement.onRegionUpdated += on_region_updated;
            p.onPlayerTeleported += on_player_teleported;
            ui_manager.add_player(p.channel.owner.transportConnection);
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
            p.onPlayerTeleported -= on_player_teleported;
            p.movement.onRegionUpdated -= on_region_updated;
            ui_manager.remove_player(p.channel.owner.transportConnection);
        }
        
        static void on_post_level_loaded(int level) {
            main.instance.module_game_object.AddComponent<time_component>();
        }

        internal static void init() {
            Provider.onServerConnected += on_server_connected;
            //Player.onPlayerCreated += on_player_created;
            Provider.onServerDisconnected += on_server_disconnected;
            Level.onPostLevelLoaded += on_post_level_loaded;
            EffectManager.onEffectButtonClicked += on_effect_button_clicked;
            EffectManager.onEffectTextCommitted += on_effect_input_commited;
        }

        internal static void uninit() {
            EffectManager.onEffectTextCommitted -= on_effect_input_commited;
            EffectManager.onEffectButtonClicked -= on_effect_button_clicked;
            Level.onPostLevelLoaded -= on_post_level_loaded;
            Provider.onServerDisconnected -= on_server_disconnected;
            //Player.onPlayerCreated -= on_player_created;
            Provider.onServerConnected -= on_server_connected;
        }
    }
}
