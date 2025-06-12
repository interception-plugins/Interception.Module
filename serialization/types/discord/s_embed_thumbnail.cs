using System;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed_thumbnail")]
    public class s_embed_thumbnail {
        [XmlAttribute]
        public string url { get; set; }

        public s_embed_thumbnail() { }

        public s_embed_thumbnail(string url) {
            this.url = url;
        }

        public static implicit operator embed_thumbnail(s_embed_thumbnail t) {
            return new embed_thumbnail(t.url);
        }

        public static implicit operator s_embed_thumbnail(embed_thumbnail t) {
            return new s_embed_thumbnail(t.url);
        }
    }
}
