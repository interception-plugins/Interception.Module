using System;
using System.Collections.Generic;

using SDG.Unturned;
using SDG.NetTransport;

namespace interception.ui {
    public sealed class image_collection : collection {
        Dictionary<int, string> urls;

        public image_collection(control _parent, short _key, ITransportConnection _tc, string _name, string _suffix_format, int _max_elements) 
            : base(_parent, _key, _tc, _name, _suffix_format, _max_elements) {
            this.urls = new Dictionary<int, string>(_max_elements);
            for (int i = 0; i < _max_elements; i++)
                this.urls.Add(i, string.Empty);
            root = get_root_window();
            if (root != null) {
                root.internal_on_spawned += on_spawn;
                root.internal_on_despawned += on_despawn;
            }
        }

        public void set_image(int index, string _url, bool reliable = true, bool should_cache = true, bool force_refresh = false) {
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            if (root != null && !root.is_spawned)
                throw new Exception("root window is despawned");
            EffectManager.sendUIEffectImageURL(key, tc, reliable, get_path_of(index), _url, should_cache, force_refresh);
            urls[index] = _url;
        }

        public string get_image_url(int index) {
            if (index < 0 || index >= max_elements)
                throw new ArgumentOutOfRangeException($"index must be in range 0-{max_elements}");
            return urls[index];
        }

        protected override void on_spawn() {
            for (int i = 0; i < _max_elements; i++)
                urls[i] = string.Empty;
        }

        protected override void on_despawn() {
            for (int i = 0; i < _max_elements; i++)
                urls[i] = string.Empty;
        }
    }
}
