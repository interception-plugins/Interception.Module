﻿using System;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

namespace interception.zones {
	public sealed class distance_fast_zone_component : zone_component {
		DateTime last_upd;
		float radius;
		List<RegionCoordinate> coords;

		public void init(string name, Vector3 pos, float radius) {
			gameObject.name = name;
			gameObject.transform.position = pos;

			last_upd = DateTime.UtcNow;
			this.radius = radius;
			coords = new List<RegionCoordinate>();
			Regions.getRegionsInRadius(pos, radius, coords);
			var len = coords.Count;
			for (int i = 0; i < len; i++)
				if (!zone_manager.regions_to_check.ContainsKey(coords[i]))
					zone_manager.regions_to_check.Add(coords[i], new Dictionary<ulong, Player>());

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
			if ((DateTime.UtcNow - last_upd).TotalMilliseconds < 250) return;
			var len = coords.Count;
			for (int i = 0; i < len; i++) {
				foreach (var region_player in zone_manager.regions_to_check[coords[i]].Values) {
					if ((region_player.transform.position - gameObject.transform.position).sqrMagnitude <= radius * radius && !region_player.life.isDead) {
						if (!players.ContainsKey(region_player.channel.owner.playerID.steamID.m_SteamID)) {
							players.Add(region_player.channel.owner.playerID.steamID.m_SteamID, region_player);
							if (on_zone_enter != null)
								on_zone_enter(region_player);
							zone_manager.trigger_on_zone_enter_global(region_player, this);
							if (zone_manager.debug_mode)
								Console.WriteLine($"zone enter ({gameObject.name}): {region_player.channel.owner.playerID.characterName}");
						}
					}
					else {
						if (players.ContainsKey(region_player.channel.owner.playerID.steamID.m_SteamID)) {
							players.Remove(region_player.channel.owner.playerID.steamID.m_SteamID);
							if (on_zone_exit != null)
								on_zone_exit(region_player);
							zone_manager.trigger_on_zone_exit_global(region_player, this);
							if (zone_manager.debug_mode)
								Console.WriteLine($"zone exit ({gameObject.name}): {region_player.channel.owner.playerID.characterName}");
						}
					}
				}
			}
			last_upd = DateTime.UtcNow;
		}
	}
}
