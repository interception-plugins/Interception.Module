using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_window_created_global_callback(ushort id, short key, ITransportConnection tc);
    public delegate void on_window_shown_global_callback(ushort id, short key, ITransportConnection tc);
    public delegate void on_window_hidden_global_callback(ushort id, short key, ITransportConnection tc);
    public delegate void on_window_destroyed_global_callback(ushort id, short key, ITransportConnection tc);
    public delegate void on_window_created_callback(ushort id, short key);
    public delegate void on_window_shown_callback(ushort id, short key);
    public delegate void on_window_hidden_callback(ushort id, short key);
    public delegate void on_window_destroyed_callback(ushort id, short key);

    public class window : control {
        public override control parent => null;
        public override string name => "Canvas";
        ushort id;
        List<control> _controls;

        bool _is_cleared;
        public bool is_cleared {
            get {
                return _is_cleared;
            }
            private set {
                _is_cleared = value;
            }
        }

        //bool _is_hidden;
        //public override bool is_visible => !_is_hidden;
        
        short _key;
        protected override short key => _key;
        ITransportConnection _tc;
        protected override ITransportConnection tc => _tc;

        public static on_window_created_global_callback on_window_created_global = delegate (ushort effect_id, short effect_key, ITransportConnection player_tc) { };
        public static on_window_shown_global_callback on_window_shown_global = delegate (ushort effect_id, short effect_key, ITransportConnection player_tc) { };
        public static on_window_hidden_global_callback on_window_hidden_global = delegate (ushort effect_id, short effect_key, ITransportConnection player_tc) { };
        public static on_window_destroyed_global_callback on_window_destroyed_global = delegate (ushort effect_id, short effect_key, ITransportConnection player_tc) { };

        public on_window_created_callback on_window_created;
        public on_window_shown_callback on_window_shown;
        public on_window_hidden_callback on_window_hidden;
        public on_window_destroyed_callback on_window_destroyed;

        public window(ushort id, short key, ITransportConnection tc) {
            this.id = id;
            this._key = key;
            this._tc = tc;
            this._is_cleared = true;
            //this._is_hidden = false;
            this._controls = new List<control>();
            on_window_created = delegate (ushort effect_id, short effect_key) { };
            on_window_shown = delegate (ushort effect_id, short effect_key) { };
            on_window_hidden = delegate (ushort effect_id, short effect_key) { };
            on_window_destroyed = delegate (ushort effect_id, short effect_key) { };
        }

        /*
        public window(ushort id, short key, ITransportConnection tc, bool hidden_by_default) {
            this.id = id;
            this._key = key;
            this._tc = tc;
            this._is_destroyed = true;
            //this._is_hidden = hidden_by_default;
            this._controls = new List<control>();
            on_window_created = delegate (ushort effect_id, short effect_key) { };
            on_window_shown = delegate (ushort effect_id, short effect_key) { };
            on_window_hidden = delegate (ushort effect_id, short effect_key) { };
            on_window_destroyed = delegate (ushort effect_id, short effect_key) { };
        }
        */

        public void send() {
            if (!is_cleared)
                throw new Exception($"window already created");
            EffectManager.SendUIEffect(Assets.FindEffectAssetByGuidOrLegacyId(Guid.Empty, id), key, tc, true);
            is_cleared = false;
            if (on_window_created_global != null)
                on_window_created_global(id, key, tc);
            if (on_window_created != null)
                on_window_created(id, key);
        }

        public override void show() {
            if (is_cleared)
                throw new Exception("cannot show destroyed window");
            EffectManager.sendUIEffectVisibility(key, tc, true, name, true);
            //_is_hidden = false;
            if (on_window_shown_global != null)
                on_window_shown_global(id, key, tc);
            if (on_window_shown != null)
                on_window_shown(id, key);
        }

        public override void hide() {
            if (is_cleared)
                throw new Exception("cannot hide destroyed window");
            EffectManager.sendUIEffectVisibility(key, tc, true, name, false);
            //_is_hidden = true;
            if (on_window_hidden_global != null)
                on_window_hidden_global(id, key, tc);
            if (on_window_hidden != null)
                on_window_hidden(id, key);
        }

        public void clear() {
            if (is_cleared)
                throw new Exception("window already destroyed");
            EffectManager.askEffectClearByID(id, tc);
            is_cleared = true;
            if (on_window_destroyed_global != null)
                on_window_destroyed_global(id, key, tc);
            if (on_window_destroyed != null)
                on_window_destroyed(id, key);
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

        public image add_image(string name) {
            image ctrl = new image(this, key, tc, name);
            _controls.Add(ctrl);
            return (image)_controls.Last();
        }
    }
}
