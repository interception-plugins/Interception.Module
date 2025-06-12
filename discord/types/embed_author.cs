using System;

namespace interception.discord.types {
    public class embed_author {
        public string name { get; private set; }
        public string url { get; private set; }
        public string icon_url { get; private set; }

        public embed_author() {
            this.name = null;
            this.url = null;
            this.icon_url = null;
        }

        public embed_author(string name, string url, string icon_url) {
            if (name.Length > 256)
                throw new ArgumentOutOfRangeException("author.name length cannot be more than 256");
            this.name = name;
            if (url != null && !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{url}\")");
            this.url = url;
            if (icon_url != null && !icon_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !icon_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{icon_url}\")");
            this.icon_url = icon_url;
        }
    }
}
