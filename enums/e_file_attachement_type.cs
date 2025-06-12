using System;
using System.Xml.Serialization;

namespace interception.enums {
    public enum e_file_attachement_type {
        [XmlEnum("path")]
        path,
        [XmlEnum("base64")]
        base64
    }
}
