using System;

namespace interception.server {
    public delegate void on_restart_performed_global_callback(int delay);

    public static class restart_manager {
        public static on_restart_performed_global_callback on_restart_performed_global;

        public static void restart(string kick_reason, int delay = 0) {
            main.instance.module_game_object.AddComponent<restart_component>().init(kick_reason, delay);
            if (on_restart_performed_global != null)
                on_restart_performed_global(delay);
        }
    }
}
