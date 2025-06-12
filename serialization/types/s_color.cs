using System;
using System.Xml.Serialization;

using UnityEngine;
using SDG.Unturned;

namespace interception.serialization.types {
    [Serializable]
    [XmlType("color")]
    public class s_color {
        [XmlAttribute]
        public float r { get; set; }
        [XmlAttribute]
        public float g { get; set; }
        [XmlAttribute]
        public float b { get; set; }
        [XmlAttribute]
        public float a { get; set; }

        public s_color() {
            this.r = 1f;
            this.b = 1f;
            this.g = 1f;
            this.a = 1f;
        }

        public s_color(float r, float g, float b) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1f;
        }

        public s_color(float r, float g, float b, float a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public string to_hex() {
            return Palette.hex((Color)this);
        }

        public static s_color from_hex(string hex) {
            return Palette.hex(hex);
        }

        public static implicit operator Color(s_color c) {
            return new Color(c.r, c.g, c.b, c.a);
        }

        public static implicit operator s_color(Color c) {
            return new s_color(c.r, c.g, c.b, c.a);
        }
        /*
        public static implicit operator s_color(Color32 c) {
            return (Color)c;
        }

        public static implicit operator s_color32(s_color c) {
            return (s_color32)(Color32)c;
        }
        */
        public override string ToString() {
            return $"({r}, {g}, {b}, {a})";
        }
    }
}
