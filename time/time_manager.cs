using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using interception.cron;

namespace interception.time {
    public delegate void on_second_passed_global_callback();
    public delegate void on_minute_passed_global_callback();
    public delegate void on_hour_passed_global_callback();

    public static class time_manager {
        public static on_second_passed_global_callback on_second_passed_global;
        public static on_minute_passed_global_callback on_minute_passed_global;
        public static on_hour_passed_global_callback on_hour_passed_global;

        static void second_passed() {
            cron_manager.tick();
        }

        internal static void trigger_on_second_passed_global() {
            second_passed();
            if (on_second_passed_global != null)
                on_second_passed_global();
        }

        internal static void trigger_on_minute_passed_global() {
            if (on_minute_passed_global != null)
                on_minute_passed_global();
        }

        internal static void trigger_on_hour_passed_global() {
            if (on_hour_passed_global != null)
                on_hour_passed_global();
        }
    }
}
