using System;

using SDG.Unturned;
using SDG.NetTransport;

using interception.utils;

namespace interception.ui {
    public sealed class image_progressbar : progressbar {
        string child_name_format;

        public image_progressbar(control _parent, short _key, ITransportConnection _tc, string _name, int max_children_count, string child_name_format, bool _visible_by_default = true) 
            : base(_parent, _key, _tc, _name, max_children_count, _visible_by_default) {
            this.child_name_format = child_name_format;
        }

        public override void set_progress(int progress, bool reliable = true) {
            if (!root.is_spawned)
                throw new Exception("root window is despawned");
            int old = this.progress;
            this.progress = math_util.clamp(progress, 0, max);
            int i = 0;
            for (; i < progress; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, $"{path}/{string.Format(child_name_format, i)}", true);
            for (; i < max; i++)
                EffectManager.sendUIEffectVisibility(key, tc, reliable, $"{path}/{string.Format(child_name_format, i)}", false);
            if (on_progress_changed != null)
                on_progress_changed(old, this.progress);
            ui_manager.trigger_on_progressbar_progress_changed(old, this.progress, this);
        }
    }
}
