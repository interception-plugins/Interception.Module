using System;

using UnityEngine;

using SDG.Unturned;

namespace interception.utils {
    public static class raycast_util {
        public static Component raycast_all(Vector3 origin, Vector3 direction, float distance, Type type) {
            var hits = Physics.RaycastAll(origin, direction, distance);
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform != null) {
                    var comp = hits[i].transform.GetComponent(type);
                    if (comp != null) return comp;
                }
                if (hits[i].collider != null && hits[i].collider.transform != null) {
                    var comp = hits[i].collider.transform.GetComponentInParent(type);
                    if (comp != null) return comp;
                }
            }
            return null;
        }

        public static Component raycast_all(Player player, float distance, Type type) {
            if (player == null)
                throw new NullReferenceException("player is null");
            return raycast_all(player.look.aim.position, player.look.aim.forward, distance, type);
        }

        public static T raycast_all<T>(Vector3 origin, Vector3 direction, float distance) {
            var hits = Physics.RaycastAll(origin, direction, distance);
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform != null) {
                    var comp = hits[i].transform.GetComponent<T>();
                    if (comp != null) return comp;
                }
                if (hits[i].collider != null && hits[i].collider.transform != null) {
                    var comp = hits[i].collider.transform.GetComponentInParent<T>();
                    if (comp != null) return comp;
                }
            }
            return default(T);
        }

        public static T raycast_all<T>(Player player, float distance) {
            if (player == null)
                throw new NullReferenceException("player is null");
            return raycast_all<T>(player.look.aim.position, player.look.aim.forward, distance);
        }

        public static Component raycast(Vector3 origin, Vector3 direction, float distance, int mask, Type type) {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask))
                return null;
            if (hit.transform != null) {
                var comp = hit.transform.GetComponent(type);
                if (comp != null) return comp;
            }
            if (hit.collider != null && hit.collider.transform != null) {
                var comp = hit.collider.transform.GetComponentInParent(type);
                if (comp != null) return comp;
            }
            return null;
        }

        public static Component raycast(Player player, float distance, int mask, Type type) {
            if (player == null)
                throw new NullReferenceException("player is null");
            return raycast(player.look.aim.position, player.look.aim.forward, distance, mask, type);
        }

        public static T raycast<T>(Vector3 origin, Vector3 direction, float distance, int mask) {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask))
                return default(T);
            if (hit.transform != null) {
                var comp = hit.transform.GetComponent<T>();
                if (comp != null) return comp;
            }
            if (hit.collider != null && hit.collider.transform != null) {
                var comp = hit.collider.transform.GetComponentInParent<T>();
                if (comp != null) return comp;
            }
            return default(T);
        }

        public static T raycast<T>(Player player, float distance, int mask) {
            if (player == null)
                throw new NullReferenceException("player is null");
            return raycast<T>(player.look.aim.position, player.look.aim.forward, distance, mask);
        }

        public static Vector3? get_eyes_position(Player player, float distance, int mask = 473546752) { // 473546752 is RayMasks.BLOCK_COLLISION
            if (player == null)
                throw new NullReferenceException("player is null");
            if (!Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, distance, mask) || hit.transform == null)
                return null;
            return hit.point;
        }
    }
}
