using System;

using UnityEngine;

namespace interception.time {
    public class time_component : MonoBehaviour {
        long last_time;
        byte seconds_passed1;
        short seconds_passed2;

        void Start() {
            last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            seconds_passed1 = 0;
            seconds_passed2 = 0;
        }

        void FixedUpdate() {
            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() != last_time) {
                time_manager.trigger_on_second_passed_global();
                seconds_passed1++;
                if (seconds_passed1 >= 60) {
                    time_manager.trigger_on_minute_passed_global();
                    seconds_passed1 = 0;
                }
                seconds_passed2++;
                if (seconds_passed2 >= 3600) {
                    time_manager.trigger_on_hour_passed_global();
                    seconds_passed2 = 0;
                }
                last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }
    }
}
