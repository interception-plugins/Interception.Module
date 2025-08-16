using System;

namespace interception.discord.types {
    public class embed_field {
        public string name { get; private set; }
        public string value { get; private set; }
        public bool inline { get; private set; }

        public embed_field(string name, string value, bool inline) {
            if (name == null || name.Length > constants.EMBED_FIELD_NAME_MAX_LEN)
                throw new ArgumentOutOfRangeException($"field.name cannot be null and its length must be less or equal {constants.EMBED_FIELD_NAME_MAX_LEN}");
            this.name = name;
            if (value == null || value.Length > constants.EMBED_FIELD_VALUE_MAX_LEN)
                throw new ArgumentOutOfRangeException($"field.value cannot be null and its length must be less or equal {constants.EMBED_FIELD_VALUE_MAX_LEN}");
            this.value = value;
            this.inline = inline;
        }
    }
}
