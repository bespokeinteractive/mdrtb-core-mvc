﻿using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EtbSomalia.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        [Route("inventory")]
        public IActionResult Index(InventoryIndexViewModel model) {
            return View(model);
        }
    }
}