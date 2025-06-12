using System;
using System.Xml.Serialization;

using UnityEngine;
using SDG.Unturned;

namespace interception.serialization.types {
    [Serializable]
    [XmlType("color32")]
    public class s_color32 {
        [XmlAttribute]
        public byte r { get; set; }
        [XmlAttribute]
        public byte g { get; set; }
        [XmlAttribute]
        public byte b { get; set; }
        [XmlAttribute]
        public byte a { get; set; }

        public s_color32() {
            this.r = byte.MaxValue;
            this.b = byte.MaxValue;
            this.g = byte.MaxValue;
            this.a = byte.MaxValue;
        }

        public s_color32(byte r, byte g, byte b) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = byte.MaxValue;
        }

        public s_color32(byte r, byte g, byte b, byte a) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public string to_hex() {
            return Palette.hex(this);
        }

        public static s_color32 from_hex(string hex) {
            return (Color32)Palette.hex(hex);
        }

        public static implicit operator Color32(s_color32 c) {
            return new Color32(c.r, c.g, c.b, c.a);
        }

        public static implicit operator s_color32(Color32 c) {
            return new s_color32(c.r, c.g, c.b, c.a);
        }
        /*
        public static implicit operator s_color32(Color c) {
            return (s_color32)(Color32)c;
        }

        public static implicit operator s_color(s_color32 c) {
            return (s_color)(Color)c;
        }
        */
        public override string ToString() {
            return $"({r}, {g}, {b}, {a})";
        }
    }
}
