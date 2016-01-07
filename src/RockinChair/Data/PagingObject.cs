using Newtonsoft.Json;
using System.Collections.Generic;

namespace RockinChair.Data
{
    public class PagingObject<T>
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "items")]
        public T[] Items { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }

        [JsonProperty(PropertyName = "next")]
        public string Next { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
        
        [JsonProperty(PropertyName = "previous")]
        public string Previous { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }
}
