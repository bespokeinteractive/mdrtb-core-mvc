using System;
using System.Collections.Generic;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class HomeIndexViewModel {
        public List<Facility> Facilities { get; set; }

        public HomeIndexViewModel() {
            Facilities = new List<Facility>();
        }
    }
}
