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
        public string Age { get; set; }
        public string AddedOn { get; set; }

        public PatientSearch() {
            Patient = new Patient();
            Program = new PatientProgram();
            Status = "";
            Facility = "";
            Age = "";
            AddedOn = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}
