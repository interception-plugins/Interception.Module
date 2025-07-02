using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;
using interception.utils;
using interception.cron;
using interception.extensions;

namespace rocket_test {
    internal class cmd_test : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (args.Length < 1) {
                p.Player.say_to(Syntax, Color.red);
                return;
            }
            chat_util.simulate_message(p.Player, string.Join(" ", args), EChatMode.GLOBAL);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "test";
        public string Syntax => "/test [message]";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }

    public class config : IRocketPluginConfiguration, IDefaultable {
        public void LoadDefaults() {

        }
    }
    
    public class main : RocketPlugin<config> {
        protected override void Load() {
            cron_manager.register_event(new cron_event("test1", DateTime.Parse("12:32:00"), false, delegate(object[] args) { Console.WriteLine(cron_manager.is_event_exist("test3")); }));
            cron_manager.register_event(new cron_event("test2", DateTime.Parse("12:31:00"), false, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test2"));
            cron_manager.register_event(new cron_event("test3", DateTime.Parse("12:31:00"), true, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test3"));
            cron_manager.register_event(new cron_event("test4", DateTime.Parse("12:32:30"), false, delegate (object[] args) { Console.WriteLine(cron_manager.is_event_exist("test4")); }));
        }

        protected override void Unload() {

        }
    }
}
