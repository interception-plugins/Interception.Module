using System;

using UnityEngine;

namespace interception.time {
    public class time_component : MonoBehaviour {
        long last_time;

        void Start() {
            last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        void FixedUpdate() {
            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() != last_time) {
                time_manager.trigger_on_second_passed_global();
                last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }
    }
}
