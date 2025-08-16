using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

namespace interception.zones {
	public sealed class sphere_zone_component : zone_component {
		SphereCollider collider;

		public Bounds bounds => collider.bounds;
		public float radius => collider.radius;

		public void init(string name, Vector3 pos, float radius) {
			gameObject.name = name;
			gameObject.transform.position = pos;
			gameObject.layer = 21;
			collider = gameObject.AddComponent<SphereCollider>();
			collider.isTrigger = true;
			collider.radius = radius;

			base.init();
		}

#pragma warning disable CS0618
		protected override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
			for (; ; ) {
				for (int i = 360; i > 0; i -= 90) {
					float x = gameObject.transform.position.x + collider.radius * (float)Math.Cos(i * (Math.PI / 180));
					float z = gameObject.transform.position.z + collider.radius * (float)Math.Sin(i * (Math.PI / 180));
					//Console.WriteLine($"[{x},{y}]");
					EffectManager.sendEffect(132, 512f, new Vector3(x, gameObject.transform.position.y, z));
				}
				EffectManager.sendEffect(132, 512f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + collider.radius, gameObject.transform.position.z));
				EffectManager.sendEffect(132, 512f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - collider.radius, gameObject.transform.position.z));
				EffectManager.sendEffect(133, 512f, gameObject.transform.position);
				yield return new WaitForSecondsRealtime(1f);
			}
		}
#pragma warning restore CS0618

		public override bool is_position_in_zone(Vector3 pos) {
			return (gameObject.transform.position - pos).sqrMagnitude <= radius * radius;
		}

		public override bool is_transform_in_zone(Transform t) {
			return (gameObject.transform.position - t.position).sqrMagnitude <= radius * radius;
		}
	}
}
