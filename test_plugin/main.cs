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
using interception.hooks;
using Rocket.Unturned.Chat;

namespace rocket_test {
    internal class cmd_test : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            //if (args.Length < 1) {
            //    p.Player.say_to(Syntax, Color.red);
            //    return;
            //}
            //chat_util.simulate_message(p.Player, string.Join(" ", args), EChatMode.GLOBAL);
            UnturnedChat.Say(p, "not hooked", Color.red);
            hook_manager.disable_hook<main.Say_def>(main.Say_hk);
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

    public class PlayerLife_hk : SDG.Unturned.PlayerLife {
        delegate void askTire_def(byte amount);
        delegate void askRest_def(byte amount);

        void askTire_hk(byte amount) {
            hook_manager.call_original(base.player.life, amount);
            UnturnedChat.Say($"{base.player.channel.owner.get_character_name()} / {amount}");
        }

        void askRest_hk(byte amount) {
            hook_manager.call_original(base.player.life, amount);
            UnturnedChat.Say(UnturnedPlayer.FromPlayer(base.player), $"{base.player.channel.owner.get_character_name()} / {amount}", Color.yellow);
        }

        public void init() {
            hook_manager.create_hook<askTire_def>(base.askTire, askTire_hk);
            hook_manager.create_hook<askRest_def>(typeof(SDG.Unturned.PlayerLife), "askRest", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, askRest_hk, 0);
        }

        public void enable() {
            hook_manager.enable_hook<askTire_def>(askTire_hk);
            hook_manager.enable_hook<askRest_def>(askRest_hk);

        }

        public void disable() {
            hook_manager.disable_hook<askTire_def>(askTire_hk);
            hook_manager.disable_hook<askRest_def>(askRest_hk);
        }
    }
    
    public class main : RocketPlugin<config> {
        internal delegate void Say_def(IRocketPlayer player, string message, Color color);

        internal static void Say_hk(IRocketPlayer player, string message, Color color) {
            hook_manager.call_original(null, player, "hooked", Color.magenta);
        }

        protected override void Load() {
            cron_manager.register_event(new cron_event("test1", DateTime.Parse("12:32:00"), false, delegate(object[] args) { Console.WriteLine(cron_manager.is_event_exist("test3")); }));
            cron_manager.register_event(new cron_event("test2", DateTime.Parse("12:31:00"), false, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test2"));
            cron_manager.register_event(new cron_event("test3", DateTime.Parse("12:31:00"), true, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test3"));
            cron_manager.register_event(new cron_event("test4", DateTime.Parse("12:32:30"), false, delegate (object[] args) { Console.WriteLine(cron_manager.is_event_exist("test4")); }));
            hook_manager.create_hook<Say_def>(UnturnedChat.Say, Say_hk);
            hook_manager.enable_hook<Say_def>(Say_hk);
            PlayerLife_hk hk = new PlayerLife_hk();
            hk.init();
            hk.enable();
            var types = typeof(SDG.Unturned.Provider).Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
                if (types[i].FullName.Contains("NetMessages") && types[i].FullName == "SDG.Unturned.NetMessages") 
                    Console.WriteLine(types[i].FullName);
        }

        protected override void Unload() {

        }
    }
}
