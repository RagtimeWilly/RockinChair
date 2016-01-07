using Newtonsoft.Json;
using System.Collections.Generic;

namespace RockinChair.Data
{
    public class ArtistSimple
    {
        [JsonProperty(PropertyName = "external_urls")]
        public KeyValuePair<string, string> ExternalUrls { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }
    }
}
