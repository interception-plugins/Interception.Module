using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_window_spawned_callback();
    public delegate void on_window_despawned_callback();

    public sealed class window : control {
        public override control parent => null;
        short _key;
        public override short key => _key;
        ITransportConnection _tc;
        internal override ITransportConnection tc => _tc;
        public override string name => "Canvas";
        public override string path => "Canvas";
        bool _is_visible;
        public override bool is_visible => _is_visible;
        bool _visible_by_default;
        protected override bool visible_by_default => _visible_by_default;
        ushort id;
        internal bool is_spawned;

        //public on_window_created_callback on_window_created;
        //public on_window_shown_callback on_window_shown;
        //public on_window_hidden_callback on_window_hidden;
        //public on_window_destroyed_callback on_window_destroyed;

        internal on_window_spawned_callback internal_on_window_spawned;
        internal on_window_despawned_callback internal_on_window_despawned;

        public window(ushort id, short _key, ITransportConnection _tc, bool _visible_by_default = true) : base() {
            this.id = id;
            this._key = _key;
            this._tc = _tc;
            this._is_visible = false;
            this._visible_by_default = _visible_by_default;
            this.is_spawned = false;
        }

        public void spawn(bool reliable = true) {
            if (is_spawned)
                throw new Exception($"window already spawned");
            EffectManager.SendUIEffect(Assets.FindEffectAssetByGuidOrLegacyId(Guid.Empty, id), key, tc, reliable);
            _is_visible = _visible_by_default;
            is_spawned = true;
            if (internal_on_window_spawned != null)
                internal_on_window_spawned();
            // todo other events after internal
        }

        public override void show(bool reliable = true) {
            if (!is_spawned)
                throw new Exception("window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, true);
            _is_visible = true;
        }

        public override void hide(bool reliable = true) {
            if (!is_spawned)
                throw new Exception("window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, false);
            _is_visible = false;
        }

        public void despawn() {
            if (!is_spawned)
                throw new Exception($"window already despawned");
            EffectManager.askEffectClearByID(id, tc);
            _is_visible = false;
            is_spawned = false;
            // todo other events before internal
            if (internal_on_window_despawned != null)
                internal_on_window_despawned();
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
