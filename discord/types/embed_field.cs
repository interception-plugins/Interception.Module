using System;

namespace interception.discord.types {
    public class embed_field {
        public string name { get; private set; }
        public string value { get; private set; }
        public bool inline { get; private set; }

        public embed_field(string name, string value, bool inline) {
            if (name == null || name.Length > 256)
                throw new ArgumentOutOfRangeException("field.name cannot be null and its length must be less or equal 256");
            this.name = name;
            if (value == null || value.Length > 1024)
                throw new ArgumentOutOfRangeException("field.value cannot be null and its length must be less or equal 1024");
            this.value = value;
            this.inline = inline;
        }
    }
}
