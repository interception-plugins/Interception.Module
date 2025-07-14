using System;
using System.Xml.Serialization;

using interception.enums;

namespace interception.serialization.types.zones {
    [Serializable]
    [XmlType("zone")]
    [XmlInclude(typeof(s_box_zone))]
    [XmlInclude(typeof(s_sphere_zone))]
    [XmlInclude(typeof(s_mesh_zone))]
    [XmlInclude(typeof(s_distance_fast_zone))]
    [XmlInclude(typeof(s_distance_slow_zone))]
    public abstract class s_zone {
        [XmlAttribute]
        public e_zone_type type { get; set; }
        [XmlAttribute]
        public string name { get; set; }
        public s_vector3 position { get; set; }

        public s_zone() { }
        public s_zone(e_zone_type type, string name, s_vector3 position) {
            this.type = type;
            this.name = name;
            this.position = position;
        }
    }
}
