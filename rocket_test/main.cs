using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using Rocket.Unturned;
using interception.zones;
using interception.enums;
using interception.input;
using interception.utils;
using interception.serialization;
using interception.serialization.types;
using interception.notsafe;

namespace rocket_test {
    internal class cmd_zone : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (args.Length < 1) {
                UnturnedChat.Say(p, Syntax, Color.red);
                return;
            }
            if (args[0].ToLower() == "create") {
                if (args.Length < 3) {
                    UnturnedChat.Say(p, Syntax, Color.red);
                    return;
                }
                if (args[2].ToLower() == "sphere") {
                    if (args.Length < 4) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float rad;
                    if (!float.TryParse(args[3], out rad)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    //zone_manager.create_sphere(args[1].ToLower(), p.Position, rad);
                    var sphere = zone_manager.create_sphere(args[1].ToLower(), p.Position, rad);
                    sphere.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {sphere.name}");
                    };
                    sphere.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {sphere.name}");
                    };
                }
                else if (args[2].ToLower() == "box") {
                    if (args.Length < 6) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float x, y, z;
                    if (!float.TryParse(args[3], out x) || !float.TryParse(args[4], out y) || !float.TryParse(args[5], out z)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    //zone_manager.create_box(args[1].ToLower(), p.Position, new Vector3(x, y, z));
                    var box = zone_manager.create_box(args[1].ToLower(), p.Position, new Vector3(x, y, z));
                    box.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {box.name}");
                    };
                    box.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {box.name}");
                    };
                }
                else if (args[2].ToLower() == "distance_slow") {
                    if (args.Length < 4) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float rad;
                    if (!float.TryParse(args[3], out rad)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    var distance_slow = zone_manager.create_distance_slow(args[1].ToLower(), p.Position, rad);
                    distance_slow.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {distance_slow.name}");
                    };
                    distance_slow.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {distance_slow.name}");
                    };
                }
                else if (args[2].ToLower() == "distance_fast") {
                    if (args.Length < 4) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float rad;
                    if (!float.TryParse(args[3], out rad)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    var distance_fast = zone_manager.create_distance_fast(args[1].ToLower(), p.Position, rad);
                    distance_fast.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {distance_fast.name}");
                    };
                    distance_fast.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {distance_fast.name}");
                    };
                }
                else if (args[2].ToLower() == "mesh") {
                    if (args.Length < 4) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float h;
                    if (!float.TryParse(args[3], out h)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    var mesh = zone_manager.create_mesh(args[1].ToLower(), p.Position, h, null);
                    mesh.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {mesh.name}");
                    };
                    mesh.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {mesh.name}");
                    };
                }
                else {
                    UnturnedChat.Say(p, Syntax, Color.red);
                    return;
                }
            }
            else if (args[0].ToLower() == "remove") {
                if (args.Length < 2) {
                    UnturnedChat.Say(p, Syntax, Color.red);
                    return;
                }
                zone_manager.remove_zone(args[1].ToLower());
            }
            else if (args[0].ToLower() == "debug") {
                zone_manager.debug_all_zones();
            }
            else {
                UnturnedChat.Say(p, Syntax, Color.red);
                return;
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "zone";
        public string Syntax => "/zone [create|remove|debug] [name] [type] [radius/size/height]";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }
    
    public class input_test_component : UnturnedPlayerComponent {
        player_input_component input;
        
        void down(e_keycode key) {
            //Console.WriteLine($"key down: {base.Player.CharacterName} / {key.ToString()}");
        }

        void up(e_keycode key) {
            //Console.WriteLine($"key up: {base.Player.CharacterName} / {key.ToString()}");
        }

        protected override void Load() {
            input = base.Player.Player.gameObject.GetComponent<player_input_component>();
            input.on_key_down += down;
            input.on_key_up += up;
        }

        protected override void Unload() {
            input.on_key_down -= down;
            input.on_key_up -= up;
            input = null;
        }
    }

    public class db_type {
        public s_vector3 vec3 { get; set; }
        public s_quaternion quat { get; set; }
    }

    public class config : IRocketPluginConfiguration, IDefaultable {
        public s_vector3 vec3;
        public s_quaternion quat;

        public void LoadDefaults() {
            vec3 = new Vector3(0f, 1337f, 0f);
            quat = Quaternion.identity;
        }
    }
    
    public class main : RocketPlugin<config> {
        json_file db_file;
        db_type db = new db_type();
        delegate int MessageBox(IntPtr handle, string c, string t, uint lol);

        protected override void Load() {
            zone_manager.on_zone_enter_global += delegate (Player p, zone_component z) {
                Console.WriteLine($"(global) enter: {p.channel.owner.playerID.characterName} / {z.name}");
            };
            zone_manager.on_zone_exit_global += delegate (Player p, zone_component z) {
                Console.WriteLine($"(global) exit: {p.channel.owner.playerID.characterName} / {z.name}");
            };
            db_file = new json_file(path_util.make_rocket_plugin_file_path("database_test.json"));
            db_file.load<db_type>(ref db);
            SaveManager.onPostSave += delegate () {
                db.vec3 = new Vector3(111f, 0f, 0f);
                db.quat = new Quaternion(0f, 0f, 0f, 111f);
                db_file.save<db_type>(db); 
            };
            input_manager.on_key_down_global += delegate (Player p, e_keycode key) {
                //Console.WriteLine($"(global) key down: {p.channel.owner.playerID.characterName} / {key.ToString()}");
            };
            input_manager.on_key_up_global += delegate (Player p, e_keycode key) {
                //Console.WriteLine($"(global) key up: {p.channel.owner.playerID.characterName} / {key.ToString()}");
            };
            var user32 = native.load_library("user32.dll");
            Console.WriteLine($"user32 addr = {user32}");
            var messageboxa = native.get_proc_addr(user32, "MessageBoxA");
            Console.WriteLine($"messageboxa addr = {messageboxa}");
            int result = native.create_func<MessageBox>(messageboxa)(IntPtr.Zero, "lol", "OwO", 0);
            Console.WriteLine($"messageboxa result = {result}");
            var free = native.free_library(user32);
            Console.WriteLine($"freelibrary result = {free}");
        }

        protected override void Unload() {

        }
    }
}
