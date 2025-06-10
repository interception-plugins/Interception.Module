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
	internal class mesh_zone_helper_component : MonoBehaviour {
		MeshCollider collider;
		mesh_zone_component parent_component;

		internal void init() {
			parent_component = gameObject.transform.parent.gameObject.GetComponent<mesh_zone_component>();
			//gameObject.name = name;
			gameObject.layer = 21;
			collider = gameObject.GetComponent<MeshCollider>();
			if (collider == null)
				collider = gameObject.GetComponentInChildren<MeshCollider>();
			if (collider == null)
				throw new Exception("cannot get any mesh collider component"); 
			
			collider.convex = true;
			collider.isTrigger = true;
		}

		void OnTriggerEnter(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;
			parent_component.OnTriggerEnter(other);
		}

		void OnTriggerExit(Collider other) {
			if (other == null || !other.CompareTag("Player")) return;
			parent_component.OnTriggerExit(other);
		}
	}
}
