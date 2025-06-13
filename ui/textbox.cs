using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_textbox_text_changed_callback(string old_value, string new_value);

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

        public on_textbox_text_changed_callback on_text_changed;

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
                root.internal_on_spawned += on_spawn;
                root.internal_on_despawned += on_despawn;
            }
            ui_manager.add_control(this);
        }

        public override void show(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, true);
            _is_visible = true;
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, false);
            _is_visible = false;
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        public void set_text(string text, bool notify = false, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectText(key, tc, reliable, path, text);
            if (notify) {
                string old = this.text;
                this.text = text;
                if (on_text_changed != null)
                    on_text_changed(old, this.text);
                ui_manager.trigger_on_textbox_text_changed_global(old, this.text, this);
            }
            else {
                this.text = text;
            }
        }

        public string get_text() {
            return text;
        }

        public void clear(bool notify = false, bool reliable = true) {
            //EffectManager.sendUIEffectText(key, tc, reliable, path, string.Empty);
            set_text(string.Empty, notify, reliable);
        }

        internal void commit(string text) {
            string old = this.text;
            this.text = text;
            if (on_text_changed != null)
                on_text_changed(old, this.text);
            ui_manager.trigger_on_textbox_text_changed_global(old, this.text, this);

        }

        protected override void on_spawn() {
            text = string.Empty;
        }

        protected override void on_despawn() {
            text = string.Empty;
        }
    }
}
