using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public class tab : control {
        short _key;
        protected override short key => _key;
        ITransportConnection _tc;
        protected override ITransportConnection tc => _tc;
        string _name;
        public override string name => _name;
        //bool _is_hidden;
        //public override bool is_visible => !_is_hidden;
        control _parent;
        public override control parent => _parent;

        List<control> _controls;

        public tab(control parent, short key, ITransportConnection tc, string name) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            //this._is_hidden = false;
            this._controls = new List<control>();
        }

        /*
        public tab(control parent, short key, ITransportConnection tc, string name, bool hidden_by_default) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            this._is_hidden = hidden_by_default;
            this._controls = new List<control>();
        }
        */

        public override void show() {
            EffectManager.sendUIEffectVisibility(key, tc, true, name, true);
            //_is_hidden = false;
        }

        public override void hide() {
            EffectManager.sendUIEffectVisibility(key, tc, true, name, false);
            //_is_hidden = true;
        }

        public tab add_tab(string name) {
            tab ctrl = new tab(this, key, tc, name);
            _controls.Add(ctrl);
            return (tab)_controls.Last();
        }

        public text add_text(string name) {
            text ctrl = new text(this, key, tc, name);
            _controls.Add(ctrl);
            return (text)_controls.Last();
        }
    }
}
