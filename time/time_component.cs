using System;

using UnityEngine;

namespace interception.time {
    public class time_component : MonoBehaviour {
        long last_time;
        byte seconds_passed1;
        short seconds_passed2;
        int last_realtime_minute;
        int last_realtime_hour;

        void Start() {
            last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            seconds_passed1 = 0;
            seconds_passed2 = 0;
            last_realtime_minute = DateTime.UtcNow.Minute;
            last_realtime_hour = DateTime.UtcNow.Hour;
        }

        void FixedUpdate() {
            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() != last_time) {
                time_manager.trigger_on_second_passed_global();
                seconds_passed1++;
                if (seconds_passed1 >= 60) {
                    time_manager.trigger_on_curtime_minute_passed_global();
                    seconds_passed1 = 0;
                }
                seconds_passed2++;
                if (seconds_passed2 >= 3600) {
                    time_manager.trigger_on_curtime_hour_passed_global();
                    seconds_passed2 = 0;
                }
                if (last_realtime_minute != DateTime.UtcNow.Minute) {
                    time_manager.trigger_on_realtime_minute_changed_global();
                    last_realtime_minute = DateTime.UtcNow.Minute;
                }
                if (last_realtime_hour != DateTime.UtcNow.Hour) {
                    time_manager.trigger_on_realtime_hour_changed_global();
                    last_realtime_hour = DateTime.UtcNow.Hour;
                }
                last_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
        }
    }
}
