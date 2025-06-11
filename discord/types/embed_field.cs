using System;

namespace interception.discord.types {
    public class embed_field {
        public string name { get; private set; }
        public string value { get; private set; }
        public bool inline { get; private set; }

        public embed_field(string name, string value, bool inline) {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }
    }
}
