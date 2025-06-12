using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using interception.discord.types;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_embed")]
    public class s_embed {
        [XmlAttribute]
        public string color { get; set; }
        public s_embed_author author { get; set; }
        [XmlAttribute]
        public string title { get; set; }
        [XmlAttribute]
        public string url { get; set; }
        [XmlAttribute]
        public string description { get; set; }
        [XmlArrayItem("field")]
        public List<s_embed_field> fields { get; set; }
        public s_embed_image image { get; set; }
        public s_embed_thumbnail thumbnail { get; set; }
        public s_embed_footer footer { get; set; }
        public DateTime? timestamp { get; set; }

        public s_embed() {
            fields = new List<s_embed_field>(25);
        }

        public static implicit operator embed(s_embed e) {
            var em = new embed();
            em.add_color(e.color);
            em.add_author(e.author);
            em.add_title(e.title);
            em.add_url(e.url);
            em.add_description(e.description);
            var len = e.fields.Count;
            for (int i = 0; i < len; i++)
                em.add_field(e.fields[i]);
            em.add_image(e.image);
            em.add_thumbnail(e.thumbnail);
            em.add_footer(e.footer);
            if (e.timestamp != null)
                em.add_timestamp((DateTime)e.timestamp);
            return em;
        }

        public static implicit operator s_embed(embed e) {
            var em = new s_embed();
            em.color = e.color_hex;
            em.author = e.author;
            em.title = e.title;
            em.url = e.url;
            em.description = e.description;
            var len = e.fields.Count;
            for (int i = 0; i < len; i++)
                em.fields.Add(e.fields[i]);
            em.image = e.image;
            em.thumbnail = e.thumbnail;
            em.footer = e.footer;
            em.timestamp = e.timestamp;
            return em;
        }
    }
}
