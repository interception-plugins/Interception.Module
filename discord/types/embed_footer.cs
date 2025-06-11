using System;

namespace interception.discord.types {
    public class embed_footer {
        public string text { get; private set; }
        public string icon_url { get; private set; }

        public embed_footer(string text, string icon_url) {
            this.text = text;
            if (icon_url != null && !icon_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !icon_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{icon_url}\")");
            this.icon_url = icon_url;
        }
    }
}
