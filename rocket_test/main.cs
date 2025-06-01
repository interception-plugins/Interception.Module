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
using interception.zones;
using SDG.Unturned;

namespace rocket_test {
    internal class cmd_keytest : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            p.Player.ServerShowHint("mamu ebal lol", 5);
            return;
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "keytest";
        public string Syntax => "/keytest";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }

    internal class cmd_meshshit : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            RaycastHit hit;
            var cast = Physics.Raycast(p.Player.look.aim.position, p.Player.look.aim.forward, out hit, 2048f,
                RayMasks.BLOCK_NAVMESH);
            if (!cast) {
                UnturnedChat.Say(p, "raycast == false", Color.magenta);
                return;
            }
            if (hit.transform == null) {
                UnturnedChat.Say(p, "hit.transform == null", Color.magenta);
                return;
            }
            if (hit.transform.gameObject == null) {
                UnturnedChat.Say(p, "hit.transform.gameObject == null", Color.magenta);
                return;
            }
            UnturnedChat.Say(p, $"hit.transform.gameObject.tag == {hit.transform.gameObject.tag}", Color.magenta);
            if (hit.collider == null) {
                UnturnedChat.Say(p, "hit.collider == null", Color.magenta);
                return;
            }
            var collider = hit.collider as MeshCollider;
            if (collider == null) {
                UnturnedChat.Say(p, "collider == null", Color.magenta);
                return;
            }
            UnturnedChat.Say(p, $"collider.isTrigger == {collider.isTrigger}", Color.magenta);
            return;
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "meshshit";
        public string Syntax => "/meshshit";
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
                else if (args[2].ToLower() == "distance") {
                    if (args.Length < 4) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    float rad;
                    if (!float.TryParse(args[3], out rad)) {
                        UnturnedChat.Say(p, Syntax, Color.red);
                        return;
                    }
                    var distance = zone_manager.create_distance(args[1].ToLower(), p.Position, rad);
                    distance.on_zone_enter += delegate (Player _p) {
                        Console.WriteLine($"enter: {_p.channel.owner.playerID.characterName} / {distance.name}");
                    };
                    distance.on_zone_exit += delegate (Player _p) {
                        Console.WriteLine($"exit: {_p.channel.owner.playerID.characterName} / {distance.name}");
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
                    var mesh = zone_manager.create_mesh(args[1].ToLower(), p.Position, h, null); // todo?
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
            else {
                UnturnedChat.Say(p, Syntax, Color.red);
                return;
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "zone";
        public string Syntax => "/zone [create|remove] [name] [type] [radius/size/height]";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }
    
    public class config : IRocketPluginConfiguration, IDefaultable {
        public void LoadDefaults() { }
    }
    
    public class main : RocketPlugin<config> {
        protected override void Load() {
            //zone_manager.on_zone_enter_global += delegate (Player p, zone z) {
            //    Console.WriteLine($"enter: {p.channel.owner.playerID.characterName} / {z.name}");
            //};
            //zone_manager.on_zone_exit_global += delegate (Player p, zone z) {
            //    Console.WriteLine($"exit: {p.channel.owner.playerID.characterName} / {z.name}");
            //};
        }

        protected override void Unload() {

        }
    }
}
