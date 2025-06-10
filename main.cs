using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using SDG.Framework.Modules;
using SDG.Unturned;
using UnityEngine;

namespace interception {
	internal class main : IModuleNexus {
		//public GameObject obj;

		public void initialize() {
			//obj = new GameObject("Interception");
			//UnityEngine.Object.DontDestroyOnLoad(obj);
			//Provider.onServerHosted += delegate () {
			//	obj.AddComponent<main>();
			//};
			game_events.init();
			CommandWindow.Log($"[+] Interception.Module loaded ({Assembly.GetExecutingAssembly().GetName().Version})");
			
		}

		public void shutdown() {
			game_events.uninit();
			CommandWindow.Log($"[-] Interception.Module unloaded (no)");
		}
	}
}
