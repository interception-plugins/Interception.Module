using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace interception.cron {
    public delegate void on_cron_event_executed_global_callback(cron_event _event);

    public static class cron_manager {
        static readonly Dictionary<string, cron_event> pool = new Dictionary<string, cron_event>();
        static List<cron_event> events => pool.Values.ToList();

        public static on_cron_event_executed_global_callback on_cron_event_executed_global = delegate(cron_event _event) { };

        internal static void trigger_on_cron_event_executed_global(cron_event _event) {
            if (on_cron_event_executed_global != null)
                on_cron_event_executed_global(_event);
        }

        internal static void clear_pool() {
            pool.Clear();
        }

        internal static void tick() {
            var len = events.Count;
            for (int i = 0; i < len; i++) {
                if (DateTime.UtcNow.ToUniversalTime().ToString("HH:mm:ss") == events[i].execution_time.ToUniversalTime().ToString("HH:mm:ss")) { // todo better comparing
                    events[i].execute();
                }
            }
        }

        public static void register_event(cron_event _event) {
            if (pool.ContainsKey(_event.name))
                throw new ArgumentException($"cron event with name {_event.name} already exist");
            pool.Add(_event.name, _event);
        }

        public static void unregister_event(string name) {
            if (!pool.ContainsKey(name))
                throw new ArgumentException($"cron event with name {name} does not exist");
            pool.Remove(name);
        }

        public static bool is_event_exist(string name) {
            return pool.ContainsKey(name);
        }

        public static cron_event find_event(string name) {
            if (!pool.ContainsKey(name)) return null;
            return pool[name];
        }
    }
}
