using System;

namespace interception.utils {
    public static class os_util {
        public static bool is_unix() {
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }

        public static bool is_64bit() {
            return IntPtr.Size == 8;
        }
    }
}
