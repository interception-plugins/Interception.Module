using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
    public class distance_zone_component : zone_component {
		List<Player> players;
		DateTime last_upd;
		float radius;

        public on_zone_enter_callback on_zone_enter;
		public on_zone_exit_callback on_zone_exit;

		void on_server_disconnected(CSteamID csid) {
			players.RemoveAll(x => x.channel.owner.playerID.steamID.m_SteamID == csid.m_SteamID);
			var p = PlayerTool.getPlayer(csid);
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
		}

		internal void init(string name, Vector3 pos, float radius) {
			gameObject.name = name;
			gameObject.transform.position = pos;

			players = new List<Player>();
			last_upd = DateTime.UtcNow;
			this.radius = radius;

			on_zone_enter = delegate (Player _) { };
			on_zone_exit = delegate (Player _) { };
			Provider.onServerDisconnected += on_server_disconnected;
		}

		public override List<Player> get_players() {
			return players;
		}

		void OnDestroy() {
			Provider.onServerDisconnected -= on_server_disconnected;
		}

		void Update() {
			if ((DateTime.UtcNow - last_upd).TotalMilliseconds < 500) return;
			var len = Provider.clients.Count;
			for (int i = 0; i < len; i++) {
				if ((Provider.clients[i].player.transform.position - gameObject.transform.position).sqrMagnitude <= radius * radius) {
					if (players.FindIndex(x => x.channel.owner.playerID.steamID.m_SteamID == Provider.clients[i].playerID.steamID.m_SteamID) == -1) {
						players.Add(Provider.clients[i].player);
						if (on_zone_enter != null)
							on_zone_enter(Provider.clients[i].player);
						zone_manager.trigger_on_zone_enter_global(Provider.clients[i].player, this);
					}
				}
				else {
					var c = players.RemoveAll(x => x.channel.owner.playerID.steamID.m_SteamID == Provider.clients[i].playerID.steamID.m_SteamID);
					if (c > 0) {
						if (on_zone_exit != null)
							on_zone_exit(Provider.clients[i].player);
						zone_manager.trigger_on_zone_exit_global(Provider.clients[i].player, this);
					}
				}
            }
			last_upd = DateTime.UtcNow;
		}
	}
}
