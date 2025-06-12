using System;
using System.Xml.Serialization;

using interception.enums;

namespace interception.serialization.types.zones {
    [Serializable]
    [XmlType("box_zone")]
    public class s_box_zone : s_zone {
        public s_vector3 forward { get; set; }
        public s_vector3 size { get; set; }

        public s_box_zone() { }
        public s_box_zone(e_zone_type type, string name, s_vector3 position, s_vector3 forward, s_vector3 size) : base(type, name, position) {
            this.forward = forward;
            this.size = size;
        }
    }
}
