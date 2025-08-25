using System;

namespace interception.extensions {
    public static class DateTime_ex {
        public static TimeSpan time_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2);
        }

        public static double days_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2).TotalDays;
        }

        public static double hours_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2).TotalHours;
        }

        public static double minutes_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2).TotalMinutes;
        }

        public static double seconds_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2).TotalSeconds;
        }

        public static double milliseconds_since(this DateTime dt1, DateTime dt2) {
            return (dt1 - dt2).TotalMilliseconds;
        }
    }
}
