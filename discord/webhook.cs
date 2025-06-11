using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using interception.enums;

namespace interception.discord {
    public class webhook {
        public string username { get; set; }
        public string avatar_url { get; set; }
        public string content { get; set; }
        public List<embed> embeds { get; private set; }
        public e_webhook_flags flags { get; set; }
        [JsonIgnore]
        public byte[] file_data { get; set; }
        [JsonIgnore]
        public string file_name { get; set; }

        public webhook(string username, string avatar_url, string content, e_webhook_flags flags = 0) {
            this.username = username;
            this.avatar_url = avatar_url;
            if (content != null && content.Length > 2000)
                throw new ArgumentOutOfRangeException("content length cannot be more than 2000");
            this.content = content;
            this.embeds = new List<embed>(10);
            this.flags = flags;
            this.file_data = null;
            this.file_name = null;
        }

        public void add_embed(embed _embed) {
            if (embeds.Count >= 10)
                throw new ArgumentOutOfRangeException("cannot add more than 10 embeds");
            embeds.Add(_embed);
        }

        public void add_file(string filename) {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file \"{filename}\" not exist");
            this.file_data = File.ReadAllBytes(filename);
            if (file_data.Length >= 1000000)
                throw new ArgumentOutOfRangeException("file size is more than 10 mb");
            this.file_name = Path.GetFileName(filename);
        }

        public void add_file(string name, byte[] data) {
            if (data.Length >= 1000000)
                throw new ArgumentOutOfRangeException("file size is more than 10 mb");
            this.file_data = data;
            this.file_name = name;
        }

        public string serialize_json() {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
