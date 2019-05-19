using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Authorization;
using EtbSomalia.ViewModel;
using EtbSomalia.Services;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index(HomeIndexViewModel model, CoreService service) {
            model.Facilities = service.GetFacilitiesRandom(12);
            return View(model);
        }

        [Route("live")]
        public IActionResult Live() {
            return LocalRedirect("/");
        }

        [Route("error/404")]
        public IActionResult Error404(int code) {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code) {
            return View(code);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
