using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_progressbar_progress_changed_callback(int old_value, int new_value);

    public abstract class progressbar : control {
        protected control _parent;
        public override control parent => _parent;
        protected short _key;
        public override short key => _key;
        ITransportConnection _tc;
        internal override ITransportConnection tc => _tc;
        protected string _name;
        public override string name => _name;
        protected string _path;
        public override string path => _path;
        protected bool _is_visible;
        public override bool is_visible => _is_visible;
        protected int max, progress;
        protected window root;

        public on_progressbar_progress_changed_callback on_progress_changed;

        public progressbar(control _parent, short _key, ITransportConnection _tc, string _name, int max, bool _visible_by_default = true) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this.max = max;
            this._path = make_path();
            this._is_visible = _visible_by_default;
            this.progress = 0;
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

        public abstract void set_progress(int progress, bool reliable);

        public void increase_by(int val, bool reliable = true) {
            int p = progress + val;
            set_progress(p, reliable);
        }

        public void decrease_by(int val, bool reliable = true) {
            int p = progress - val;
            set_progress(p, reliable);
        }

        public void increment(bool reliable = true) {
            int p = progress + 1;
            set_progress(p, reliable);
        }

        public void decrement(bool reliable = true) {
            int p = progress - 1;
            set_progress(p, reliable);
        }

        public int get_progress() {
            return progress;
        }

        protected override void on_spawn() {
            progress = 0;
        }

        protected override void on_despawn() {
            progress = 0;
        }
    }
}
