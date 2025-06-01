using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Steamworks;
using SDG.Unturned;

namespace interception.extension {
    public static class Player_ex {
        public static CSteamID get_csteamid(this Player p) {
            return p.channel.owner.playerID.steamID;
        }

        public static ulong get_steamid64(this Player p) {
            return p.channel.owner.playerID.steamID.m_SteamID;
        }
    }
}
