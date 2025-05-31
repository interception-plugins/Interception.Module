using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SDG.Unturned;

namespace interception.zones {
    public delegate void on_zone_enter_callback(Player player);
    public delegate void on_zone_exit_callback(Player player);

    public class zone_component : MonoBehaviour { // todo rename it to *_zone_component
        public virtual void destroy() {
            Destroy(gameObject);
        }

        public virtual List<Player> get_players() { return null; }
    }
}
