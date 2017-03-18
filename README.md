# Rockin' Chair

![Build Status](https://ci.appveyor.com/api/projects/status/github/RagtimeWilly/RockinChair?branch=master&svg=true) [![NuGet](https://img.shields.io/nuget/v/RockinChair.svg)](https://www.nuget.org/packages/RockinChair/)

.NET Spotify API client to allow automated curation of playlists. 

## Installing from NuGet

`PM> Install-Package RockinChair`

## Sample

There is a [Sample Application](https://github.com/RagtimeWilly/RockinChair/tree/master/src/RockinChair.SampleApp) included that demonstrates how a .NET application can authenticate itself with the spotify api.

## Authentication

As described in in the [Authorization Guide](https://developer.spotify.com/web-api/authorization-guide/) you first need to [register your application](https://developer.spotify.com/my-applications) to get a unique client id and client secret key to use in the authorization flows.

Once you have these the suggested approach is to set up a controller which can be used to handle authentication for your application:

```
public class AuthController : ApiController
{
    private readonly Service _service;
    private readonly string _clientId;
    private readonly string _redirect;
    private readonly string[] _requiredScopes;

    public AuthController(Service service)
    {
        _service = service;
        _clientId = ConfigurationManager.AppSettings["ClientId"];
        _redirect = http://localhost:[PortNumber]/api/auth/Authenticated";
        _requiredScopes = ConfigurationManager.AppSettings["RequiredScopes"].Split(',');
    }

    [HttpGet]
    public HttpResponseMessage Get()
    {
        var response = Request.CreateResponse(HttpStatusCode.Found);
        response.Headers.Location = SpotifyAuthorizer.GetAuthorizationUrl(_clientId, _redirect, _requiredScopes);
        return response;
    }

    [HttpGet]
    public void Authenticated(string code)
    {
        _service.Authorize(code);
    }
}
```

The `Get` reponse will redirect the user to the to the Spotify Accounts service which will present details of the scopes for which access is being sought. If the user is not logged in, they are prompted to do so using their Spotify credentials. When they have logged in they will be re-directed to the `Authenticated` method which makes a call to start the application service:
```
private readonly ManualResetEvent _start;

private readonly string _clientId;
private readonly string _clientSecret;
private readonly string _redirect;

private string _authCode; 

public Service()
{
    _clientId = ConfigurationManager.AppSettings["ClientId"];
    _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
    _redirect = ConfigurationManager.AppSettings["Redirect"];

    _start = new ManualResetEvent(false);
}

public async Task Start()
{
    // Wait until authorization complete before starting
    _start.WaitOne();

    var token = await _authorizer.ExchangeCode(_clientId, _clientSecret, _authCode, _redirect);

    var spotify = new SpotifyAuthorizedClient(() => new HttpClient(), token.Token);

    // Perform authorized action
}

public void Authorize(string authCode)
{
    _authCode = authCode;
    _start.Set();
}
```

The token will expire and need to be refreshed periodically for long running applications. This can be done using the [`SpotifyAuthorizer`](https://github.com/RagtimeWilly/RockinChair/blob/master/src/RockinChair/SpotifyAuthorizer.cs):
```
AuthToken = await _authorizer.ExchangeCode(_config.ClientId, _config.ClientSecret, authCode, _config.Redirect);

var refreshToken = AuthToken.RefreshToken;
var expiresIn = AuthToken.ExpiresIn;

while (true)
{
    await Task.Delay(TimeSpan.FromSeconds(expiresIn - 500));

    AuthToken = await _authorizer.RefreshToken(_config.ClientId, _config.ClientSecret, refreshToken);

    Console.WriteLine("AuthToken refreshed!");
}
```

## Spotify Client

The basic (unauthorized) client just allows you to search for [`Tracks`](https://github.com/RagtimeWilly/RockinChair/blob/master/src/RockinChair/Data/Track.cs) using the artist and track name:
```
var spotify = new SpotifyClient(() => new HttpClient());

var tracks = spotify
              .SearchTracks("The Band", "The Weight")
              .Result;
```

## Spotify Authorized Client

The authorized client allows you to access playlist ids and add tracks to them:
```
var spotify = new SpotifyAuthorizedClient(() => new HttpClient(), "authToken");

var playlists = spotify
                  .GetPlaylists("someUsername")
                  .Result;

await spotify.AddTrackToPlaylist("someUsername", "playlistId", "trackId");
```

## Getting help

If you have any problems or suggestions please create an [issue](https://github.com/RagtimeWilly/RockinChair/issues) or a [pull request](https://github.com/RagtimeWilly/RockinChair/pulls)