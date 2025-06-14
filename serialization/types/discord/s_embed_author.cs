using System;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed_author")]
    public class s_embed_author {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string url { get; set; }
        [XmlAttribute]
        public string icon_url { get; set; }

        public s_embed_author() {}

        public s_embed_author(string name, string url, string icon_url) {
            this.name = name;
            this.url = url;
            this.icon_url = icon_url;
        }

        public static implicit operator embed_author(s_embed_author a) {
            if (a == null) return null;
            return new embed_author(a.name, a.url, a.icon_url);
        }

        public static implicit operator s_embed_author(embed_author a) {
            if (a == null) return null;
            return new s_embed_author(a.name, a.url, a.icon_url);
        }
    }
}
