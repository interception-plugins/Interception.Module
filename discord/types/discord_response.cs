using System;
using System.Net;

namespace interception.discord.types {
    public class discord_response {
        public HttpStatusCode status { get; private set; }
        public string content { get; private set; }

        public discord_response(HttpStatusCode status, string content) {
            this.status = status;
            this.content = content;
        }
    }
}
