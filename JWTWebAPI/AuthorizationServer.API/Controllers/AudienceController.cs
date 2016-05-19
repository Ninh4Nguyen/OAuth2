using AuthorizationServer.API.Models;
using AuthorizationServer.API.Services;
using System.Web.Http;

namespace AuthorizationServer.API.Controllers
{
    [RoutePrefix("api/audience")]
    public class AudienceController : ApiController
    {
        private IAudienceServices _audienceServices;
        public AudienceController(IAudienceServices audienceServices)
        {
            _audienceServices = audienceServices;
        }
        [Route("register")]
        public IHttpActionResult Post(AudienceModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            Audience audience = _audienceServices.Create(model.Name);
            return Ok<Audience>(audience);
        }
    }
}
