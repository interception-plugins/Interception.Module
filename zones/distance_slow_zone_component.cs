using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

namespace interception.zones {
	public sealed class distance_slow_zone_component : zone_component {
		DateTime last_upd;
		float _radius;
		public float radius {
			get {
				return _radius;
			}
			private set {
				_radius = value;
			}
		}

		public void init(string name, Vector3 pos, float radius) {
			gameObject.name = name;
			gameObject.transform.position = pos;

			last_upd = DateTime.UtcNow;
			this.radius = radius;

			base.init();
		}

#pragma warning disable CS0618
		protected override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
			for (; ; ) {
				for (int i = 360; i > 0; i -= 90) {
					float x = gameObject.transform.position.x + radius * (float)Math.Cos(i * (Math.PI / 180));
					float z = gameObject.transform.position.z + radius * (float)Math.Sin(i * (Math.PI / 180));
					//Console.WriteLine($"[{x},{y}]");
					EffectManager.sendEffect(132, 512f, new Vector3(x, gameObject.transform.position.y, z));
				}
				EffectManager.sendEffect(132, 512f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + radius, gameObject.transform.position.z));
				EffectManager.sendEffect(132, 512f, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - radius, gameObject.transform.position.z));
				EffectManager.sendEffect(133, 512f, gameObject.transform.position);
				yield return new WaitForSecondsRealtime(1f);
			}
		}
#pragma warning restore CS0618

		void Update() {
			if ((DateTime.UtcNow - last_upd).TotalMilliseconds < 500) return;
			var len = Provider.clients.Count;
			for (int i = 0; i < len; i++) {
				if ((Provider.clients[i].player.transform.position - gameObject.transform.position).sqrMagnitude <= radius * radius && !Provider.clients[i].player.life.isDead) {
					if (!players.ContainsKey(Provider.clients[i].playerID.steamID.m_SteamID)) {
						players.Add(Provider.clients[i].playerID.steamID.m_SteamID, Provider.clients[i].player);
						if (on_zone_enter != null)
							on_zone_enter(Provider.clients[i].player);
						zone_manager.trigger_on_zone_enter_global(Provider.clients[i].player, this);
						if (zone_manager.debug_mode)
							Console.WriteLine($"zone enter ({gameObject.name}): {Provider.clients[i].playerID.characterName}");
					}
				}
				else {
					if (players.ContainsKey(Provider.clients[i].playerID.steamID.m_SteamID)) {
						players.Remove(Provider.clients[i].playerID.steamID.m_SteamID);
						if (on_zone_exit != null)
							on_zone_exit(Provider.clients[i].player);
						zone_manager.trigger_on_zone_exit_global(Provider.clients[i].player, this);
						if (zone_manager.debug_mode)
							Console.WriteLine($"zone exit ({gameObject.name}): {Provider.clients[i].playerID.characterName}");
					}
				}
			}
			last_upd = DateTime.UtcNow;
		}
	}
}
