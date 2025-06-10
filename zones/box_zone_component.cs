using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
	public sealed class box_zone_component : zone_component {
		BoxCollider collider;

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

		internal void init(string name, Vector3 pos, Vector3 size) {
			gameObject.name = name;
			gameObject.transform.position = pos;
			gameObject.layer = 21;
			collider = gameObject.AddComponent<BoxCollider>();
			collider.isTrigger = true;
			collider.size = size;

			players = new Dictionary<ulong, Player>();

			on_zone_enter = zone_enter;
			on_zone_exit = zone_exit;
			Provider.onServerDisconnected += on_server_disconnected;
			if (zone_manager.debug_mode)
				enable_debug();
		}

		public override List<Player> get_players() => players.Values.ToList();

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
