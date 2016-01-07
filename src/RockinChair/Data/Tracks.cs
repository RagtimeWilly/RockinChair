using Newtonsoft.Json;

namespace RockinChair.Data
{
    public class Tracks
    {
        [JsonProperty(PropertyName = "tracks")]
        public PagingObject<Track> Data { get; set; }
    }
}
