using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Authorization;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("live")]
        public IActionResult Live() {
            return LocalRedirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
