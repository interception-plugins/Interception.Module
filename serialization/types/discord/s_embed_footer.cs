using System;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed_footer")]
    public class s_embed_footer {
        [XmlAttribute]
        public string text { get; set; }
        [XmlAttribute]
        public string icon_url { get; set; }

        public s_embed_footer() {}

        public s_embed_footer(string text, string icon_url) {
            this.text = text;
            this.icon_url = icon_url;
        }

        public static implicit operator embed_footer(s_embed_footer f) {
            return new embed_footer(f.text, f.icon_url);
        }

        public static implicit operator s_embed_footer(embed_footer f) {
            return new s_embed_footer(f.text, f.icon_url);
        }
    }
}
