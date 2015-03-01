namespace CSharpCodeSamples.Definitions
{
    using Newtonsoft.Json;

    using Common.Interfaces.Models.Definitions;

    public class ResizeSetting : IResizeSetting
    {
        [JsonProperty(PropertyName = "allowResizing")]
        public bool   AllowResizing  { get { return true; } }
        [JsonProperty(PropertyName = "minimumWidth")]
        public int    ColumnMinWidth { get; private set; }
        [JsonProperty(PropertyName = "columnKey")]
        public string Key            { get; private set; }

        public ResizeSetting(string key, int minWidth)
        {
            Key            = key;
            ColumnMinWidth = minWidth;
        }
    }
}
