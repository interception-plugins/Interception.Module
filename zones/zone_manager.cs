using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using UnityEngine;

/* 
    todo (zone_manager.cs):
        1. better pool implementation
*/

namespace interception.zones {
    public delegate void on_zone_enter_global_callback(Player player, zone_component zone);
    public delegate void on_zone_exit_global_callback(Player player, zone_component zone);

    public static class zone_manager {
        static Dictionary<string, GameObject> pool = new Dictionary<string, GameObject>();

        public static on_zone_enter_global_callback on_zone_enter_global = delegate (Player player, zone_component zone) { };
        public static on_zone_exit_global_callback on_zone_exit_global = delegate (Player player, zone_component zone) { };

        internal static bool debug_mode = false;

        public static sphere_zone_component create_sphere(string name, Vector3 pos, float radius) {
            if (pool.ContainsKey(name.ToLower()))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<sphere_zone_component>();
            comp.init(name.ToLower(), pos, radius);
            pool.Add(name.ToLower(), obj);
            return comp;
        }

        public static box_zone_component create_box(string name, Vector3 pos, Vector3 size) {
            if (pool.ContainsKey(name.ToLower()))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<box_zone_component>();
            comp.init(name.ToLower(), pos, size);
            pool.Add(name.ToLower(), obj);
            return comp;
        }

        public static distance_zone_component create_distance(string name, Vector3 pos, float radius) {
            if (pool.ContainsKey(name.ToLower()))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<distance_zone_component>();
            comp.init(name.ToLower(), pos, radius);
            pool.Add(name.ToLower(), obj);
            return comp;
        }

        public static mesh_zone_component create_mesh(string name, Vector3 pos, float height, int? mask) {
            if (pool.ContainsKey(name.ToLower()))
                throw new ArgumentException($"zone with name {name} already exist");
            GameObject obj = new GameObject();
            var comp = obj.AddComponent<mesh_zone_component>();
            try {
                comp.init(name.ToLower(), pos, height, mask);
            }
            catch {
                return null;
            }
            //comp.init(name.ToLower(), pos, height, mask);
            pool.Add(name.ToLower(), obj);
            return comp;
        }

        public static void remove_zone(string name) {
            if (!pool.ContainsKey(name))
                throw new ArgumentException($"zone with name {name} does not exist");
            pool[name].GetComponent<zone_component>().destroy();
            Console.WriteLine(pool.Count);
            pool.Remove(name);
        }

        public static void debug_all_zones() {
            var len = pool.Count;
            if (!debug_mode) {
                for (int i = 0; i < len; i++) {
                    var comp = pool[pool.ElementAt(i).Key].GetComponent<zone_component>();
                    comp.StartCoroutine(comp.debug_routine());
                }
                debug_mode = true;
            }
            else {
                for (int i = 0; i < len; i++) {
                    var comp = pool[pool.ElementAt(i).Key].GetComponent<zone_component>();
                    comp.StopAllCoroutines();
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

        // todo throw exception maybe?
        public static zone_component find_zone(string name) {
            if (!pool.ContainsKey(name)) return null;
            return pool[name].GetComponent<zone_component>();
        }
    }
}
