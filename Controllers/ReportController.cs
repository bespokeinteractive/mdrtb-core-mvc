using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EtbSomalia.Controllers
{
    public class ReportController : Controller
    {
        [Route("reports")]
        public IActionResult Index() {
            return View();
        }
    }
}
