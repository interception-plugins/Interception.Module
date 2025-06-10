using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
	public sealed class mesh_zone_component : zone_component {
		MeshCollider collider;
		GameObject child_object;

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

		internal void init(string name, Vector3 pos, float height, int? mask, bool destroy_if_failed) {
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
				throw ex;
            }

			collider = child_object.GetComponent<MeshCollider>();

			players = new Dictionary<ulong, Player>();

			on_zone_enter = zone_enter;
			on_zone_exit = zone_exit;
			Provider.onServerDisconnected += on_server_disconnected;
			if (zone_manager.debug_mode)
				enable_debug();
		}

#pragma warning disable CS0618
		public override IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
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

		public override List<Player> get_players() => players.Values.ToList();

		internal void OnTriggerEnter(Collider other) {
			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_enter != null)
				on_zone_enter(p);
			zone_manager.trigger_on_zone_enter_global(p, this);
		}

		internal void OnTriggerExit(Collider other) {
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
