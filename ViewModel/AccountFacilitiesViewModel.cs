using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class AccountFacilitiesViewModel
    {
        public List<Facility> Facilities { get; set; }
        public Facility Facility { get; set; }

        public IEnumerable<SelectListItem> Agencies { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }

        public AccountFacilitiesViewModel() {
            Facilities = new List<Facility>();
            Facility = new Facility();
        }
    }
}
