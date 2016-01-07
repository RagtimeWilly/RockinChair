using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RockinChair.SampleApp.Controllers
{
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
            _redirect = ConfigurationManager.AppSettings["Redirect"];
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
}
