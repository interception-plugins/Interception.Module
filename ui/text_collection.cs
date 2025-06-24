using System;
using System.Collections.Generic;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public sealed class text_collection : collection {
        Dictionary<int, string> texts;

        public text_collection(control _parent, short _key, ITransportConnection _tc, string _name, string _suffix_format, int _max_elements) 
            : base(_parent, _key, _tc, _name, _suffix_format, _max_elements) {
            this.texts = new Dictionary<int, string>(_max_elements);
            for (int i = 0; i < _max_elements; i++)
                this.texts.Add(i, string.Empty);
            root = get_root_window();
            if (root != null) {
                root.internal_on_spawned += on_spawn;
                root.internal_on_despawned += on_despawn;
            }
        }

        public void set_text(int index, string _text, bool reliable = true) {
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectText(key, tc, reliable, get_path_of(index), _text);
            texts[index] = _text;
        }

        public string get_text(int index) {
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            return texts[index];
        }

        protected override void on_spawn() {
            for (int i = 0; i < _max_elements; i++)
                texts[i] = string.Empty;
        }

        protected override void on_despawn() {
            for (int i = 0; i < _max_elements; i++)
                texts[i] = string.Empty;
        }
    }
}
