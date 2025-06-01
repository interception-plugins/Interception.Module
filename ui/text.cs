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
    public class text : control {
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

        string _text;
        Color? _color;
        //List<KeyValuePair<string, string>> rich_text_params = new List<KeyValuePair<string, string>>();

        public text(control parent, short key, ITransportConnection tc, string name) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            //this._is_hidden = false;
            this._text = string.Empty;
            this._color = null;
        }

        /*
        public text(control parent, short key, ITransportConnection tc, string name, bool hidden_by_default) {
            this._parent = parent;
            this._key = key;
            this._tc = tc;
            this._name = name;
            //this._is_hidden = hidden_by_default;
            this._text = string.Empty;
            this._color = null;
        }
        */

        public override void show() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), true);
            //_is_hidden = false;
        }

        public override void hide() {
            EffectManager.sendUIEffectVisibility(key, tc, true, path_util.build_path(this), false);
            //_is_hidden = true;
        }

        public void set_text(string text) {
            EffectManager.sendUIEffectText(key, tc, true, path_util.build_path(this), _color != null ? $"<color={Palette.hex((Color32)_color)}>{text}</color>" : text);
            _text = text;
        }

        public string get_text() {
            return _text;
        }

        
        public void set_text_color(Color? color) {
            this._color = color;
            set_text(_text);
        }

        public void set_text_color(Color32? color) {
            this._color = (Color)color;
            set_text(_text);
        }

        public void set_text_color(string color) {
            if (color == null) {
                this._color = null;
                set_text(_text);
                return;
            }
            this._color = (Color)Palette.hex(color);
            set_text(_text);
        }

        /*
        public void add_rich_text_param(string key, string value) {
            
        }
        */
    }
}
