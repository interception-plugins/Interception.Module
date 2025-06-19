using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_button_collection_click_callback(int index);
    public delegate void on_button_collection_shown_callback(int index);
    public delegate void on_button_collection_hidden_callback(int index);

    public class button_collection : control {
        control _parent;
        public override control parent => _parent;
        short _key;
        public override short key => _key;
        ITransportConnection _tc;
        internal override ITransportConnection tc => _tc;
        string _name;
        public override string name => _name;
        string _path;
        public override string path => _path;
        public override bool is_visible => true;
        protected window root;
        public int max_buttons;
        public string suffix_format;

        public on_button_collection_click_callback on_click_any;
        public on_button_collection_shown_callback on_shown_any;
        public on_button_collection_hidden_callback on_hidden_any;

        public button_collection(control _parent, short _key, ITransportConnection _tc, string _name, int max_buttons, string suffix_format) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this.root = get_root_window();
            this.max_buttons = max_buttons;
            this.suffix_format = suffix_format;
            ui_manager.add_control(this, _name + string.Format(suffix_format, "{{INDEX}}"));
        }

        public override void show(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            for (int i = 0; i < max_buttons; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(i), true);
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            for (int i = 0; i < max_buttons; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(i), false);
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        public string get_path_of(int index) {
            return path + string.Format(suffix_format, index);
        }

        public void show(int index, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            if (index >= max_buttons)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_buttons}");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(index), true);
            if (on_shown_any != null)
                on_shown_any(index);
            ui_manager.trigger_on_button_collection_shown_global(index, this);
        }

        public void hide(int index, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            if (index >= max_buttons)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_buttons}");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(index), false);
            if (on_hidden_any != null)
                on_hidden_any(index);
            ui_manager.trigger_on_button_collection_shown_global(index, this);
        }

        internal void click_any(int index) {
            if (on_click_any != null)
                on_click_any(index);
            ui_manager.trigger_on_button_collection_click_global(index, this);
        }
    }
}
