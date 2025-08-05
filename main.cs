using System;
using System.Reflection;

using SDG.Framework.Modules;
using SDG.Unturned;
using UnityEngine;

using interception.cron;
using interception.zones;
using interception.input;
using interception.hooks;

namespace interception {
	internal class main : IModuleNexus {
		internal static main instance;
		internal GameObject module_game_object;

		public void initialize() {
			instance = this;
			module_game_object = new GameObject("interception_module_obj");
			UnityEngine.Object.DontDestroyOnLoad(module_game_object);
			game_events.init();
			CommandWindow.Log($"[+] Interception.Module loaded ({Assembly.GetExecutingAssembly().GetName().Version})");
		}

		public void shutdown() {
			hook_manager.unhook_all();
			cron_manager.clear_pool();
			zone_manager.clear_zones();
			input_manager.remove_component_from_online_players();
			game_events.uninit();
			UnityEngine.Object.Destroy(module_game_object);
			CommandWindow.Log($"[-] Interception.Module unloaded");
		}
	}
}
