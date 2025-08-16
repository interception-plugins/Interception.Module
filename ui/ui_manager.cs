using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_window_spawned_global_callback(window w);
    public delegate void on_window_despawned_global_callback(window w);

    public delegate void on_button_click_global_callback(button b);
    public delegate void on_textbox_text_changed_global_callback(string old_value, string new_value, textbox tb);
    public delegate void on_progressbar_progress_changed_global_callback(int old_value, int new_value, progressbar pb);
    
    public delegate void on_button_collection_click_global_callback(int index, button_collection bc);
    public delegate void on_collection_element_shown_global_callback(int index, collection bc);
    public delegate void on_collection_element_hidden_global_callback(int index, collection bc);

    public delegate void on_control_shown_global_callback(control c);
    public delegate void on_control_hidden_global_callback(control c);

    public static class ui_manager {
        public static on_window_spawned_global_callback on_window_spawned_global;
        public static on_window_despawned_global_callback on_window_despawned_global;
        
        public static on_button_click_global_callback on_button_click_global;
        public static on_textbox_text_changed_global_callback on_textbox_text_changed_global;
        public static on_progressbar_progress_changed_global_callback on_progressbar_progress_changed_global;

        public static on_button_collection_click_global_callback on_button_collection_click_global;
        public static on_collection_element_shown_global_callback on_collection_element_shown_global;
        public static on_collection_element_hidden_global_callback on_collection_element_hidden_global;

        public static on_control_shown_global_callback on_control_shown_global;
        public static on_control_hidden_global_callback on_control_hidden_global;

        internal static readonly Dictionary<ITransportConnection, Dictionary<string, control>> pool = new Dictionary<ITransportConnection, Dictionary<string, control>>();

        public static int get_pool_connections_count() {
            return pool.Count;
        }

        public static int get_pool_controls_count(ITransportConnection tc) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            return pool[tc].Count;
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

        internal static void trigger_on_progressbar_progress_changed_global(int old_value, int new_value, progressbar pb) {
            if (on_progressbar_progress_changed_global != null)
                on_progressbar_progress_changed_global(old_value, new_value, pb);
        }

        internal static void trigger_on_button_collection_click_global(int index, button_collection bc) {
            if (on_button_collection_click_global != null)
                on_button_collection_click_global(index, bc);
        }

        internal static void trigger_on_collection_element_shown_global(int index, collection bc) {
            if (on_collection_element_shown_global != null)
                on_collection_element_shown_global(index, bc);
        }

        internal static void trigger_on_collection_element_hidden_global(int index, collection bc) {
            if (on_collection_element_hidden_global != null)
                on_collection_element_hidden_global(index, bc);
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


        internal static void add_control(control ctrl, string name) {
            if (ctrl == null)
                throw new NullReferenceException("parameter is null");
            if (!pool.ContainsKey(ctrl.tc))
                throw new KeyNotFoundException($"{ctrl.path}'s transport connection is not present in the pool");
            if (pool[ctrl.tc].ContainsKey(name))
                throw new ArgumentException($"{name}'s already present in the pool");
            pool[ctrl.tc].Add(name, ctrl);
        }

        internal static void on_effect_button_clicked(ITransportConnection tc, string b) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            if (pool[tc].ContainsKey(b) && (pool[tc][b] is button)) {
                ((button)pool[tc][b]).click();
                return;
            }
            string bb = Regex.Replace(b, @"[0-9]${1,}", "{{INDEX}}");
            if (pool[tc].ContainsKey(bb) && (pool[tc][bb] is button_collection)) {
                int index;
                if (!int.TryParse(Regex.Replace(b, @"[^0-9]", string.Empty), out index)) return;
                ((button_collection)pool[tc][bb]).element_click(index);
                return;
            }
        }

        internal static void on_effect_text_commited(ITransportConnection tc, string b, string t) {
            if (!pool.ContainsKey(tc))
                throw new ArgumentException($"transport connection is not present in the pool");
            if (pool[tc].ContainsKey(b) && pool[tc][b] is textbox)
                ((textbox)pool[tc][b]).commit(t);
        }
    }
}
