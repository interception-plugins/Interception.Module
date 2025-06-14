using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using interception.enums;
using interception.serialization.types.discord;

namespace interception.discord.types {
    public class webhook {
        public string username { get; set; }
        public string avatar_url { get; set; }
        public string content { get; set; }
        public List<embed> embeds { get; private set; }
        public e_webhook_flag flags { get; set; }
        [JsonIgnore]
        public List<file_attachement> files { get; private set; }

        public webhook(string username, string avatar_url, string content, e_webhook_flag flags = 0) {
            if (username != null && (username.Length == 0 || username.Length > 80))
                throw new ArgumentOutOfRangeException("username length must be in range 1-80");
            this.username = username;
            if (avatar_url != null && !avatar_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !avatar_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{avatar_url}\")");
            this.avatar_url = avatar_url;
            if (content != null && content.Length > 2000)
                throw new ArgumentOutOfRangeException("content length must be less or equal 2000");
            this.content = content;
            this.embeds = new List<embed>(10);
            this.flags = flags;
            this.files = new List<file_attachement>(10);
        }

        public void add_embed(embed _embed) {
            if (embeds.Count >= 10)
                throw new ArgumentOutOfRangeException("cannot add more than 10 embeds");
            embeds.Add(_embed);
        }

        public void add_file(string filename) {
            if (files.Count >= 10)
                throw new ArgumentOutOfRangeException("cannot add more than 10 files");
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file \"{filename}\" not exist");
            byte[] data = File.ReadAllBytes(filename);
            if (data.Length >= 10000000)
                throw new ArgumentOutOfRangeException("file size must be less or equal 10 mb");
            string name = Path.GetFileName(filename);
            files.Add(new file_attachement(data, name));
        }

        public void add_file(byte[] data, string name) {
            if (files.Count >= 10)
                throw new ArgumentOutOfRangeException("cannot add more than 10 files");
            if (data.Length >= 10000000) // or 10485760?
                throw new ArgumentOutOfRangeException("file size must be less or equal 10 mb");
            files.Add(new file_attachement(data, name));
        }

        public void add_file(string base64, string name) {
            if (files.Count >= 10)
                throw new ArgumentOutOfRangeException("cannot add more than 10 files");
            var data = Convert.FromBase64String(base64);
            if (data.Length >= 10000000)
                throw new ArgumentOutOfRangeException("file size must be less or equal 10 mb");
            files.Add(new file_attachement(data, name));
        }

        public string serialize_json_data() {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public s_webhook serialize() {
            var wh = new s_webhook(username, avatar_url, content);
            var len = embeds.Count;
            for (int i = 0; i < len; i++)
                wh.embeds.Add((s_embed)embeds[i]);
            len = files.Count;
            for (int i = 0; i < len; i++)
                wh.files.Add(new s_file_attachement(e_file_attachement_type.base64, Convert.ToBase64String(files[i].data), files[i].name));
            return wh;
        }
    }
}
