using System;
using System.Xml.Serialization;

namespace interception.enums {
    public enum e_keycode {
        [XmlEnum("space")]
        space = 0,
        [XmlEnum("unused0")]
        unused0 = 1, // not used at all i guess
        [XmlEnum("unused1")]
        unused1 = 2, // not used at all i guess
        [XmlEnum("x")]
        x = 3, // depends on settings
        [XmlEnum("z")]
        z = 4, // depends on settings
        [XmlEnum("shift1")]
        shift1 = 5, // depends on settings
        [XmlEnum("q")]
        q = 6, // depends on settings
        [XmlEnum("e")]
        e = 7, // depends on settings
        [XmlEnum("b")]
        b = 8, // works only if player has a gun with equipped tactical, also on_key_up getting called instantly
        [XmlEnum("shift0")]
        shift0 = 9,
        [XmlEnum("comma")]
        comma = 10,
        [XmlEnum("period")]
        period = 11,
        [XmlEnum("slash")]
        slash = 12,
        [XmlEnum("semicolon")]
        semicolon = 13,
        [XmlEnum("quote")]
        quote = 14,
        [XmlEnum("unknown")]
        unknown = 15 // not used
    }
}
