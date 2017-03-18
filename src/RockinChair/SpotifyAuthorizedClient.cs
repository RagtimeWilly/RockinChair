using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RockinChair
{
    public class SpotifyAuthorizedClient : SpotifyClient
    {
        private string _token;

        public SpotifyAuthorizedClient(Func<HttpClient> clientFactory, string token)
            : base(clientFactory)
        {
            _token = token;
        }

        public void UpdateToken(string newToken)
        {
            _token = newToken;
        }

        public async Task<string> GetPlaylists(string username)
        {
            var url = $"https://api.spotify.com/v1/users/{username}/playlists";

            using (var httpClient = ClientFactory())
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

            using (var httpClient = ClientFactory())
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
