using System;

namespace interception.discord.types {
    public class embed_image {
        public string url { get; private set; }

        public embed_image(string url) {
            if (url == null)
                throw new NullReferenceException("url cannot be null");
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{url}\")");
            this.url = url;
        }
    }
}
