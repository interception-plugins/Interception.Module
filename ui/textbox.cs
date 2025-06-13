using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_text_changed_callback(string old_value, string new_value);

    public sealed class textbox : control {
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
        bool _is_visible;
        public override bool is_visible => _is_visible;
        string text;
        window root;

        public on_text_changed_callback on_text_changed;

        public textbox(control _parent, short _key, ITransportConnection _tc, string _name, bool _visible_by_default = true) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this._is_visible = _visible_by_default;
            this.text = string.Empty;
            this.root = get_root_window();
            if (root != null) {
                root.internal_on_window_spawned += on_spawn;
                root.internal_on_window_despawned += on_despawn;
            }
            ui_manager.add_control(this);
        }

        public override void show(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, true);
            _is_visible = true;
        }

        public override void hide(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, false);
            _is_visible = false;
        }

        public void set_text(string text, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectText(key, tc, reliable, path, text);
            this.text = text;
        }

        public string get_text() {
            return text;
        }

        public void clear(bool reliable = true) {
            EffectManager.sendUIEffectText(key, tc, reliable, path, string.Empty);
            this.text = string.Empty;
        }

        internal void commit(string text) {
            string old = this.text;
            this.text = text;
            if (on_text_changed != null)
                on_text_changed(old, this.text);
        }

        protected override void on_spawn() {
            text = string.Empty;
        }

        protected override void on_despawn() {
            text = string.Empty;
        }
    }
}
