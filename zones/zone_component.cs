using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace interception.zones {
    public delegate void on_zone_enter_callback(Player player);
    public delegate void on_zone_exit_callback(Player player);

    public class zone_component : MonoBehaviour {
        protected Coroutine debug_routine;

        //public virtual void destroy() {
        //    Destroy(gameObject);
        //}

        public on_zone_enter_callback on_zone_enter;
        public on_zone_exit_callback on_zone_exit;

#pragma warning disable CS0618
        protected virtual IEnumerator<WaitForSecondsRealtime> debug_routine_worker() {
            for (; ; ) {
                EffectManager.sendEffect(133, 512f, gameObject.transform.position);
                yield return new WaitForSecondsRealtime(1f);
            }
        }
#pragma warning restore CS0618

        internal void enable_debug() {
            if (debug_routine != null) return;
            debug_routine = StartCoroutine(debug_routine_worker());
        }

        internal void disable_debug() {
            if (debug_routine == null) return;
            StopCoroutine(debug_routine);
            debug_routine = null;
        }

        protected readonly Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();
        public virtual List<Player> get_players() => players.Values.ToList();

        void on_server_disconnected(CSteamID csid) {
            var p = PlayerTool.getPlayer(csid);
            if (p == null || !players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
            players.Remove(p.channel.owner.playerID.steamID.m_SteamID);
            if (on_zone_exit != null)
                on_zone_exit(p);
            zone_manager.trigger_on_zone_exit_global(p, this);
            if (zone_manager.debug_mode)
                Console.WriteLine($"zone exit ({gameObject.name}): {p.channel.owner.playerID.characterName}");
        }

        void on_player_died(PlayerLife pl, EDeathCause cause, ELimb limb, CSteamID instigator) {
            if (!players.ContainsKey(pl.player.channel.owner.playerID.steamID.m_SteamID)) return;
            players.Remove(pl.player.channel.owner.playerID.steamID.m_SteamID);
            if (on_zone_exit != null)
                on_zone_exit(pl.player);
            zone_manager.trigger_on_zone_exit_global(pl.player, this);
            if (zone_manager.debug_mode)
                Console.WriteLine($"zone exit ({gameObject.name}): {pl.player.channel.owner.playerID.characterName}");

        }

        public void init() {
            Provider.onServerDisconnected += on_server_disconnected;
            PlayerLife.onPlayerDied += on_player_died;
            if (zone_manager.debug_mode)
                enable_debug();
        }

        void OnDestroy() {
            PlayerLife.onPlayerDied -= on_player_died;
            Provider.onServerDisconnected -= on_server_disconnected;
            on_zone_enter = null;
            on_zone_exit = null;
        }

        internal void OnTriggerEnter(Collider other) {
            if (other == null || !other.CompareTag("Player")) return;

            var p = other.gameObject.GetComponent<Player>();
            if (players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
            players.Add(p.channel.owner.playerID.steamID.m_SteamID, p);

            if (on_zone_enter != null)
                on_zone_enter(p);
            zone_manager.trigger_on_zone_enter_global(p, this);

            if (zone_manager.debug_mode)
                CommandWindow.Log($"[+] zone enter ({gameObject.name}): {p.channel.owner.playerID.characterName}");
        }

        internal void OnTriggerExit(Collider other) {
            if (other == null || !other.CompareTag("Player")) return;

            var p = other.gameObject.GetComponent<Player>();
            if (!players.ContainsKey(p.channel.owner.playerID.steamID.m_SteamID)) return;
            players.Remove(p.channel.owner.playerID.steamID.m_SteamID);

            if (on_zone_exit != null)
                on_zone_exit(p);
            zone_manager.trigger_on_zone_exit_global(p, this);

            if (zone_manager.debug_mode)
                CommandWindow.Log($"[-] zone exit ({gameObject.name}): {p.channel.owner.playerID.characterName}");
        }
    }
}
