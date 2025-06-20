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
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, true);
            _is_visible = true;
            if (on_shown != null)
                on_shown();
            ui_manager.trigger_on_control_shown_global(this);
        }

        public override void hide(bool reliable = true) {
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectVisibility(key, tc, reliable, name, false);
            _is_visible = false;
            if (on_hidden != null)
                on_hidden();
            ui_manager.trigger_on_control_hidden_global(this);
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

        public text_progressbar add_text_progressbar(string name, int max_chars, char fill_char) {
            return new text_progressbar(this, key, tc, name, max_chars, fill_char);
        }

        public image_progressbar add_image_progressbar(string name, int max_children_count, string child_name_format) {
            return new image_progressbar(this, key, tc, name, max_children_count, child_name_format);
        }

        public button_collection add_button_collection(string name, int max_buttons, string suffix_format) {
            return new button_collection(this, key, tc, name, max_buttons, suffix_format);
        }
    }
}
