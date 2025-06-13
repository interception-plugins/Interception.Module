#warning do not compile

using System;
using System.Collections.Generic;

using interception.ui;

namespace interception.utils {
    public static class ui_util {
        public static string make_path(params string[] arr) {
            List<string> l = new List<string>();
            var len = arr.Length;
            for (int i = 0; i < len; i++)
                if (!string.IsNullOrEmpty(arr[i]) && !string.IsNullOrWhiteSpace(arr[i]))
                    l.Add(arr[i]);
            return string.Join("/", l.ToArray());
        }
    }
}
