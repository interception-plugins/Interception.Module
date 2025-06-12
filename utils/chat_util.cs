using System;

using UnityEngine;
using SDG.Unturned;

namespace interception.utils {
    public static class chat_util {
        public static void say_to(Player p, string text, Color c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(Player p, string text, Color32 c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(Player p, string text, string color, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Palette.hex(color), null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_to(Player p, string text, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Color.green, null, p.channel.owner, EChatMode.SAY, icon, rich_text);
        }

        public static void say_as(Player p, string text, Color c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, p.channel.owner, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say_as(Player p, string text, Color32 c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, p.channel.owner, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say_as(Player p, string text, string color, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Palette.hex(color), p.channel.owner, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say_as(Player p, string text, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Color.green, p.channel.owner, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say(string text, Color c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say(string text, Color32 c, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, c, null, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say(string text, string color, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Palette.hex(color), null, null, EChatMode.SAY, icon, rich_text);
        }

        public static void say(string text, string icon = null, bool rich_text = true) {
            ChatManager.serverSendMessage(text, Color.green, null, null, EChatMode.SAY, icon, rich_text);
        }

        static string make_message_text(string text, EChatMode mode) {
            text = "%SPEAKER%: " + text;
            if (mode != EChatMode.LOCAL) {
                if (mode == EChatMode.GROUP)
                    text = "[G] " + text;
            }
            else {
                text = "[A] " + text;
            }
            return text;
        }

        static Color make_message_color(Player p) {
            Color c = Color.white;
            if (p.channel.owner.isAdmin && !Provider.hideAdmins)
                c = Palette.ADMIN;
            else if (p.channel.owner.isPro)
                c = Palette.PRO;
            return c;
        }

        public static void simulate_message(Player p, string text, Color c, EChatMode mode) {
            ChatManager.serverSendMessage(make_message_text(text, mode), c, p.channel.owner, null, mode, null, false);
        }

        public static void simulate_message(Player p, Player to, string text, Color c, EChatMode mode) {
            ChatManager.serverSendMessage(make_message_text(text, mode), c, p.channel.owner, to.channel.owner, mode, null, false);
        }

        public static void simulate_message(Player p, string text, EChatMode mode) {
            ChatManager.serverSendMessage(make_message_text(text, mode), make_message_color(p), p.channel.owner, null, mode, null, false);
        }

        public static void simulate_message(Player p, Player to, string text, EChatMode mode) {
            ChatManager.serverSendMessage(make_message_text(text, mode), make_message_color(p), p.channel.owner, to.channel.owner, mode, null, false);
        }
    }
}
