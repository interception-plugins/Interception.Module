using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

using interception.utils;

namespace interception.zones {
    public delegate void on_zone_enter_callback(Player player);
    public delegate void on_zone_exit_callback(Player player);

    public class zone_component : MonoBehaviour {
        protected Coroutine debug_routine;

        //public virtual void destroy() {
        //    Destroy(gameObject);
        //}

#pragma warning disable CS0618
        protected virtual IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
            for (; ; ) {
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

        //protected Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();
        public virtual List<Player> get_players() { return null; }
    }
}
