using System;
using System.Xml.Serialization;

namespace interception.enums {
    public enum e_webhook_flag : int {
        [XmlEnum("suppress_embeds")]
        suppress_embeds = 4,
        [XmlEnum("suppress_notifications")]
        suppress_notifications = 4096
    }
}
