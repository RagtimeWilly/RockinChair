using Newtonsoft.Json;

namespace RockinChair.Data
{
    public class Image
    {
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
    }
}
