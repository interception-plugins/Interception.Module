using System;

using UnityEngine;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public sealed class text_progressbar : progressbar {
        char fill_char;

        public text_progressbar(control _parent, short _key, ITransportConnection _tc, string _name, char fill_char, int max_chars) 
            : base(_parent, _key, _tc, _name, max_chars) {
            this.fill_char = fill_char;
        }

        public override void set_progress(int progress, bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            int old = this.progress;
            this.progress = Mathf.Clamp(progress, 0, max);
            EffectManager.sendUIEffectText(key, tc, reliable, path, new string(fill_char, this.progress));
            if (on_progress_changed != null)
                on_progress_changed(old, this.progress);
            ui_manager.trigger_on_progressbar_progress_changed_global(old, this.progress, this);
        }
    }
}
