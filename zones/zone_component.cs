using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SDG.Unturned;

using interception.utils;

// todo somehow 0_o
namespace interception.zones {
    public delegate void on_zone_enter_callback(Player player);
    public delegate void on_zone_exit_callback(Player player);

    public class zone_component : MonoBehaviour {
        public virtual void destroy() {
            Destroy(gameObject);
        }

        public virtual IEnumerator<WaitForSecondsRealtime> debug_routine() {
            for (; ; ) {
                var a = collider_util.get_collider_vertex_positions(gameObject);
                var len = a.Length;
                for (int i = 0; i < len; i++) {
                    EffectManager.sendEffect(132, 512f, a[i]);
                }
                EffectManager.sendEffect(133, 512f, gameObject.transform.position);
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        public virtual List<Player> get_players() { return null; }
    }
}
