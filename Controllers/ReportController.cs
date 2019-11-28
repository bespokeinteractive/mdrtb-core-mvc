using EtbSomalia.Services;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EtbSomalia.Controllers
{
    public class ReportController : Controller
    {
        [Route("reports")]
        public IActionResult Index(ReportViewModel model) {
            CoreService Core = new CoreService(HttpContext);

            model.Dashboard = Core.GetDashboardSummary();
            return View(model);
        }
    }
}
