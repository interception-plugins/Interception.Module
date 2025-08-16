using System;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_button_click_callback();

    public sealed class button : control {
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

        public on_button_click_callback on_click;

        public button(control _parent, short _key, ITransportConnection _tc, string _name) : base() {
            this._parent = _parent;
            this._key = _key;
            this._tc = _tc;
            this._name = _name;
            this._path = make_path();
            ui_manager.add_control(this);
        }

        internal void click() {
            if (on_click != null)
                on_click();
            ui_manager.trigger_on_button_click_global(this);
        }
    }
}
