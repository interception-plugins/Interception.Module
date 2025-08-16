using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

using UnityEngine;
using SDG.Unturned;

namespace interception.zones {
	public sealed class mesh_zone_component : zone_component {
		MeshCollider collider;
		GameObject child_object;

		public void init(string name, Vector3 pos, float height, int? mask, bool destroy_if_failed) {
			gameObject.name = name;
			//gameObject.transform.position = pos;
			gameObject.layer = 21;

			
			RaycastHit hit;
			var raycast = Physics.Raycast(pos + new Vector3(0f, height, 0f), -Vector3.up, out hit, height * 2f, 
				mask == null ? RayMasks.SMALL | RayMasks.MEDIUM | RayMasks.LARGE/* | RayMasks.NAVMESH | RayMasks.CLIP*/ : (int)mask);
			if (!raycast) {
				if (destroy_if_failed)
					Destroy(gameObject);
				throw new Exception("raycast returned false");
			}
			if (hit.transform == null) {
				if (destroy_if_failed)
					Destroy(gameObject);
				throw new Exception("cannot get transform from raycast hit");
			}
			if (hit.transform.gameObject == null) {
				if (destroy_if_failed)
					Destroy(gameObject);
				throw new Exception("cannot get gameObject from raycast hit");
			}
			gameObject.transform.position = hit.transform.position;
			gameObject.transform.rotation = hit.transform.rotation;
			child_object = Instantiate(hit.transform.gameObject);
			child_object.transform.parent = gameObject.transform;
			if (child_object.GetComponent<MeshCollider>() == null && child_object.GetComponentInChildren<MeshCollider>() == null) {
				if (destroy_if_failed)
					Destroy(gameObject);
				throw new Exception("cannot get mesh collider from raycast hit");
			}
			try {
				child_object.AddComponent<mesh_zone_helper_component>().init();
			}
			catch (Exception ex) {
				if (destroy_if_failed)
					Destroy(gameObject);
				ExceptionDispatchInfo.Capture(ex).Throw();
				//throw new Exception("Rethrown", ex);
				return;
            }

			collider = child_object.GetComponent<MeshCollider>();

			base.init();
		}

#pragma warning disable CS0618
		protected override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
			for (; ; ) {
				var len = collider.sharedMesh.vertices.Length;
				for (int i = 0; i < len; i++) {
					EffectManager.sendEffect(132, 512f, transform.TransformPoint(collider.sharedMesh.vertices[i]));
				}
				EffectManager.sendEffect(133, 512f, child_object.transform.position);
				yield return new WaitForSecondsRealtime(1f);
			}
		}
#pragma warning restore CS0618

		static Vector3 mul(Vector3 v1, Vector3 v2) {
			Vector3 result = v1;
			result.x *= v2.x;
			result.y *= v2.y;
			result.z *= v2.z;
			return result;
		}

		// todo im pretty sure its wrong
		// also maybe simply calling bounds.Contains will work, i dunno
		public override bool is_position_in_zone(Vector3 pos) {
			var len = collider.sharedMesh.vertices.Length;
			for (int i = 0; i < len; i++) {
				if (Math.Asin(Vector3.Dot(collider.sharedMesh.vertices[i].normalized,
					Vector3.Normalize(mul(collider.sharedMesh.vertices[i], gameObject.transform.position) - pos))) <= 0.0) return false;
			}
			return true;
		}

		public override bool is_transform_in_zone(Transform t) {
			return is_position_in_zone(t.position);
		}
	}
}
