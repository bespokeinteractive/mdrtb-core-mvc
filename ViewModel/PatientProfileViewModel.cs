using System;
using System.Collections.Generic;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class PatientProfileViewModel
    {
        public Patient Patient { get; set; }
        public PatientProgram Program { get; set; }
        public PatientRegimen Regimen { get; set; }
        public Vitals LatestVitals { get; set; }

        public List<Examinations> Examinations { get; set; }

        public PatientProfileViewModel() {
            Patient = new Patient();
            Program = new PatientProgram();
            Regimen = new PatientRegimen();

            LatestVitals = new Vitals();
            Examinations = new List<Examinations>();
        }
    }
}
