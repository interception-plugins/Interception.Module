using System;

using UnityEngine;
using SDG.Unturned;

namespace interception.utils {
    public static class vehicle_util {
		public static Vector3 get_position_for_vehicle(Vector3 pos) {
			Vector3 vector = pos;
			RaycastHit raycastHit;
			Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
			if (raycastHit.collider != null)
				vector.y = raycastHit.point.y + 16f;
			return vector;
		}

		public static Vector3 get_position_for_vehicle(Transform transform) {
			if (transform == null)
				throw new NullReferenceException("transform is null");
			return get_position_for_vehicle(transform.position);
		}

		public static Vector3 get_position_for_vehicle(Player player) {
			if (player == null)
				throw new NullReferenceException("player is null");
			return get_position_for_vehicle(player.transform.position);
		}
	}
}
