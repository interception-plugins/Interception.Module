using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public sealed class text : control {
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
        string _text;
        //Color? color;
        window root;

        public text(control _parent, short _key, ITransportConnection _tc, string _name) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this._text = string.Empty;
            //this.color = null;
            this.root = get_root_window();
            if (root != null) {
                root.internal_on_spawned += on_spawn;
                root.internal_on_despawned += on_despawn;
            }
        }

        public override void show(bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, true);
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, false);
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        public void set_text(string _text, bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectText(key, tc, reliable, path, _text);
            //EffectManager.sendUIEffectText(key, tc, true, path, color != null ? $"<color={Palette.hex((Color32)_color)}>{_text}</color>" : _text);
            this._text = _text;
        }

        public string get_text() {
            return _text;
        }

        protected override void on_spawn() {
            _text = string.Empty;
        }

        protected override void on_despawn() {
            _text = string.Empty;
        }

        /*
        public void set_text_color(Color? color) {
            this.color = color;
            set_text(_text);
        }

        public void set_text_color(Color32? color) {
            this.color = (Color)color;
            set_text(_text);
        }

        public void set_text_color(string color) {
            if (color == null) {
                this._color = null;
                set_text(_text);
                return;
            }
            this._color = (Color)Palette.hex(color);
            set_text(_text);
        }

        public void add_rich_text_param(string key, string value) {
            
        }
        */
    }
}
