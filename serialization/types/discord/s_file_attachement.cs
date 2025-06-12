using System;
using System.Xml.Serialization;

using interception.enums;

namespace interception.serialization.types.discord {
    [Serializable]
    [XmlType("discord_file_attachement")]
    public class s_file_attachement {
        public e_file_attachement_type type { get; set; }
        public string path_or_data { get; set; }
        public string file_name { get; set; }

        public s_file_attachement() {}

        public s_file_attachement(e_file_attachement_type type, string path_or_data, string file_name) {
            this.type = type;
            this.path_or_data = path_or_data;
            this.file_name = file_name;
        }
    }
}
