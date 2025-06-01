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
    public delegate void on_click_global_callback(short key, string name, ITransportConnection tc); // todo
    public delegate void on_click_callback(short key, string name);

    public class button : control {
        short _key;
        protected override short key => _key;
        ITransportConnection _tc;
        protected override ITransportConnection tc => _tc;
        string _name;
        public override string name => _name;
        control _parent;
        public override control parent => _parent;

        List<control> _controls;

        public on_click_callback on_click;

        public button(control parent, short key, ITransportConnection tc, string name) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            this._controls = new List<control>();
            this.on_click = delegate (short effect_key, string button_name) { };
        }

        public override void show() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), true);
        }

        public override void hide() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), false);
        }

        public text add_text(string name) {
            text ctrl = new text(this, key, tc, name);
            _controls.Add(ctrl);
            return (text)_controls.Last();
        }

        public image add_image(string name) {
            image ctrl = new image(this, key, tc, name);
            _controls.Add(ctrl);
            return (image)_controls.Last();
        }
    }
}
