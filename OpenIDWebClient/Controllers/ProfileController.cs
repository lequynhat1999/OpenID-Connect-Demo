using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenIDWebClient.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController()
        {

        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
