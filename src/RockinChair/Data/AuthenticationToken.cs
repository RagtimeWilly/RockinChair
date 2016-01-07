using Newtonsoft.Json;

namespace RockinChair.Data
{
    public class AuthenticationToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
