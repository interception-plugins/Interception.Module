using System;
using System.Collections.Generic;

using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_window_spawned_global_callback(window w);
    public delegate void on_window_despawned_global_callback(window w);

    //public delegate void on_click_global_callback(short key, string name, ITransportConnection tc);

    public static class ui_manager {
        //public static on_window_created_global_callback on_window_created_global;
        //public static on_window_shown_global_callback on_window_shown_global;
        //public static on_window_hidden_global_callback on_window_hidden_global;
        //public static on_window_destroyed_global_callback on_window_destroyed_global;

        internal static readonly Dictionary<ITransportConnection, Dictionary<string, control>> pool = new Dictionary<ITransportConnection, Dictionary<string, control>>();

        internal static void add_player(ITransportConnection tc) {
            if (pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection already present in the pool");
            pool.Add(tc, new Dictionary<string, control>());
        }

        internal static void remove_player(ITransportConnection tc) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            pool.Remove(tc);
        }

        internal static void add_control(control ctrl) {
            if (ctrl == null)
                throw new NullReferenceException("parameter is null");
            if (!pool.ContainsKey(ctrl.tc))
                throw new KeyNotFoundException($"{ctrl.path}'s transport connection is not present in the pool");
            if (pool[ctrl.tc].ContainsKey(ctrl.name))
                throw new ArgumentException($"{ctrl.name}'s already present in the pool");
            pool[ctrl.tc].Add(ctrl.name, ctrl);
        }

        internal static void on_effect_button_clicked(ITransportConnection tc, string b) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            if (pool[tc].ContainsKey(b) && pool[tc][b] is button)
                ((button)pool[tc][b]).click();
        }

        internal static void on_effect_text_commited(ITransportConnection tc, string b, string t) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            if (pool[tc].ContainsKey(b) && pool[tc][b] is textbox)
                ((textbox)pool[tc][b]).commit(t);
        }
    }
}
