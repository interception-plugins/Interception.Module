using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
	public sealed class distance_fast_zone_component : zone_component {
		Dictionary<ulong, Player> players;
		DateTime last_upd;
		float radius;
		List<RegionCoordinate> coords;

		public on_zone_enter_callback on_zone_enter;
		public on_zone_exit_callback on_zone_exit;

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

			players = new Dictionary<ulong, Player>();
			last_upd = DateTime.UtcNow;
			this.radius = radius;
			coords = new List<RegionCoordinate>();
			Regions.getRegionsInRadius(pos, radius, coords);
			var len = coords.Count;
			for (int i = 0; i < len; i++)
				if (!zone_manager.regions_to_check.ContainsKey(coords[i]))
					zone_manager.regions_to_check.Add(coords[i], new Dictionary<ulong, Player>());

			Provider.onServerDisconnected += on_server_disconnected;
			if (zone_manager.debug_mode)
				enable_debug();
		}

		public override List<Player> get_players() => players.Values.ToList();

#pragma warning disable CS0618
		public override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
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

		void OnDestroy() {
			Provider.onServerDisconnected -= on_server_disconnected;
			on_zone_enter = null;
			on_zone_exit = null;
		}

		void Update() {
			if ((DateTime.UtcNow - last_upd).TotalMilliseconds < 500) return;
			var len = coords.Count;
			for (int i = 0; i < len; i++) {
				var region_players = zone_manager.regions_to_check[coords[i]].Values.ToList();
				var len2 = region_players.Count;
				for (int j = 0; j < len2; j++) {
					if ((region_players[j].transform.position - gameObject.transform.position).sqrMagnitude <= radius * radius) {
						if (!players.ContainsKey(region_players[j].channel.owner.playerID.steamID.m_SteamID)) {
							players.Add(region_players[j].channel.owner.playerID.steamID.m_SteamID, region_players[j]);
							if (on_zone_enter != null)
								on_zone_enter(region_players[j]);
							zone_manager.trigger_on_zone_enter_global(region_players[j], this);
						}
					}
					else {
						if (players.ContainsKey(region_players[j].channel.owner.playerID.steamID.m_SteamID)) {
							players.Remove(region_players[j].channel.owner.playerID.steamID.m_SteamID);
							if (on_zone_exit != null)
								on_zone_exit(region_players[j]);
							zone_manager.trigger_on_zone_exit_global(region_players[j], this);
						}
					}
				}
			}
			last_upd = DateTime.UtcNow;
		}
	}
}
