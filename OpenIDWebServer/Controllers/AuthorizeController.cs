using Microsoft.AspNetCore.Mvc;
using OpenIDWebServer.Models;

namespace OpenIDWebServer.Controllers
{
    [Route("oauth2/[controller]")]
    public class AuthorizeController : Controller
    {
        public IActionResult Index(AuthenticationRequestModel authenticationRequest)
        {
            return View(authenticationRequest);
        }

        [HttpPost("authorize")]
        public IActionResult Authorize(AuthenticationRequestModel authenticationRequest, string user)
        {
            return View(authenticationRequest);
        }
    }
}
