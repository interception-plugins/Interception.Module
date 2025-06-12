using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using interception.enums;
using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_webhook")]
    public class s_webhook {
        [XmlAttribute]
        public string username { get; set; }
        [XmlAttribute]
        public string avatar_url { get; set; }
        [XmlAttribute]
        public string content { get; set; }
        [XmlArrayItem("embed")]
        public List<s_embed> embeds { get; set; }
        [XmlArrayItem("webhook_flag")]
        public List<e_webhook_flag> flags { get; set; }
        [XmlArrayItem("file_attachement")]
        public List<s_file_attachement> files { get; set; }

        public s_webhook() {
            this.embeds = new List<s_embed>(10);
            this.flags = new List<e_webhook_flag>();
            this.files = new List<s_file_attachement>(10);
        }

        public s_webhook(string username, string avatar_url, string content) {
            this.username = username;
            this.avatar_url = avatar_url;
            this.content = content;
            this.embeds = new List<s_embed>(10);
            this.flags = new List<e_webhook_flag>();
            this.files = new List<s_file_attachement>(10);
        }

        public webhook deserialize() {
            var wh = new webhook(username, avatar_url, content);
            var len = flags.Count;
            for (int i = 0; i < len; i++)
                wh.flags |= flags[i];
            len = embeds.Count;
            for (int i = 0; i < len; i++)
                wh.add_embed(embeds[i]);
            len = files.Count;
            for (int i = 0; i < len; i++) {
                switch (files[i].type) {
                    case e_file_attachement_type.path:
                        wh.add_file(files[i].path_or_data);
                        break;
                    case e_file_attachement_type.base64:
                        wh.add_file(files[i].path_or_data, files[i].file_name);
                        break;
                    default: break;
                }
            }
            return wh;
        }

        public static implicit operator webhook(s_webhook wh) {
            return wh.deserialize();
        }

        public static implicit operator s_webhook(webhook wh) {
            return wh.serialize();
        }
    }
}
