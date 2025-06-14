using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public class image : control {
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
        string _url;
        window root;

        public image(control _parent, short _key, ITransportConnection _tc, string _name, bool _visible_by_default = true) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this._is_visible = _visible_by_default;
            this._url = string.Empty;
            this.root = get_root_window();
            if (root != null) {
                root.internal_on_spawned += on_spawn;
                root.internal_on_despawned += on_despawn;
            }
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

        //public void set_image(string _url, bool reliable = true) {
        //    EffectManager.sendUIEffectImageURL(key, tc, reliable, path, _url);
        //}
        
        public void set_image(string _url, bool reliable = true, bool should_cache = true, bool force_refresh = false) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectImageURL(key, tc, reliable, path, _url, should_cache, force_refresh);
            this._url = _url;
        }

        public string get_image_url() {
            return _url;
        }

        protected override void on_spawn() {
            _url = string.Empty;
        }

        protected override void on_despawn() {
            _url = string.Empty;
        }
    }
}
