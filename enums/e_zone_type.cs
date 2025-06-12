using System;
using System.Xml.Serialization;

namespace interception.enums {
    public enum e_zone_type {
        [XmlEnum("sphere")]
        sphere,
        [XmlEnum("box")]
        box,
        [XmlEnum("distance_slow")]
        distance_slow,
        [XmlEnum("distance_fast")]
        distance_fast,
        [XmlEnum("mesh")]
        mesh
    }
}
