using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using SDG.NetTransport;
using UnityEngine;

using interception.utils;

namespace interception.ui {
    public delegate void on_progress_changed_callback(int old_value, int new_value);

    public sealed class progressbar_text : control {
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
        int max_chars, progress;
        char fill_char;
        window root;

        public on_progress_changed_callback on_progress_changed;

        public progressbar_text(control _parent, short _key, ITransportConnection _tc, string _name, int max_chars, char fill_char, bool _visible_by_default = true) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this.max_chars = max_chars;
            this.fill_char = fill_char;
            this._path = make_path();
            this._is_visible = _visible_by_default;
            this.progress = 0;
            this.root = get_root_window();
            if (root != null) {
                root.internal_on_window_spawned += on_spawn;
                root.internal_on_window_despawned += on_despawn;
            }
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

        public void set_progress(int progress, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            int old = this.progress;
            this.progress = math_util.clamp(progress, 0, max_chars);
            EffectManager.sendUIEffectText(key, tc, reliable, path, new string(fill_char, this.progress));
            if (on_progress_changed != null)
                on_progress_changed(old, this.progress);
        }

        public void increase_by(int val, bool reliable = true) {
            set_progress(progress += val, reliable);
        }

        public void decrease_by(int val, bool reliable = true) {
            set_progress(progress -= val, reliable);
        }

        public void increment(bool reliable = true) {
            set_progress(++progress, reliable);
        }

        public void decrement(bool reliable = true) {
            set_progress(--progress, reliable);
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
