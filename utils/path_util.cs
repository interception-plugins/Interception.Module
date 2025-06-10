using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace interception.utils {
    public static class path_util {
        public static string get_rocket_plugin_directory() {
            return Path.Combine(Directory.GetCurrentDirectory(), $"Plugins/{System.Reflection.Assembly.GetCallingAssembly().GetName().Name}");
        }

        public static string make_rocket_plugin_file_path(string filename) {
            return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), $"Plugins/{System.Reflection.Assembly.GetCallingAssembly().GetName().Name}"), filename);
        }
    }
}
