using System;

namespace interception.discord.types {
    public class file_attachment {
        public byte[] data { get; private set; }
        public string name { get; private set; }

        public file_attachment(byte[] data, string name) {
            this.data = data;
            this.name = name;
        }
    }
}
