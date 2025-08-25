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
using interception.time;
using interception.random;
using interception.random.generators;

namespace rocket_test {
    internal class cmd_test : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;

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
        WELL512a well512;
        mt19937 mt;
        mt19937_64 mt64;

        protected override void Load() {
            well512 = new WELL512a();
            mt = new mt19937((ulong)DateTime.Now.Ticks);
            mt64 = new mt19937_64((ulong)DateTime.Now.Ticks);
            time_manager.on_second_passed_global += delegate () {
                if (!Level.isLoaded) return;
                byte[] buf1 = new byte[32];
                byte[] buf2 = new byte[32];
                byte[] buf3 = new byte[32];
                well512.rand_bytes(buf1);
                Console.WriteLine($"well512\n" +
                    $"value: {well512.value}\n" +
                    $"rand_int: {well512.rand_int()}\n" +
                    $"rand_int(max): {well512.rand_int(1337)}\n" +
                    $"rand_int(min, max): {well512.rand_int(-1337, 1337)}\n" +
                    $"rand_double(max): {well512.rand_double(1337.0)}\n" +
                    $"rand_double(min, max): {well512.rand_double(-1337.0, 1337.0)}\n" +
                    $"rand_bytes: {string.Join("-", buf1)}\n\n");
                mt.rand_bytes(buf2);
                Console.WriteLine($"mt19937\n" +
                    $"value: {mt.value}\n" +
                    $"rand_int: {mt.rand_int()}\n" +
                    $"rand_int(max): {mt.rand_int(1337)}\n" +
                    $"rand_int(min, max): {mt.rand_int(-1337, 1337)}\n" +
                    $"rand_double(max): {mt.rand_double(1337.0)}\n" +
                    $"rand_double(min, max): {mt.rand_double(-1337.0, 1337.0)}\n" +
                    $"rand_bytes: {string.Join("-", buf2)}\n\n");
                mt64.rand_bytes(buf3);
                Console.WriteLine($"mt19937_64\n" +
                    $"value: {mt64.value}\n" +
                    $"rand_int: {mt64.rand_int()}\n" +
                    $"rand_int(max): {mt64.rand_int(1337)}\n" +
                    $"rand_int(min, max): {mt64.rand_int(-1337, 1337)}\n" +
                    $"rand_double(max): {mt64.rand_double(1337.0)}\n" +
                    $"rand_double(min, max): {mt64.rand_double(-1337.0, 1337.0)}\n" +
                    $"rand_bytes: {string.Join("-", buf3)}\n\n");
            };
        }

        protected override void Unload() {

        }
    }
}
