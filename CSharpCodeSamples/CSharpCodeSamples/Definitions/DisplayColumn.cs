namespace CSharpCodeSamples.Definitions
{
    using System;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    using Common.Interfaces.Models.Definitions;

    [JsonObject][Serializable]
    public class DisplayColumn : IDisplayColumn_Build, IDisplayColumn
    {
        private string _columnMinWidth;
        private string _columnWidth;
        private string _textAlign;
        private string _textAlignHeader;

        [JsonProperty(PropertyName="minwidth")]
        public string ColumnMinWidth
        {
            get { return _columnMinWidth; }
            set { _columnMinWidth = value.Trim(); }
        }
        [JsonProperty(PropertyName="width")]
        public string ColumnWidth
        {
            get { return _columnWidth; }
            set
            {
                _columnWidth = value.Trim();
                if (_columnWidth != "" && !_columnWidth.EndsWith("px")) _columnWidth += "px";
            }
        }
        [JsonProperty(PropertyName="dataType")]
        public string DataType        { get; set; }
        [JsonProperty(PropertyName="key")]
        public string Key             { get; set; }
        [JsonProperty(PropertyName="headerText")]
        public string HeaderText      { get; set; }
        [JsonProperty(PropertyName="template")]
        public string Template        { get; set; }
        [JsonIgnore][IgnoreDataMember]
        public string TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value.Trim().ToLowerInvariant();
                if (_textAlign != "left" && _textAlign != "center" && _textAlign != "right")
                    _textAlign = "left";
            }
        }
        [JsonIgnore][IgnoreDataMember]
        public string TextAlignHeader
        {
            get { return _textAlignHeader; }
            set
            {
                _textAlignHeader = value.Trim().ToLowerInvariant();
                if (_textAlignHeader != "left" && _textAlignHeader != "center" && _textAlignHeader != "right")
                    _textAlignHeader = "left";
            }
        }
        [JsonProperty(PropertyName = "hidden")]
        public bool   IsHidden        { get; set; }
        [JsonIgnore][IgnoreDataMember]
        public bool   IsUpdatable     { get; set; }
    }
}
