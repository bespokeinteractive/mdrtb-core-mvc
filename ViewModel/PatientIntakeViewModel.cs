using System;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class PatientIntakeViewModel
    {
        public PatientProgram Program { get; set; }
        public Patient Patient { get; set; }

        public PatientIntakeViewModel() {
            Program = new PatientProgram();
            Patient = new Patient();
        }
    }
}
