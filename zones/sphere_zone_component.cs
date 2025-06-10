using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
	public sealed class sphere_zone_component : zone_component {
		SphereCollider collider;

		Dictionary<ulong, Player> players;

		public on_zone_enter_callback on_zone_enter;
		public on_zone_exit_callback on_zone_exit;

		void zone_enter(Player player) {
			if (players.ContainsKey(player.channel.owner.playerID.steamID.m_SteamID)) return;
			players.Add(player.channel.owner.playerID.steamID.m_SteamID, player);
		}

		void zone_exit(Player player) {
			if (!players.ContainsKey(player.channel.owner.playerID.steamID.m_SteamID)) return;
			players.Remove(player.channel.owner.playerID.steamID.m_SteamID);
		}

		void on_server_disconnected(CSteamID csid) {
			var p = PlayerTool.getPlayer(csid);
			if (p == null || !players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
		}

		internal void init(string name, Vector3 pos, float radius) {
			gameObject.name = name;
			gameObject.transform.position = pos;
			gameObject.layer = 21;
			collider = gameObject.AddComponent<SphereCollider>();
			collider.isTrigger = true;
			collider.radius = radius;

			players = new Dictionary<ulong, Player>();

			on_zone_enter = zone_enter;
			on_zone_exit = zone_exit;
			Provider.onServerDisconnected += on_server_disconnected;
			if (zone_manager.debug_mode)
				enable_debug();
		}

		public override List<Player> get_players() => players.Values.ToList();

#pragma warning disable CS0618
		public override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
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

		void OnTriggerEnter(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();

			if (on_zone_enter != null)
				on_zone_enter(p);
			zone_manager.trigger_on_zone_enter_global(p, this);
		}

		void OnTriggerExit(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
		}

		void OnDestroy() {
			Provider.onServerDisconnected -= on_server_disconnected;
			on_zone_enter = null;
			on_zone_exit = null;
		}
	}
}
