using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

using interception.utils;

namespace interception.zones {
	public sealed class box_zone_component : zone_component {
		BoxCollider collider;

		public Bounds bounds => collider.bounds;
		public Vector3 size => collider.size;
		public Vector3 forward => gameObject.transform.forward;

		public void init(string name, Vector3 pos, Vector3 forward, Vector3 size) {
			gameObject.name = name;
			gameObject.transform.position = pos;
			gameObject.transform.forward = forward;
			gameObject.layer = 21;
			collider = gameObject.AddComponent<BoxCollider>();
			collider.isTrigger = true;
			collider.size = size;

			base.init();
		}

#pragma warning disable CS0618
		protected override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
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
	}
}
