using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_collection_element_shown_callback(int index);
    public delegate void on_collection_element_hidden_callback(int index);

    public abstract class collection : control {
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
        protected window root;
        public string _suffix_format;
        public string suffix_format => _suffix_format;
        public int _max_elements;
        public int max_elements => _max_elements;

        public on_collection_element_shown_callback on_element_shown;
        public on_collection_element_shown_callback on_element_hidden;

        public collection(control _parent, short _key, ITransportConnection _tc, string _name, string _suffix_format, int _max_elements) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this.root = get_root_window();
            this._suffix_format = _suffix_format;
            this._max_elements = _max_elements;
            ui_manager.add_control(this, _name + string.Format(suffix_format, "{{INDEX}}"));
        }

        public virtual string get_path_of(int index) {
            return path + string.Format(suffix_format, index);
        }

        public override void show(bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            for (int i = 0; i < max_elements; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(i), true);
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            for (int i = 0; i < max_elements; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(i), false);
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        public virtual void show(int index, bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(index), true);
            if (on_element_shown != null)
                on_element_shown(index);
            ui_manager.trigger_on_collection_element_shown_global(index, this);
        }

        public virtual void hide(int index, bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, get_path_of(index), false);
            if (on_element_hidden != null)
                on_element_hidden(index);
            ui_manager.trigger_on_collection_element_hidden_global(index, this);
        }
    }
}
