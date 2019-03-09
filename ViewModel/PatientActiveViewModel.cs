using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class PatientActiveViewModel
    {
        public List<PatientSearch> PatientSearch { get; set; }
        public List<SelectListItem> Facilities { get; set; }
        public Facility Active { get; set; }

        public PatientActiveViewModel() {
            PatientSearch = new List<PatientSearch>();
            Facilities = new List<SelectListItem>();

            Active = new Facility();
        }
    }
}
