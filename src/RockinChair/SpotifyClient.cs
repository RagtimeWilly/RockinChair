using Newtonsoft.Json;
using RockinChair.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RockinChair
{
    public class SpotifyClient
    {
        protected const string BaseUrl = "https://api.spotify.com/v1";

        protected readonly Func<HttpClient> _clientFactory;

        public SpotifyClient(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Track>> SearchTracks(string artist, string trackName)
        {
            var uri = new Uri($"{BaseUrl}/search?q=track:{trackName}%20artist:{artist}&type=track");
           
            using (var httpClient = _clientFactory())
            {
                var json = await httpClient.GetStringAsync(uri);

                var tracks = JsonConvert.DeserializeObject<Tracks>(json);

                return tracks.Data.Items;
            }
        }
    }
}
