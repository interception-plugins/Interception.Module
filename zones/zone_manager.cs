using System;
using System.Collections.Generic;
using System.Linq;

using SDG.Unturned;
using UnityEngine;

namespace interception.zones {
    public delegate void on_zone_enter_global_callback(Player player, zone_component zone);
    public delegate void on_zone_exit_global_callback(Player player, zone_component zone);

    public static class zone_manager {
        static readonly Dictionary<string, GameObject> pool = new Dictionary<string, GameObject>();

        public static on_zone_enter_global_callback on_zone_enter_global;
        public static on_zone_exit_global_callback on_zone_exit_global;

        internal static bool debug_mode = false;

        internal static readonly Dictionary<RegionCoordinate, Dictionary<ulong, Player>> regions_to_check = new Dictionary<RegionCoordinate, Dictionary<ulong, Player>>();

        public static sphere_zone_component create_sphere_zone(string name, Vector3 pos, float radius) {
            if (pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<sphere_zone_component>();
            try {
                comp.init(name, pos, radius);
                pool.Add(name, obj);
            }
            catch {
                UnityEngine.Object.Destroy(obj);
                return null;
            }
            return comp;
        }

        public static box_zone_component create_box_zone(string name, Vector3 pos, Vector3 forward, Vector3 size) {
            if (pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<box_zone_component>();
            try {
                comp.init(name, pos, forward, size);
                pool.Add(name, obj);
            }
            catch {
                UnityEngine.Object.Destroy(obj);
                return null;
            }
            return comp;
        }

        public static distance_slow_zone_component create_distance_slow_zone(string name, Vector3 pos, float radius) {
            if (pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<distance_slow_zone_component>();
            try {
                comp.init(name, pos, radius);
                pool.Add(name, obj);
            }
            catch {
                UnityEngine.Object.Destroy(obj);
                return null;
            }
            return comp;
        }

        public static distance_fast_zone_component create_distance_fast_zone(string name, Vector3 pos, float radius) {
            if (pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<distance_fast_zone_component>();
            try {
                comp.init(name, pos, radius);
                pool.Add(name, obj);
            }
            catch {
                UnityEngine.Object.Destroy(obj);
                return null;
            }
            return comp;
        }

        public static mesh_zone_component create_mesh_zone(string name, Vector3 pos, float height, int? mask) {
            if (pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<mesh_zone_component>();
            try {
                comp.init(name, pos, height, mask, false);
                pool.Add(name, obj);
            }
            catch {
                UnityEngine.Object.Destroy(obj);
                return null;
            }
            return comp;
        }

        public static void remove_zone(string name) {
            if (!pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} does not exist");
            UnityEngine.Object.Destroy(pool[name]);
            pool.Remove(name);
        }

        public static void debug_all_zones() {
            var len = pool.Count;
            if (!debug_mode) {
                for (int i = 0; i < len; i++) {
                    var comp = pool[pool.ElementAt(i).Key].GetComponent<zone_component>();
                    comp.enable_debug();
                }
                debug_mode = true;
            }
            else {
                for (int i = 0; i < len; i++) {
                    var comp = pool[pool.ElementAt(i).Key].GetComponent<zone_component>();
                    comp.disable_debug();
                }
                debug_mode = false;
            }
        }

        internal static void trigger_on_zone_enter_global(Player player, zone_component zone) {
            if (on_zone_enter_global != null)
                on_zone_enter_global(player, zone);
        }

        internal static void trigger_on_zone_exit_global(Player player, zone_component zone) {
            if (on_zone_exit_global != null)
                on_zone_exit_global(player, zone);
        }

        internal static void clear_zones() {
            var len = pool.Count;
            for (int i = 0; i < len; i++)
                UnityEngine.Object.Destroy(pool.ElementAt(i).Value);
            pool.Clear();
        }

        public static bool is_player_in_zone(string name, Player player) {
            if (!pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} does not exist");
            return pool[name].GetComponent<zone_component>().get_players()
                .FindIndex(x => x.channel.owner.playerID.steamID.m_SteamID == player.channel.owner.playerID.steamID.m_SteamID) != -1;
        }

        public static List<Player> get_players_in_zone(string name) {
            if (!pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} does not exist");
            return pool[name].GetComponent<zone_component>().get_players();
        }

        public static bool is_zone_exist(string name) {
            return pool.ContainsKey(name);
        }

        public static zone_component find_zone(string name) {
            if (!pool.ContainsKey(name)) return null;
            return pool[name].GetComponent<zone_component>();
        }

        public static int get_zones_count() {
            return pool.Count;
        }
    }
}
