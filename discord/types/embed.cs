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
        public embed_author author => _author;

        string _title;
        public string title => _title;

        string _url;
        public string url => _url;

        string _description;
        public string description => _description;

        public List<embed_field> fields { get; private set; }

        embed_image _image;
        public embed_image image => _image;

        embed_thumbnail _thumbnail;
        public embed_thumbnail thumbnail => _thumbnail;

        embed_footer _footer;
        public embed_footer footer => _footer;

        DateTime? _timestamp;
        public DateTime? timestamp => _timestamp;

        public embed() {
            fields = new List<embed_field>(25);
        }

        public void add_color(Color _color) {
            this._color = int.Parse(Palette.hex((Color32)_color).Substring(1), NumberStyles.HexNumber);
        }

        public void add_color(Color32 _color) {
            this._color = int.Parse(Palette.hex(_color).Substring(1), NumberStyles.HexNumber);
        }

        public void add_color(string _color) {
            if (!_color.StartsWith("#") || _color.Length != 7)
                throw new ArgumentException("color must be a 6 digit hex value and start with \"#\"");
            this._color = int.Parse(_color.Substring(1), NumberStyles.HexNumber);
        }

        public void add_color(int _color) {
            if (_color < 0 || _color > 0xFFFFFF)
                throw new ArgumentOutOfRangeException("color must be a value between 0 and 16777215");
            this._color = _color;
        }

        public void add_author(embed_author _author) {
            this._author = _author;
        }

        public void add_title(string _title) {
            if (_title.Length > 256)
                throw new ArgumentOutOfRangeException("title length must be less or equal 256");
            this._title = _title;
        }

        public void add_url(string _url) {
            if (url != null && !_url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !_url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"passed parameter is not a url (\"{_url}\")");
            this._url = _url;
        }

        public void add_description(string _description) {
            if (_description != null && _description.Length > 4096)
                throw new ArgumentOutOfRangeException("description length must be less or equal 4096");
            this._description = _description;
        }

        public void add_field(embed_field _field) {
            if (fields.Count >= 25)
                throw new ArgumentOutOfRangeException("cannot add more than 25 fields");
            fields.Add(_field);
        }

        public void add_image(embed_image _image) {
            this._image = _image;
        }

        public void add_image(string image_url) {
            this._image = new embed_image(image_url);
        }

        public void add_thumbnail(embed_thumbnail _thumbnail) {
            this._thumbnail = _thumbnail;
        }

        public void add_thumbnail(string image_url) {
            this._thumbnail = new embed_thumbnail(image_url);
        }

        public void add_footer(embed_footer _footer) {
            this._footer = _footer;
        }

        public void add_timestamp(DateTime _timestamp) {
            this._timestamp = _timestamp;
        }
    }
}
