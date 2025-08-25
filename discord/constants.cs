using System;

namespace interception.discord {
    internal static class constants {
        public const int WEBHOOK_USERNAME_MAX_LEN = 80;
        public const int WEBHOOK_CONTENT_MAX_LEN = 2000;
        public const int WEBHOOK_MAX_EMBEDS = 10;
        public const int WEBHOOK_MAX_FILES = 10;
        public const int WEBHOOK_MAX_FILE_SIZE = 10485760; // 10 mb

        public const int EMBED_MAX_FIELDS = 25;
        public const int EMBED_TITLE_MAX_LEN = 256;
        public const int EMBED_DESCRIPTION_MAX_LEN = 4096;

        public const int EMBED_AUTHOR_NAME_MAX_LEN = 4096;

        public const int EMBED_FIELD_NAME_MAX_LEN = 256;
        public const int EMBED_FIELD_VALUE_MAX_LEN = 1024;

        public const int EMBED_FOOTER_TEXT_MAX_LEN = 2048;
    }
}
