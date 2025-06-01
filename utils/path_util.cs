using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using interception.ui;

namespace interception.utils {
    public static class path_util {
        public static string build_path(params string[] arr) {
            List<string> l = new List<string>();
            var len = arr.Length;
            for (int i = 0; i < len; i++)
                if (!string.IsNullOrEmpty(arr[i]) && !string.IsNullOrWhiteSpace(arr[i]))
                    l.Add(arr[i]);
            return string.Join("/", l.ToArray());
        }

        public static string build_path(control head) {
            List<string> l = new List<string>();
            int i = 0;
            while (head != null) {
                i++;
                if (!string.IsNullOrEmpty(head.name) && !string.IsNullOrWhiteSpace(head.name))
                    l.Insert(0, head.name);
                head = head.parent;
            }
            return string.Join("/", l.ToArray());
        }
    }
}
