using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RockinChair.Data;

namespace RockinChair
{
    public class SpotifyAuthorizedClient
    {
        private const string BaseUrl = "https://api.spotify.com/v1";

        private readonly Func<HttpClient> _clientFactory;

        private string _token;

        public SpotifyAuthorizedClient(Func<HttpClient> clientFactory, string token)
        {
            _clientFactory = clientFactory;
            _token = token;
        }

        public void UpdateToken(string newToken)
        {
            _token = newToken;
        }

        public async Task<IEnumerable<Track>> SearchTracks(string artist, string trackName)
        {
            var uri = new Uri($"{BaseUrl}/search?q=track:{trackName}%20artist:{artist}&type=track");

            using (var httpClient = _clientFactory())
            {
                var header = new AuthenticationHeaderValue("Bearer", _token);

                httpClient.DefaultRequestHeaders.Authorization = header;

                var json = await httpClient.GetStringAsync(uri);

                var tracks = JsonConvert.DeserializeObject<Tracks>(json);

                return tracks.Data.Items;
            }
        }

        public async Task<string> GetPlaylists(string username)
        {
            var url = $"https://api.spotify.com/v1/users/{username}/playlists";

            using (var httpClient = _clientFactory())
            {
                var header = new AuthenticationHeaderValue("Bearer", _token);

                httpClient.DefaultRequestHeaders.Authorization = header;

                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task AddTrackToPlaylist(string username, string playlist, string track)
        {
            var url = $"https://api.spotify.com/v1/users/{username}/playlists/{playlist}/tracks?uris={track}";

            using (var httpClient = _clientFactory())
            {
                var header = new AuthenticationHeaderValue("Bearer", _token);

                httpClient.DefaultRequestHeaders.Authorization = header;

                var response = await httpClient.PostAsync(url, null);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
