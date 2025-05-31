using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using SDG.NetTransport;
using UnityEngine;

using interception.util;

namespace interception.ui {
    public class image : control {
        short _key;
        protected override short key => _key;
        ITransportConnection _tc;
        protected override ITransportConnection tc => _tc;
        string _name;
        public override string name => _name;
        control _parent;
        public override control parent => _parent;

        string _url;

        public image(control parent, short key, ITransportConnection tc, string name) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            this._url = string.Empty;
        }

        public override void show() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), true);
        }

        public override void hide() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), false);
        }

        public void set_url(string url) {
            EffectManager.sendUIEffectImageURL(key, tc, true, path_util.build_path(this), url);
            _url = url;
        }

        public string get_url() {
            return _url;
        }
    }
}
