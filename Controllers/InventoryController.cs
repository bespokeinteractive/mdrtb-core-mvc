using System;
using EtbSomalia.Models;
using EtbSomalia.Services;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EtbSomalia.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        [Route("inventory")]
        public IActionResult Index(InventoryIndexViewModel model, InventoryDrugService service, long fac = 0) {
            CoreService core = new CoreService(HttpContext);
            model.Facilities = core.GetFacilitiesIEnumerable();

            if (fac.Equals(0))
                model.Active = core.GetFacility(Convert.ToInt64(model.Facilities[0].Value));
            else
                model.Active = core.GetFacility(fac);

            model.InventoryDrugs = service.GetInventoryDrugs(model.Active, null);

            return View(model);
        }

        public JsonResult GetInventoryDrugs(long facl, long catg = 0, string filter = "") {
            DrugCategory category = null;

            if (string.IsNullOrEmpty(filter))
                filter = "";
            if (!catg.Equals(0))
                category = new DrugCategory(catg);

            return Json(new InventoryDrugService().GetInventoryDrugs(new Facility(facl), category, filter));
        }
    }
}
