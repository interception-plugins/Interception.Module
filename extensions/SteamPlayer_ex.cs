using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Steamworks;
using SDG.Unturned;

namespace interception.extensions {
    public static class SteamPlayer_ex {
        public static CSteamID get_csteamid(this SteamPlayer p) {
            return p.playerID.steamID;
        }

        public static ulong get_steamid64(this SteamPlayer p) {
            return p.playerID.steamID.m_SteamID;
        }

        public static string get_character_name(this SteamPlayer p) {
            return p.playerID.characterName;
        }

        public static string get_steam_name(this SteamPlayer p) {
            return p.playerID.playerName;
        }
    }
}
