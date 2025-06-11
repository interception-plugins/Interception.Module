using System;

namespace interception.utils {
    public static class webhook_util {
        public static string mention_user(ulong id) {
            return $"<@{id}>";
        }

        public static string mention_channel(ulong id) {
            return $"<#{id}>";
        }

        public static string mention_role(ulong id) {
            return $"<@&{id}>";
        }
    }
}
