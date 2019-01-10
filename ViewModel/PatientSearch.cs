using System;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class PatientSearch
    {
        public Patient Patient { get; set; }
        public PatientProgram Program { set; get; }
        public string Status { get; set; }
        public string Facility { get; set; }
        public string age { get; set; }

        public PatientSearch() {
            Patient = new Patient();
            Program = new PatientProgram();
            Status = "";
            Facility = "";
            age = "";
        }
    }
}
