using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

// todo
namespace interception.zones {
	public sealed class mesh_zone_component : zone_component {
		MeshCollider collider;
		//Collider collider;

		List<Player> players;

		public on_zone_enter_callback on_zone_enter;
		public on_zone_exit_callback on_zone_exit;

		void zone_enter(Player player) {
			players.Add(player);
		}

		void zone_exit(Player player) {
			players.RemoveAll(x => x.channel.owner.playerID.steamID.m_SteamID == player.channel.owner.playerID.steamID.m_SteamID);
		}

		void on_server_disconnected(CSteamID csid) {
			var p = PlayerTool.getPlayer(csid);
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
		}

		void grab_mesh() { // ?

        }

		internal void init(string name, Vector3 pos, float height, int? mask) {
			throw new NotImplementedException(); // todo remove after i implement this (if possible ofc)
			gameObject.name = name;
			//gameObject.transform.position = pos;
			gameObject.layer = 21;

			RaycastHit hit;
			var raycast = Physics.Raycast(pos + new Vector3(0f, height, 0f), -Vector3.up, out hit, height * 2f, 
				mask == null ? /*RayMasks.SMALL | RayMasks.MEDIUM | RayMasks.LARGE | */RayMasks.NAVMESH | RayMasks.CLIP : (int)mask);
			if (!raycast) {
				destroy();
				throw new Exception("raycast returned false");
			}
			if (hit.transform == null) {
				destroy();
				throw new Exception("cannot get transform from raycast hit");
			}
			if (hit.transform.gameObject == null) {
				destroy();
				throw new Exception("cannot get gameObject from raycast hit");
			}
			gameObject.transform.position = hit.transform.position;
			gameObject.transform.rotation = hit.transform.rotation;
			var hit_collider = hit.transform.gameObject.GetComponent<MeshCollider>();
			if (hit_collider == null)
				hit_collider = hit.transform.gameObject.GetComponentInChildren<MeshCollider>();
			if (hit_collider == null) {
				destroy();
				throw new Exception("cannot get mesh collider from raycast hit");
			}
			collider = gameObject.AddComponent<MeshCollider>();
			FieldInfo[] fields = typeof(MeshCollider).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
			var len = fields.Length;
			for (int i = 0; i < len; i++) {
				typeof(MeshCollider).GetField(fields[i].Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
					.SetValue(collider, fields[i].GetValue(hit_collider));
			}
			collider.convex = true;
			collider.isTrigger = true;

			players = new List<Player>();

			on_zone_enter = zone_enter;
			on_zone_exit = zone_exit;
			Provider.onServerDisconnected += on_server_disconnected;
			Console.WriteLine("done");
			StartCoroutine(dbg());
		}

		IEnumerator<WaitForSecondsRealtime> dbg() {
			for (; ; ) {
				yield return new WaitForSecondsRealtime(0.5f);
				EffectManager.sendEffect(147, 2048f, collider.bounds.center);
				float sqr_magniture = -1337f;
				if (Provider.clients.Count > 0)
					sqr_magniture = collider.bounds.SqrDistance(Provider.clients[0].player.transform.position);
				Console.WriteLine($"dbg\ncenter: {collider.bounds.center}\nsize: {collider.bounds.size}\nsqr_magnitude: {sqr_magniture}\nsharedMesh is null: {collider.sharedMesh == null}\n");
			}
        }

		public override List<Player> get_players() {
			return players;
		}

		void OnTriggerEnter(Collider other) {
			Console.WriteLine("enter triggered");
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_enter != null)
				on_zone_enter(p);
			zone_manager.trigger_on_zone_enter_global(p, this);
		}

		void OnTriggerExit(Collider other) {
			Console.WriteLine("exit triggered");
			if (other == null || !other.CompareTag("Player")) return;

			var p = other.gameObject.GetComponent<Player>();
			if (on_zone_exit != null)
				on_zone_exit(p);
			zone_manager.trigger_on_zone_exit_global(p, this);
		}

		void OnDestroy() {
			Provider.onServerDisconnected -= on_server_disconnected;
		}
	}
}
