using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using interception.cron;

namespace interception.time {
    public delegate void on_second_passed_global_callback();

    public static class time_manager {
        public static on_second_passed_global_callback on_second_passed_global = delegate () { };

        static void second_passed() {
            cron_manager.tick();
        }

        internal static void trigger_on_second_passed_global() {
            second_passed();
            if (on_second_passed_global != null)
                on_second_passed_global();
        }
    }
}
