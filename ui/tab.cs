using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public sealed class tab : control {
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
        window root;

        public tab(control _parent, short _key, ITransportConnection _tc, string _name, bool _visible_by_default = true) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            this._is_visible = _visible_by_default;
            this.root = get_root_window();
        }

        public override void show(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, true);
            _is_visible = true;
        }

        public override void hide(bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, false);
            _is_visible = false;
        }

        public tab add_tab(string name) {
            return new tab(this, key, tc, name);
        }

        public text add_text(string name) {
            return new text(this, key, tc, name);
        }

        public image add_image(string name) {
            return new image(this, key, tc, name);
        }
    }
}
