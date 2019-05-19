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
        public PatientTransfer Transfer { get; set; }
        public PatientProgram Program { get; set; }
        public PatientRegimen Regimen { get; set; }
        public PatientExamination Examination { get; set; }

        public string DateOfBirth { get; set; }
        public string RegimenDate { get; set; }
        public string OutcomeDate { get; set; }
        public string TransferDate { get; set; }

        public IEnumerable<SelectListItem> Centers { get; set; }
        public IEnumerable<SelectListItem> Gender { get; set; }
        public IEnumerable<SelectListItem> Facility { get; set; }
        public IEnumerable<SelectListItem> Outcomes { get; set; }
        public IEnumerable<SelectListItem> ExamOpts { get; set; }
        public IEnumerable<SelectListItem> Regimens { get; set; }

        public List<Examinations> Examinations { get; set; }
        public List<Contacts> Contacts { get; set; }

        public PatientProfileViewModel() {
            Transfer = new PatientTransfer();
            Patient = new Patient();
            Program = new PatientProgram();
            Regimen = new PatientRegimen();
            Examination = new PatientExamination();

            DateOfBirth = DateTime.Now.ToString("dd/MM/yyyy");
            RegimenDate = DateTime.Now.ToString("dd/MM/yyyy");
            OutcomeDate = DateTime.Now.ToString("dd/MM/yyyy");
            TransferDate = DateTime.Now.ToString("dd/MM/yyyy");

            Centers = new List<SelectListItem>();
            Facility = new List<SelectListItem>();
            Outcomes = new List<SelectListItem>();
            ExamOpts = new List<SelectListItem>();
            Regimens = new List<SelectListItem>();

            LatestVitals = new Vitals();
            Examinations = new List<Examinations>();
            Contacts = new List<Contacts>();

            Gender = new PatientService().InitializeGender();
        }
    }
}
