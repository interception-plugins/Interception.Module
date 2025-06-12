using System;
using System.Xml.Serialization;

using interception.enums;

namespace interception.serialization.types.zones {
    [Serializable]
    [XmlType("mesh_zone")]
    public class s_mesh_zone : s_zone {
        [XmlAttribute]
        public float height { get; set; }
        [XmlAttribute]
        public int? mask { get; set; }

        public s_mesh_zone() { }
        public s_mesh_zone(e_zone_type type, string name, s_vector3 position, float height, int? mask) : base(type, name, position) {
            this.height = height;
            this.mask = mask;
        }
    }
}
