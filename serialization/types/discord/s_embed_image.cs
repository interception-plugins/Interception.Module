using System;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed_image")]
    public class s_embed_image {
        [XmlAttribute]
        public string url { get; set; }

        public s_embed_image() {}

        public s_embed_image(string url) {
            this.url = url;
        }

        public static implicit operator embed_image(s_embed_image img) {
            return new embed_image(img.url);
        }

        public static implicit operator s_embed_image(embed_image img) {
            return new s_embed_image(img.url);
        }
    }
}
