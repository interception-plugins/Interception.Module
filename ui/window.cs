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

        public on_window_spawned_callback on_spawned;
        public on_window_despawned_callback on_despawned;

        internal on_window_spawned_callback internal_on_spawned;
        internal on_window_despawned_callback internal_on_despawned;

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
            if (internal_on_spawned != null)
                internal_on_spawned();
            if (on_spawned != null)
                on_spawned();
            ui_manager.trigger_on_window_spawned_global_global(this);
        }

        public override void show(bool reliable = true) {
            if (!is_spawned)
                throw new Exception("window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, true);
            _is_visible = true;
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (!is_spawned)
                throw new Exception("window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, false);
            _is_visible = false;
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        public void despawn() {
            if (!is_spawned)
                throw new Exception($"window already despawned");
            EffectManager.askEffectClearByID(id, tc);
            _is_visible = false;
            is_spawned = false;
            if (on_despawned != null)
                on_despawned();
            ui_manager.trigger_on_window_despawned_global_global(this);
            if (internal_on_despawned != null)
                internal_on_despawned();
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

        public button add_button(string name) {
            return new button(this, key, tc, name);
        }

        public textbox add_textbox(string name) {
            return new textbox(this, key, tc, name);
        }

        public progressbar_text add_progressbar(string name, int max_chars, char fill_char) {
            return new progressbar_text(this, key, tc, name, max_chars, fill_char);
        }
    }
}
