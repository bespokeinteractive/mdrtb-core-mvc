using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class InventoryIndexViewModel
    {
        public DateTime Date1x { get; set; }
        public DateTime Date2x { get; set; }
        public Facility Active { get; set; }

        public List<FacilityDrug> InventoryDrugs { get; set; }
        public List<SelectListItem> Facilities { get; set; }

        public InventoryIndexViewModel() {
            Date1x = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Date2x = Date1x.AddMonths(1).AddDays(-1);
            InventoryDrugs = new List<FacilityDrug>();
        }
    }
}
