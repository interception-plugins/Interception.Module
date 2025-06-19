using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using interception.cron;

namespace interception.time {
    public delegate void on_second_passed_global_callback();

    public delegate void on_curtime_minute_passed_global_callback();
    public delegate void on_curtime_hour_passed_global_callback();

    public delegate void on_realtime_minute_changed_global_callback();
    public delegate void on_realtime_hour_changed_global_callback();

    public static class time_manager {
        public static on_second_passed_global_callback on_second_passed_global;

        public static on_curtime_minute_passed_global_callback on_curtime_minute_passed_global;
        public static on_curtime_hour_passed_global_callback on_curtime_hour_passed_global;

        public static on_realtime_minute_changed_global_callback on_realtime_minute_changed_global;
        public static on_realtime_hour_changed_global_callback on_realtime_hour_changed_global;

        static void second_passed() {
            cron_manager.tick();
        }

        internal static void trigger_on_second_passed_global() {
            second_passed();
            if (on_second_passed_global != null)
                on_second_passed_global();
        }

        internal static void trigger_on_curtime_minute_passed_global() {
            if (on_curtime_minute_passed_global != null)
                on_curtime_minute_passed_global();
        }

        internal static void trigger_on_curtime_hour_passed_global() {
            if (on_curtime_hour_passed_global != null)
                on_curtime_hour_passed_global();
        }

        internal static void trigger_on_realtime_minute_changed_global() {
            if (on_realtime_minute_changed_global != null)
                on_realtime_minute_changed_global();
        }

        internal static void trigger_on_realtime_hour_changed_global() {
            if (on_realtime_hour_changed_global != null)
                on_realtime_hour_changed_global();
        }
    }
}
