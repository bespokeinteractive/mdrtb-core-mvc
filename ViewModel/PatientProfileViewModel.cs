using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class PatientProfileViewModel
    {
        public Patient Patient { get; set; }
        public Vitals LatestVitals { get; set; }
        public PatientProgram Program { get; set; }
        public PatientRegimen Regimen { get; set; }
        public PatientExamination Examination { get; set; }

        public string DateOfBirth { get; set; }

        public IEnumerable<SelectListItem> Gender { get; set; }
        public IEnumerable<SelectListItem> Facility { get; set; }
        public IEnumerable<SelectListItem> Outcomes { get; set; }
        public IEnumerable<SelectListItem> ExamOpts { get; set; }


        public List<Examinations> Examinations { get; set; }
        public List<Contacts> Contacts { get; set; }

        public PatientProfileViewModel() {
            Patient = new Patient();
            Program = new PatientProgram();
            Regimen = new PatientRegimen();
            Examination = new PatientExamination();

            LatestVitals = new Vitals();
            Examinations = new List<Examinations>();
            Contacts = new List<Contacts>();

            Gender = new PatientService().InitializeGender();
        }
    }
}
