using System;

namespace interception.discord.types {
    public class file_attachement {
        public byte[] data { get; private set; }
        public string name { get; private set; }

        public file_attachement(byte[] data, string name) {
            this.data = data;
            this.name = name;
        }
    }
}
