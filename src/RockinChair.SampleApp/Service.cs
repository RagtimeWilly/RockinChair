using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace RockinChair.SampleApp
{
    public class Service
    {
        private readonly ManualResetEvent _start;
        private readonly Func<string, SpotifyAuthorizedClient> _clientFactory;
        private readonly SpotifyAuthorizer _authorizer;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirect;

        private string _authCode; 

        public Service(Func<string, SpotifyAuthorizedClient> clientFactory, SpotifyAuthorizer authorizer)
        {
            _clientFactory = clientFactory;
            _authorizer = authorizer;
            _clientId = ConfigurationManager.AppSettings["ClientId"];
            _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            _redirect = ConfigurationManager.AppSettings["Redirect"];

            _start = new ManualResetEvent(false);
        }

        public async Task Start()
        {
            _start.WaitOne();

            var token = await _authorizer.ExchangeCode(_clientId, _clientSecret, _authCode, _redirect);

            var refreshToken = token.RefreshToken;

            var spotify = _clientFactory(token.Token);

            // Perform authorized action
            await spotify.AddTrackToPlaylist("ragtimewilly", "0k9tBC49nZb8w3P0h58h2c", "spotify:track:49ZR4CJXkG8cKigutb66un");

            // Refresh token
            token = await _authorizer.RefreshToken(_clientId, _clientSecret, refreshToken);
        }

        public void Authorize(string authCode)
        {
            _authCode = authCode;
            _start.Set();
        }
    }
}
