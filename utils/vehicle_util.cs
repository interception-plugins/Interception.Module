using System;

using UnityEngine;
using SDG.Unturned;

namespace interception.utils {
    public static class vehicle_util {
		public static Vector3 get_position_for_vehicle(Player player) {
			Vector3 vector = player.transform.position + player.transform.forward * 6f;
			RaycastHit raycastHit;
			Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
			if (raycastHit.collider != null)
				vector.y = raycastHit.point.y + 16f;
			return vector;
		}

		public static Vector3 get_position_for_vehicle(Transform transform) {
			Vector3 vector = transform.position + transform.forward * 6f;
			RaycastHit raycastHit;
			Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
			if (raycastHit.collider != null)
				vector.y = raycastHit.point.y + 16f;
			return vector;
		}

		public static Vector3 get_position_for_vehicle(Vector3 pos) {
			Vector3 vector = pos;
			RaycastHit raycastHit;
			Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
			if (raycastHit.collider != null)
				vector.y = raycastHit.point.y + 16f;
			return vector;
		}
	}
}
