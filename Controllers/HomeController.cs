﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Authorization;
using EtbSomalia.ViewModel;
using EtbSomalia.Services;

namespace EtbSomalia.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index(HomeIndexViewModel model) {
            CoreService Core = new CoreService(HttpContext);

            model.Dashboard = Core.GetDashboardSummary();
            model.Facilities = Core.GetFacilitiesRandom(12);

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
