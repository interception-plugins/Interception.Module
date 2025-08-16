using System;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;
using UnityEngine;
using SDG.Unturned;

namespace interception.discord.types {
    public class embed {
        int _color;
        public int color => _color;
        [JsonIgnore]
        public string color_hex => "#" + color.ToString("X6");

        embed_author _author;
        public embed_author author {
            get {
                return _author;
            }
            set {
                _author = value;
            }
        }

        string _title;
        public string title {
            get {
                return _title;
            }
            set {
                if (value.Length > constants.EMBED_TITLE_MAX_LEN)
                    throw new ArgumentOutOfRangeException($"title length must be less or equal {constants.EMBED_TITLE_MAX_LEN}");
                _title = value;
            }
        }

        string _url;
        public string url {
            get {
                return _url;
            }
            set {
                if (value != null && !value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException($"passed parameter is not a url (\"{value}\")");
                _url = value;
            }
        }

        string _description;
        public string description {
            get {
                return _description;
            }
            set {
                if (value != null && value.Length > constants.EMBED_DESCRIPTION_MAX_LEN)
                    throw new ArgumentOutOfRangeException($"description length must be less or equal {constants.EMBED_DESCRIPTION_MAX_LEN}");
                _description = value;
            }
        }

        public List<embed_field> fields { get; private set; }

        embed_image _image;
        public embed_image image {
            get {
                return _image;
            }
            set {
                _image = value;
            }
        }

        embed_thumbnail _thumbnail;
        public embed_thumbnail thumbnail {
            get {
                return _thumbnail;
            }
            set {
                _thumbnail = value;
            }
        }

        embed_footer _footer;
        public embed_footer footer {
            get {
                return _footer;
            }
            set {
                _footer = value;
            }
        }

        DateTime? _timestamp;
        public DateTime? timestamp {
            get {
                return _timestamp;
            }
            set {
                _timestamp = value;
            }
        }

        public embed() {
            fields = new List<embed_field>(constants.EMBED_MAX_FIELDS);
        }

        public void colorize(Color _color) {
            this._color = int.Parse(Palette.hex((Color32)_color).Substring(1), NumberStyles.HexNumber);
        }

        public void colorize(Color32 _color) {
            this._color = int.Parse(Palette.hex(_color).Substring(1), NumberStyles.HexNumber);
        }

        public void colorize(string _color) {
            if (!_color.StartsWith("#") || _color.Length != 7)
                throw new ArgumentException("color must be a 6 digit hex value and start with \"#\"");
            this._color = int.Parse(_color.Substring(1), NumberStyles.HexNumber);
        }

        public void colorize(int _color) {
            if (_color < 0 || _color > 0xFFFFFF)
                throw new ArgumentOutOfRangeException("color must be a value between 0 and 16777215");
            this._color = _color;
        }

        public void add_field(embed_field _field) {
            if (fields.Count >= constants.EMBED_MAX_FIELDS)
                throw new ArgumentOutOfRangeException($"cannot add more than {constants.EMBED_MAX_FIELDS} fields");
            fields.Add(_field);
        }

        public void set_image_from_url(string image_url) {
            image = new embed_image(image_url);
        }

        public void set_thumbnail_from_url(string image_url) {
            thumbnail = new embed_thumbnail(image_url);
        }

        #region OBSOLETE
        [Obsolete("use \"colorize(Color _color)\" instead")]
        public void add_color(Color _color) => colorize(_color);
        [Obsolete("use \"colorize(Color32 _color)\" instead")]
        public void add_color(Color32 _color) => colorize(_color);
        [Obsolete("use \"colorize(string _color)\" instead")]
        public void add_color(string _color) => colorize(_color);
        [Obsolete("use \"colorize(int _color)\" instead")]
        public void add_color(int _color) => colorize(_color);

        [Obsolete("use property instead")]
        public void add_author(embed_author _author) {
            author = _author;
        }

        [Obsolete("use property instead")]
        public void add_title(string _title) {
            title = _title;
        }

        [Obsolete("use property instead")]
        public void add_url(string _url) {
            url = _url;
        }
        
        [Obsolete("use property instead")]
        public void add_description(string _description) {
            description = _description;
        }

        [Obsolete("use property instead")]
        public void add_image(embed_image _image) {
            image = _image;
        }

        [Obsolete("use \"set_image_from_url(string image_url)\" instead")]
        public void add_image(string image_url) {
            set_image_from_url(image_url);
        }

        [Obsolete("use property instead")]
        public void add_thumbnail(embed_thumbnail _thumbnail) {
            thumbnail = _thumbnail;
        }

        [Obsolete("use \"set_thumbnail_from_url(string image_url)\" instead")]
        public void add_thumbnail(string image_url) {
            set_thumbnail_from_url(image_url);
        }

        [Obsolete("use property instead")]
        public void add_footer(embed_footer _footer) {
            footer = _footer;
        }

        [Obsolete("use property instead")]
        public void add_timestamp(DateTime _timestamp) {
            timestamp = _timestamp;
        }
        #endregion
    }
}
