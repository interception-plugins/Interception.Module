using System;
using System.Xml.Serialization;

using UnityEngine;

namespace interception.serialization.types {
    [Serializable]
    public class s_vector3 {
        [XmlAttribute]
        public float x { get; set; }
        [XmlAttribute]
        public float y { get; set; }
        [XmlAttribute]
        public float z { get; set; }

        public s_vector3() {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }

        public s_vector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(s_vector3 v) {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator s_vector3(Vector3 v) {
            return new s_vector3(v.x, v.y, v.z);
        }

        public override string ToString() {
            return $"({x}, {y}, {z})";
        }
    }
}
