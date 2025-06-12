using System;
using System.Xml.Serialization;

namespace interception.enums {
    public enum e_webhook_flag : int {
        [XmlEnum("disable_auto_embeds")]
        disable_auto_embeds = 4,
        [XmlEnum("silent")]
        silent = 4096
    }
}
