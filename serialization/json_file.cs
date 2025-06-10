using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace interception.serialization {
    public class json_file {
        string path;

        public json_file(string path) {
            this.path = path;
        }

        public void save<T>(T type) {
            File.WriteAllText(path, JsonConvert.SerializeObject(type, Formatting.Indented));
        }

        public void save<T>(T type, Formatting formatting) {
            File.WriteAllText(path, JsonConvert.SerializeObject(type, formatting));
        }

        public void load<T>(ref T type) {
            if (!File.Exists(path)) {
                type = (T)Activator.CreateInstance(typeof(T));
                save(type);
                return;
            }
            type = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
