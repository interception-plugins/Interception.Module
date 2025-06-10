using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

using interception.utils;

namespace interception.zones {
    public delegate void on_zone_enter_callback(Player player);
    public delegate void on_zone_exit_callback(Player player);

    public class zone_component : MonoBehaviour {
        Coroutine debug_routine;

        //public virtual void destroy() {
        //    Destroy(gameObject);
        //}

#pragma warning disable CS0618
        public virtual IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
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
#pragma warning restore CS0618

        internal void enable_debug() {
            if (debug_routine != null) return;
            debug_routine = StartCoroutine(debug_routine_worker());
        }

        internal void disable_debug() {
            if (debug_routine == null) return;
            StopCoroutine(debug_routine);
            debug_routine = null;
        }

        public virtual List<Player> get_players() { return null; }
    }
}
