using System;

using SDG.Unturned;

namespace interception.utils {
    public static class ui_util {
        public static void enable_cursor(Player p) {
            p.setPluginWidgetFlag(EPluginWidgetFlags.Modal | EPluginWidgetFlags.NoBlur, true);
        }

        public static void disable_cursor(Player p) {
            p.setPluginWidgetFlag(EPluginWidgetFlags.Modal | EPluginWidgetFlags.NoBlur, false);
        }

        public static void enable_blur(Player p) {
            p.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
        }

        public static void disable_blur(Player p) {
            p.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
        }
    }
}
