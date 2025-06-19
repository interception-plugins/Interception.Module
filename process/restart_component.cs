using System;
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;
using SDG.Unturned;

namespace interception.process {
    internal class restart_component : MonoBehaviour {
        string kick_reason;
        int delay;

        void restart() {
            SaveManager.save();
            for (int i = 0; i < Provider.clients.Count; i++)
                Provider.kick(Provider.clients[i].playerID.steamID, kick_reason);
            var str = Environment.CommandLine;
            str = str.Remove(0, str.IndexOf(' ') + 1);
            ProcessStartInfo psi = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, str);
            Process.Start(psi);
            Process.GetCurrentProcess().Kill();
        }

        IEnumerator<WaitForSecondsRealtime> restart_routine() {
            yield return new WaitForSecondsRealtime((float)delay);
            restart();
        }

        public void init(string kick_reason, int delay) {
            this.kick_reason = kick_reason;
            this.delay = delay;
            StartCoroutine(restart_routine());
        }
    }
}
