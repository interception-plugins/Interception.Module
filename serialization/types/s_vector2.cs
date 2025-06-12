using System;
using System.Xml.Serialization;

using UnityEngine;

namespace interception.serialization.types {
    [Serializable]
    [XmlType("vector2")]
    public class s_vector2 {
        [XmlAttribute]
        public float x { get; set; }
        [XmlAttribute]
        public float y { get; set; }

        public s_vector2() {
            this.x = 0f;
            this.y = 0f;
        }

        public s_vector2(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2(s_vector2 v) {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator s_vector2(Vector2 v) {
            return new s_vector2(v.x, v.y);
        }

        public override string ToString() {
            return $"({x}, {y})";
        }
    }
}
