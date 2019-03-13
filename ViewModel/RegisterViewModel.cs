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

        public RegisterViewModel() {
            Type = "tb";

            Register = new List<Register>();
            Facilities = new List<SelectListItem>();
            Active = new Facility();
        }
    }
}
