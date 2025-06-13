using System;

using SDG.Unturned;

namespace interception.utils {
    public static class ui_util {
        /*
        public static string make_path(params string[] arr) {
            List<string> l = new List<string>();
            var len = arr.Length;
            for (int i = 0; i < len; i++)
                if (!string.IsNullOrEmpty(arr[i]) && !string.IsNullOrWhiteSpace(arr[i]))
                    l.Add(arr[i]);
            return string.Join("/", l.ToArray());
        }
        */

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
