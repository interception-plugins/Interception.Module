using System;
using System.Xml.Serialization;

using interception.enums;

namespace interception.serialization.types.zones {
    [Serializable]
    [XmlType("sphere_zone")]
    public class s_sphere_zone : s_zone {
        [XmlAttribute]
        public float radius { get; set; }

        public s_sphere_zone() { }
        public s_sphere_zone(e_zone_type type, string name, s_vector3 position, float radius) : base(type, name, position) {
            this.radius = radius;
        }
    }
}
