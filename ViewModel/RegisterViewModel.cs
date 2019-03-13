using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class RegisterViewModel
    {
        public string Type { get; set; }
        public List<Register> Register { get; set; }
        public List<SelectListItem> Facilities { get; set; }
        public Facility Active { get; set; }

        public DateTime Date1x { get; set; }
        public DateTime Date2x { get; set; }

        public RegisterViewModel() {
            Type = "tb";

            Date1x = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Date2x = Date1x.AddMonths(1).AddDays(-1);

            Register = new List<Register>();
            Facilities = new List<SelectListItem>();
            Active = new Facility();
        }
    }
}
