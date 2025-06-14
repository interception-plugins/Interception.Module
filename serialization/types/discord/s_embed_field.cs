using System;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed_field")]
    public class s_embed_field {
        [XmlAttribute]
        public string name { get; set; }
        public string value { get; set; }
        [XmlAttribute]
        public bool inline { get; set; }

        public s_embed_field() {}

        public s_embed_field(string name, string value, bool inline) {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }

        public static implicit operator embed_field(s_embed_field f) {
            if (f == null) return null;
            return new embed_field(f.name, f.value, f.inline);
        }

        public static implicit operator s_embed_field(embed_field f) {
            if (f == null) return null;
            return new s_embed_field(f.name, f.value, f.inline);
        }
    }
}
