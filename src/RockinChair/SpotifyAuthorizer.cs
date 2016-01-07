using Newtonsoft.Json;
using RockinChair.Data;
using RockinChair.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RockinChair
{
    public class SpotifyAuthorizer
    {
        private const string AuthorizeUrl = "https://accounts.spotify.com/authorize";
        private const string TokenUrl = "https://accounts.spotify.com/api/token";

        private readonly Func<HttpClient> _clientFactory;

        public SpotifyAuthorizer(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public static Uri GetAuthorizationUrl(string clientId, string redirect, string[] scopes)
        {
            var scope = scopes.Any() ? $"&scope={string.Join("%20", scopes)}" : string.Empty;

            return new Uri(
                $"{AuthorizeUrl}" +
                $"?response_type=code" +
                $"&client_id={clientId}" +
                $"{scope}" +
                $"&redirect_uri={redirect}");
        }

        public async Task<AuthenticationToken> GetToken(string clientId, string clientSecret)
        {
            var content = "grant_type=client_credentials";

            return await GetToken(clientId, clientSecret, content);
        }

        public async Task<AuthenticationToken> ExchangeCode(string clientId, string clientSecret, string code, string redirect)
        {
            var content =
                $"grant_type=authorization_code" +
                $"&code={code}" +
                $"&redirect_uri={redirect}";

            return await GetToken(clientId, clientSecret, content);
        }

        public async Task<AuthenticationToken> RefreshToken(string clientId, string clientSecret, string refreshToken)
        {
            var content =
                $"grant_type=refresh_token" +
                $"&refresh_token={refreshToken}";

            return await GetToken(clientId, clientSecret, content);
        }

        private async Task<AuthenticationToken> GetToken(string clientId, string clientSecret, string content)
        {
            using (var httpClient = _clientFactory())
            {
                var header = new AuthenticationHeaderValue("Basic", ($"{clientId}:{clientSecret}").ToBase64());
                var stringContent = content.ToStringContent();

                httpClient.DefaultRequestHeaders.Authorization = header;

                var response = await httpClient.PostAsync(TokenUrl, stringContent);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AuthenticationToken>(json);
            }
        }
    }
}
