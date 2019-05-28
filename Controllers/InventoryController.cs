using System;
using EtbSomalia.Models;
using EtbSomalia.Services;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EtbSomalia.Controllers
{
    [Authorize(Roles = "Administrator, Super User, Regional Admin, Agency Admin, Facility Admin")]
    public class InventoryController : Controller
    {
        [Route("inventory")]
        public IActionResult Index(InventoryIndexViewModel model, DrugService service, long fac = 0) {
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
            Facility facility = null; 
            DrugCategory category = null;

            if (!facl.Equals(0))
                facility = new Facility(facl);
            if (!catg.Equals(0))
                category = new DrugCategory(catg);
            if (string.IsNullOrEmpty(filter))
                filter = "";

            return Json(new DrugService().GetInventoryDrugs(facility, category, filter));
        }

        public JsonResult GetExpiredDrugBatches(long facl, long catg = 0, string filter = "") {
            Facility facility = null;
            DrugCategory category = null;

            if (!facl.Equals(0))
                facility = new Facility(facl);
            if (!catg.Equals(0))
                category = new DrugCategory(catg);
            if (string.IsNullOrEmpty(filter))
                filter = "";

            return Json(new DrugService().GetDrugBatches(facility, category, DateTime.Now, true, false, filter));
        }

        public JsonResult GetDrugReceiptDetails(long facl, long catg = 0, string filter = "") {
            Facility facility = null;
            DrugCategory category = null;

            if (!facl.Equals(0))
                facility = new Facility(facl);
            if (!catg.Equals(0))
                category = new DrugCategory(catg);
            if (string.IsNullOrEmpty(filter))
                filter = "";

            return Json(new DrugService().GetDrugReceiptDetails(facility, category, null, null, filter));
        }

    }
}
