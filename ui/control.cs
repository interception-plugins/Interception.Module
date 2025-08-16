using System;
using System.Collections.Generic;

using SDG.NetTransport;
using SDG.Unturned;

namespace interception.ui {
    public delegate void on_control_shown_callback();
    public delegate void on_control_hidden_callback();

    public abstract class control {
        public abstract control parent { get; }
        public abstract short key { get; }
        internal abstract ITransportConnection tc { get; }
        public abstract string name { get; }
        public abstract string path { get; }
        protected virtual void on_spawn() { }
        protected virtual void on_despawn() { }

        public on_control_shown_callback on_shown;
        public on_control_shown_callback on_hidden;

        //protected List<control> controls;

        public virtual void show(bool reliable = true) {
            var root = get_root_window();
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, true);
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public virtual void hide(bool reliable = true) {
            var root = get_root_window();
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, path, false);
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
        }

        protected virtual string make_path() {
            var head = this;
            List<string> l = new List<string>();
            while (head != null) {
                if (!string.IsNullOrEmpty(head.name) && !string.IsNullOrWhiteSpace(head.name))
                    l.Insert(0, head.name);
                head = head.parent;
            }
            return string.Join("/", l.ToArray());
        }

        protected window get_root_window() {
            if (this is window)
                return (window)this;
            var head = this;
            while (head != null) {
                head = head.parent;
                if (head is window)
                    return (window)head;
            }
            //throw new Exception($"somehow parent window for control {path} was not found 0_o");
            return null;
        }

        public control() {
            //controls = new List<control>();
        }
    }
}
