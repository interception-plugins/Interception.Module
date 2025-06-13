using System;
using System.Collections.Generic;

using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_window_spawned_global_callback(window w);
    public delegate void on_window_despawned_global_callback(window w);

    public delegate void on_button_click_global_callback(button b);
    public delegate void on_textbox_text_changed_global_callback(string old_value, string new_value, textbox tb);
    public delegate void on_progressbar_progress_changed_global_callback(int old_value, int new_value, progressbar_text pb);

    public delegate void on_control_shown_global_callback(control c);
    public delegate void on_control_hidden_global_callback(control c);

    public static class ui_manager {
        public static on_window_spawned_global_callback on_window_spawned_global;
        public static on_window_despawned_global_callback on_window_despawned_global;
        
        public static on_button_click_global_callback on_button_click_global;
        public static on_textbox_text_changed_global_callback on_textbox_text_changed_global;
        public static on_progressbar_progress_changed_global_callback on_progressbar_progress_changed_global;

        public static on_control_shown_global_callback on_control_shown_global;
        public static on_control_hidden_global_callback on_control_hidden_global;

        internal static readonly Dictionary<ITransportConnection, Dictionary<string, control>> pool = new Dictionary<ITransportConnection, Dictionary<string, control>>();

        public static int get_pool_count() {
            return pool.Count;
        }

        internal static void trigger_on_window_spawned_global_global(window w) {
            if (on_window_spawned_global != null)
                on_window_spawned_global(w);
        }

        internal static void trigger_on_window_despawned_global_global(window w) {
            if (on_window_despawned_global != null)
                on_window_despawned_global(w);
        }

        internal static void trigger_on_button_click_global(button b) {
            if (on_button_click_global != null)
                on_button_click_global(b);
        }

        internal static void trigger_on_textbox_text_changed_global(string old_value, string new_value, textbox tb) {
            if (on_textbox_text_changed_global != null)
                on_textbox_text_changed_global(old_value, new_value, tb);
        }

        internal static void trigger_on_progressbar_progress_changed(int old_value, int new_value, progressbar_text pb) {
            if (on_progressbar_progress_changed_global != null)
                on_progressbar_progress_changed_global(old_value, new_value, pb);
        }

        internal static void trigger_on_control_shown_global(control c) {
            if (on_control_shown_global != null)
                on_control_shown_global(c);
        }

        internal static void trigger_on_control_hidden_global(control c) {
            if (on_control_hidden_global != null)
                on_control_hidden_global(c);
        }

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
