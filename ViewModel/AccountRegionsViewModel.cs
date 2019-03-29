using System;
using System.Collections.Generic;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class AccountRegionsViewModel
    {
        public List<Region> Regions { get; set; }
        public Region Region { get; set; }

        public AccountRegionsViewModel() {
            Regions = new List<Region>();
            Region = new Region();
        }
    }
}
