using System;
using System.Xml.Serialization;

using UnityEngine;

namespace interception.serialization.types {
    [Serializable]
    public class s_quaternion {
        [XmlAttribute]
        public float x { get; set; }
        [XmlAttribute]
        public float y { get; set; }
        [XmlAttribute]
        public float z { get; set; }
        [XmlAttribute]
        public float w { get; set; }

        public s_quaternion() {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
            this.w = 0f;
        }

        public s_quaternion(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator Quaternion(s_quaternion q) {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }

        public static implicit operator s_quaternion(Quaternion q) {
            return new s_quaternion(q.x, q.y, q.z, q.w);
        }

        public override string ToString() {
            return $"({x}, {y}, {z}, {w})";
        }
    }
}
