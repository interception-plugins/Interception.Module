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

        public static explicit operator embed(s_embed e) {
            var em = new embed();
            if (e.color != null)
                em.add_color(e.color);
            if (e.author != null)
                em.add_author(e.author);
            if (e.title != null)
                em.add_title(e.title);
            if (e.url != null)
                em.add_url(e.url);
            if (e.description != null)
                em.add_description(e.description);
            var len = e.fields.Count;
            for (int i = 0; i < len; i++)
                em.add_field(e.fields[i]);
            if (e.image != null)
                em.add_image(e.image);
            if (e.thumbnail != null)
                em.add_thumbnail(e.thumbnail);
            if (e.footer != null)
                em.add_footer(e.footer);
            if (e.timestamp != null)
                em.add_timestamp((DateTime)e.timestamp);
            return em;
        }

        public static explicit operator s_embed(embed e) {
            var em = new s_embed();
            em.color = e.color_hex;
            if (e.author != null)
                em.author = e.author;
            if (e.title != null)
                em.title = e.title;
            if (e.url != null)
                em.url = e.url;
            if (e.description != null)
                em.description = e.description;
            var len = e.fields.Count;
            for (int i = 0; i < len; i++)
                em.fields.Add(e.fields[i]);
            if (e.image != null)
                em.image = e.image;
            if (e.thumbnail != null)
                em.thumbnail = e.thumbnail;
            if (e.footer != null)
                em.footer = e.footer;
            if (e.timestamp != null)
                em.timestamp = e.timestamp;
            return em;
        }
    }
}
