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
        public List<file_attachment> files { get; private set; }

        public webhook(string username, string avatar_url, string content, e_webhook_flag flags = 0) {
            if (username != null && (username.Length == 0 || username.Length > constants.WEBHOOK_USERNAME_MAX_LEN))
                throw new ArgumentOutOfRangeException($"username length must be in range 1-{constants.WEBHOOK_USERNAME_MAX_LEN}");
            this.username = username;
            if (avatar_url != null && !avatar_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !avatar_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{avatar_url}\")");
            this.avatar_url = avatar_url;
            if (content != null && content.Length > constants.WEBHOOK_CONTENT_MAX_LEN)
                throw new ArgumentOutOfRangeException($"content length must be less or equal {constants.WEBHOOK_CONTENT_MAX_LEN}");
            this.content = content;
            this.embeds = new List<embed>(constants.WEBHOOK_MAX_EMBEDS);
            this.flags = flags;
            this.files = new List<file_attachment>(constants.WEBHOOK_MAX_FILES);
        }

        public void add_embed(embed _embed) {
            if (embeds.Count >= constants.WEBHOOK_MAX_EMBEDS)
                throw new ArgumentOutOfRangeException($"cannot add more than {constants.WEBHOOK_MAX_EMBEDS} embeds");
            embeds.Add(_embed);
        }

        public void add_file(string filename) {
            if (files.Count >= constants.WEBHOOK_MAX_FILES)
                throw new ArgumentOutOfRangeException($"cannot add more than {constants.WEBHOOK_MAX_FILES} files");
            if (!File.Exists(filename))
                throw new FileNotFoundException($"file \"{filename}\" not exist");
            byte[] data = File.ReadAllBytes(filename);
            if (data.Length >= constants.WEBHOOK_MAX_FILE_SIZE)
                throw new ArgumentOutOfRangeException($"file size must be less or equal {constants.WEBHOOK_MAX_FILE_SIZE / 1024 / 1024} mb");
            string name = Path.GetFileName(filename);
            files.Add(new file_attachment(data, name));
        }

        public void add_file(byte[] data, string name) {
            if (files.Count >= constants.WEBHOOK_MAX_FILES)
                throw new ArgumentOutOfRangeException($"cannot add more than {constants.WEBHOOK_MAX_FILES} files");
            if (data.Length >= constants.WEBHOOK_MAX_FILE_SIZE)
                throw new ArgumentOutOfRangeException($"file size must be less or equal {constants.WEBHOOK_MAX_FILE_SIZE / 1024 / 1024} mb");
            files.Add(new file_attachment(data, name));
        }

        public void add_file(string base64, string name) {
            if (files.Count >= constants.WEBHOOK_MAX_FILES)
                throw new ArgumentOutOfRangeException($"cannot add more than {constants.WEBHOOK_MAX_FILES} files");
            var data = Convert.FromBase64String(base64);
            if (data.Length >= constants.WEBHOOK_MAX_FILE_SIZE)
                throw new ArgumentOutOfRangeException($"file size must be less or equal {constants.WEBHOOK_MAX_FILE_SIZE / 1024 / 1024} mb");
            files.Add(new file_attachment(data, name));
        }

        public string serialize_json_data() {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // todo i think it should be moved somewhere else
        public static webhook deserialize_json_data(string data) {
            return JsonConvert.DeserializeObject<webhook>(data);
        }

        public s_webhook serialize() {
            var wh = new s_webhook(username, avatar_url, content);
            var len = embeds.Count;
            for (int i = 0; i < len; i++)
                wh.embeds.Add((s_embed)embeds[i]);
            len = files.Count;
            for (int i = 0; i < len; i++)
                wh.files.Add(new s_file_attachment(e_file_attachment_type.base64, Convert.ToBase64String(files[i].data), files[i].name));
            return wh;
        }
    }
}
