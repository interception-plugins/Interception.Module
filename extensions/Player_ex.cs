using System;

using UnityEngine;
using Steamworks;
using SDG.Unturned;

namespace interception.extensions {
    public static class Player_ex {
        public static CSteamID get_csteamid(this Player p) {
            return p.channel.owner.playerID.steamID;
        }

        public static ulong get_steamid64(this Player p) {
            return p.channel.owner.playerID.steamID.m_SteamID;
        }

        public static string get_character_name(this Player p) {
            return p.channel.owner.playerID.characterName;
        }

        public static string get_steam_name(this Player p) {
            return p.channel.owner.playerID.playerName;
        }

        public static void mute_voice(this Player p) {
            p.voice.ServerSetPermissions(p.voice.GetAllowTalkingWhileDead(), false);
        }

        public static void unmute_voice(this Player p) {
            p.voice.ServerSetPermissions(p.voice.GetAllowTalkingWhileDead(), true);
        }

        public static void say_to(this Player p, string text, Color c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(this Player p, string text, Color32 c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(this Player p, string text, string color, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Palette.hex(color), null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(this Player p, string text, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Color.green, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }
    }
}
