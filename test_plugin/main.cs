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
using interception.cron;
using interception.discord;
using interception.discord.types;
using interception.extensions;
using interception.ui;
using System.Net;
using System.IO;
using System.Globalization;
using System.Diagnostics;
using SDG.NetTransport;

namespace rocket_test {
    public class ui_test_component : UnturnedPlayerComponent {
        ITransportConnection tc;
        int i = 0;
        window canvas;
        tab main_tab;
        text test_text;
        image test_bg;
        button test_button0;
        button test_button1;
        button test_button2;
        text_progressbar text_progress_bar_test;
        button test_button3;
        button test_button4;
        image_progressbar image_progress_bar_test;
        tab test_tab;
        button test_button5;
        textbox test_input_field;
        text test_input_field_dup_text;
        button test_button6;
        button test_button7;
        tab scroll_test;
        button_collection scroll_button_test;

        protected override void Load() {
            tc = base.Player.Player.channel.owner.transportConnection;
            canvas = new window(31915, 1337, tc);
            canvas.on_spawned += delegate () {
                Console.WriteLine("window spawned");
            };
            canvas.on_despawned += delegate () {
                Console.WriteLine("window despawned");
            };
            main_tab = canvas.add_tab("main");
            test_text = main_tab.add_text("test_text");
            test_bg = main_tab.add_image("test_bg");
            test_button0 = new button(test_bg, test_bg.key, tc, "test_button0");
            test_button0.on_click += delegate () {
                switch (i) {
                    case 0:
                        test_bg.set_image("https://media.discordapp.net/attachments/1297303869088600166/1382287218504110163/mBBSxlWeSDU.jpg?ex=684d3dca&is=684bec4a&hm=5531bba36a1987d0b4771f0cc66a803aa7a7cd7eca8b35b00ef042e93e26f784&=&format=webp&width=795&height=801");
                        i++;
                        break;
                    case 1:
                        test_bg.set_image("https://sun9-1.userapi.com/impg/-lFuy_XZsBKTFCYfKS21RC9t3h48w6R7fv--ZQ/hyvsHQwlKB4.jpg?size=1430x1079&quality=95&sign=be61d839faa5b43dd10d161321c4d379&type=album");
                        i++;
                        break;
                    case 2:
                        test_bg.set_image("https://media.discordapp.net/attachments/1297303869088600166/1382287219645091840/1FffDJjjK54.jpg?ex=684d3dca&is=684bec4a&hm=12760389a16b018fefc767742f08bcc9173474e5bda674c56164360e5df54739&=&format=webp&width=362&height=327");
                        i++;
                        break;
                    case 3:
                        test_bg.set_image("https://media.discordapp.net/attachments/1297303869088600166/1382287220261519391/t7tMSk-ucGQ.jpg?ex=684d3dca&is=684bec4a&hm=740ce155b8af9fc54d495f7d44a06894d3f22b3b649cdbd854dc6b54a4f6b188&=&format=webp&width=1053&height=932");
                        i++;
                        break;
                    default:
                        i = 0;
                        break;
                }
            };
            test_button1 = main_tab.add_button("test_button1");
            test_button1.on_click += delegate () {
                text_progress_bar_test.increment();
            };
            test_button2 = main_tab.add_button("test_button2");
            test_button2.on_click += delegate () {
                text_progress_bar_test.decrement();
            };
            text_progress_bar_test = main_tab.add_text_progressbar("Image/Image/text_progress_bar_test", 31, 'w');
            test_button3 = main_tab.add_button("test_button3");
            test_button3.on_click += delegate () {
                image_progress_bar_test.increment();
            };
            test_button4 = main_tab.add_button("test_button4");
            test_button4.on_click += delegate () {
                image_progress_bar_test.decrement();
            };
            image_progress_bar_test = main_tab.add_image_progressbar("image_progress_bar_test", 19, "image_progress_bar_{0}");
            test_tab = main_tab.add_tab("test_tab");
            test_button5 = test_tab.add_button("test_button5");
            test_button5.on_click += delegate () {
                test_input_field.set_text(string.Empty);
            };
            test_input_field = test_tab.add_textbox("test_input_field");
            test_input_field.on_text_changed += delegate (string oldval, string newval) {
                test_input_field_dup_text.set_text(newval);
            };
            test_input_field_dup_text = test_tab.add_text("test_input_field_dup_text");
            test_button6 = main_tab.add_button("test_button6");
            test_button6.on_hidden += delegate () {
                Console.WriteLine("test_button6 is hidden now");
            };
            test_button6.on_click += delegate () {
                test_button6.hide();
            };
            test_button7 = main_tab.add_button("test_button7");
            test_button7.on_click += delegate () {
                canvas.despawn();
                ui_util.disable_cursor(base.Player.Player);
                ui_util.disable_blur(base.Player.Player);
            };
            scroll_test = main_tab.add_tab("Scroll View/Viewport/Content");
            scroll_button_test = scroll_test.add_button_collection("scroll_button_test", 50, "_{0}");
            scroll_button_test.on_click_any += delegate (int index) {
                scroll_button_test.hide(index);
            };
            scroll_button_test.on_hidden_any += delegate (int index) {
                Console.WriteLine($"button was hiiden in collection: {index}");
            };
        }

        public void enable_ui() {
            canvas.spawn();
            //ui_util.enable_cursor(base.Player.Player);
            ui_util.enable_blur(base.Player.Player);
            test_text.set_text($"this text was changed by a plugin: {System.Reflection.Assembly.GetExecutingAssembly().FullName}");
        }
    }
    
    internal class cmd_test : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (args.Length < 1) {
                p.Player.say_to(Syntax, Color.red);
                return;
            }
            chat_util.simulate_message(p.Player, string.Join(" ", args), EChatMode.GLOBAL);
            p.GetComponent<ui_test_component>().enable_ui();
            Console.WriteLine(ui_manager.get_pool_count());
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "test";
        public string Syntax => "/test [message]";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }

    internal class cmd_restart : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            SaveManager.save();
            for (int i = 0; i < Provider.clients.Count; i++)
                Provider.kick(Provider.clients[i].get_csteamid(), "server restart");
            var str = System.Environment.CommandLine;
            str = str.Remove(0, str.IndexOf(' ') + 1);
            ProcessStartInfo psi = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, str);
            Process.Start(psi);
            Process.GetCurrentProcess().Kill();
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "restart";
        public string Syntax => "/restart";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }

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
                    var sphere = zone_manager.create_sphere_zone(args[1].ToLower(), p.Position, rad);
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
                    var box = zone_manager.create_box_zone(args[1].ToLower(), p.Position, p.Player.transform.forward, new Vector3(x, y, z));
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
                    var distance_slow = zone_manager.create_distance_slow_zone(args[1].ToLower(), p.Position, rad);
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
                    var distance_fast = zone_manager.create_distance_fast_zone(args[1].ToLower(), p.Position, rad);
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
                    var mesh = zone_manager.create_mesh_zone(args[1].ToLower(), p.Position, h, null);
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
        public e_keycode key;
        public xml_dictionary<s_vector3, s_quaternion> dict;

        public void LoadDefaults() {
            key = e_keycode.shift0;
            dict = new Dictionary<s_vector3, s_quaternion>() {
                { new s_vector3(1337f, 0f, 0f), new s_quaternion() }
            };
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
            IntPtr user32;
            int result = native.make_delegate<MessageBox>("user32.dll", "MessageBoxA", out user32)(IntPtr.Zero, "lol", "OwO", 0);
            Console.WriteLine($"messageboxa result = {result}");
            var free = native.free_library(user32);
            Console.WriteLine($"freelibrary result = {free}");
            cron_manager.register_event(new cron_event("test1", DateTime.Parse("12:32:00"), false, delegate(object[] args) { Console.WriteLine(cron_manager.is_event_exist("test3")); }));
            cron_manager.register_event(new cron_event("test2", DateTime.Parse("12:31:00"), false, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test2"));
            cron_manager.register_event(new cron_event("test3", DateTime.Parse("12:31:00"), true, delegate (object[] args) { Console.WriteLine((string)args[0]); }, "test3"));
            cron_manager.register_event(new cron_event("test4", DateTime.Parse("12:32:30"), false, delegate (object[] args) { Console.WriteLine(cron_manager.is_event_exist("test4")); }));
            webhook wh = new webhook(null, null, null, e_webhook_flag.suppress_notifications);
            var embed = new embed();
            embed.add_color(Color.blue);
            embed.add_title("embed");
            embed.add_description("OwO");
            embed.add_footer(new embed_footer("footer", null));
            embed.add_timestamp(DateTime.UtcNow.AddHours(-2));
            wh.add_embed(embed);
            ui_manager.on_button_click_global += delegate (button b) {
                Console.WriteLine($"button clicked: {b.path}");
            };
            ui_manager.on_textbox_text_changed_global += delegate (string oldval, string newval, textbox tb) {
                Console.WriteLine($"textbox text changed: {tb.path} (was = {oldval} / now = {newval})");
            };
            ui_manager.on_progressbar_progress_changed_global += delegate (int oldval, int newval, progressbar pb) {
                Console.WriteLine($"progress bar value changed: {pb.path} (was = {oldval} / now = {newval})");
            };
            ui_manager.on_button_collection_click_global += delegate (int i, button_collection bc) {
                Console.WriteLine($"button was clicked in collection: {bc.get_path_of(i)}");
            };
            ui_manager.on_control_hidden_global += delegate (control c) {
                Console.WriteLine($"control hidden: {c.path}");
            };
        }

        protected override void Unload() {

        }
    }
}
