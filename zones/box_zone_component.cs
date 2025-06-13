using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

using interception.utils;

namespace interception.zones {
	public sealed class box_zone_component : zone_component {
		BoxCollider collider;

		void on_server_disconnected(CSteamID csid) {
			var p = PlayerTool.getPlayer(csid);
			if (p == null || !players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
			players.Remove(p.channel.owner.playerID.steamID.m_SteamID);
			if (zone_manager.debug_mode)
				Console.WriteLine($"zone exit ({gameObject.name}): {p.channel.owner.playerID.characterName}");
		}

		public void init(string name, Vector3 pos, Vector3 forward, Vector3 size) {
			gameObject.name = name;
			gameObject.transform.position = pos;
			gameObject.transform.forward = forward;
			gameObject.layer = 21;
			collider = gameObject.AddComponent<BoxCollider>();
			collider.isTrigger = true;
			collider.size = size;

			Provider.onServerDisconnected += on_server_disconnected;
			if (zone_manager.debug_mode)
				enable_debug();
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

		void OnTriggerEnter(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_enter != null)
				on_zone_enter(p);
			zone_manager.trigger_on_zone_enter_global(p, this);

			if (players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
			players.Add(p.channel.owner.playerID.steamID.m_SteamID, p);
			if (zone_manager.debug_mode)
				Console.WriteLine($"zone enter ({gameObject.name}): {p.channel.owner.playerID.characterName}");
		}

		void OnTriggerExit(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);

			if (!players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
			players.Remove(p.channel.owner.playerID.steamID.m_SteamID);
			if (zone_manager.debug_mode)
				Console.WriteLine($"zone exit ({gameObject.name}): {p.channel.owner.playerID.characterName}");
		}

		void OnDestroy() {
			Provider.onServerDisconnected -= on_server_disconnected;
			on_zone_enter = null;
			on_zone_exit = null;
		}
	}
}
