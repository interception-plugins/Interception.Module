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

        public tab(control _parent, short _key, ITransportConnection _tc, string _name) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
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

        public text_progressbar add_text_progressbar(string name, char fill_char, int max_chars) {
            return new text_progressbar(this, key, tc, name, fill_char, max_chars);
        }

        public image_progressbar add_image_progressbar(string name, string child_name_format, int max_children_count) {
            return new image_progressbar(this, key, tc, name, child_name_format, max_children_count);
        }

        public text_collection add_text_collection(string name, string suffix_format, int max_elements) {
            return new text_collection(this, key, tc, name, suffix_format, max_elements);
        }

        public image_collection add_image_collection(string name, string suffix_format, int max_elements) {
            return new image_collection(this, key, tc, name, suffix_format, max_elements);
        }

        public button_collection add_button_collection(string name, string suffix_format, int max_elements) {
            return new button_collection(this, key, tc, name, suffix_format, max_elements);
        }
    }
}
