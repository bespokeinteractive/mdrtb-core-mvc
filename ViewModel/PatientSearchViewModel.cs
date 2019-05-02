using System;
using System.Collections.Generic;

namespace EtbSomalia.ViewModel
{
    public class PatientSearchViewModel
    {
        public string Query { get; set; }
        public List<PatientSearch> Search { get; set; }

        public PatientSearchViewModel() {
            Query = "";
            Search = new List<PatientSearch>();
        }
    }
}
