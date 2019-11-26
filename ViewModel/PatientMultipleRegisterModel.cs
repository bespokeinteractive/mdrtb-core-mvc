using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class PatientMultipleRegisterModel
    {
        public IEnumerable<SelectListItem> Gender { get; set; }
        public IEnumerable<SelectListItem> Facilities { get; set; }
        public IEnumerable<SelectListItem> TBTypes { get; set; }
        public IEnumerable<SelectListItem> TBConfirmation { get; set; }
        public IEnumerable<SelectListItem> TBCategory { get; set; }
        public IEnumerable<SelectListItem> ResistanceProfile { get; set; }
        public IEnumerable<SelectListItem> DotsBy { get; set; }
        public IEnumerable<SelectListItem> Referees { get; set; }
        public IEnumerable<SelectListItem> SputumSmearItems { get; set; }
        public IEnumerable<SelectListItem> GeneXpertItems { get; set; }
        public IEnumerable<SelectListItem> HivExamItems { get; set; }
        public IEnumerable<SelectListItem> XrayExamItems { get; set; }

        public List<PatientModel> PatientModel { get; set; }

        public int Facility { get; set; }

        public PatientMultipleRegisterModel()
        {
            Gender = new PatientService().InitializeGender();

            Facilities = new List<SelectListItem>();
            TBCategory = new List<SelectListItem>();
            ResistanceProfile = new List<SelectListItem> {
                new SelectListItem{Value = "1", Text = "TB"},
                new SelectListItem{Value = "2", Text = "MDRTB"}
            };
            TBConfirmation = new List<SelectListItem> {
                new SelectListItem { Value = "6", Text = "BC"},
                new SelectListItem { Value = "7", Text = "CD"}
            };
            TBTypes = new List<SelectListItem> {
                new SelectListItem { Value = "3", Text = "P"},
                new SelectListItem { Value = "4", Text = "Ep"},
            };

            Facility = 0;

            PatientModel = new List<PatientModel>();
            for (int i = 0; i < 20; i++) {
                PatientModel.Add(new PatientModel());
            }

            DotsBy = new List<SelectListItem>();
            Referees = new List<SelectListItem>();
            SputumSmearItems = new List<SelectListItem>();
            GeneXpertItems = new List<SelectListItem>();
            HivExamItems = new List<SelectListItem>();
            XrayExamItems = new List<SelectListItem>();
        }
    }

    public class PatientModel {
        public PatientProgram PatientProgram { get; set; }
        public PatientExamination Examination { get; set; }

        public string Age { get; set; }
        public string Date { get; set; }
        public string Weight { get; set; }
        public string Address { get; set; }

        public PatientModel()
        {
            PatientProgram = new PatientProgram();
            Examination = new PatientExamination();

            Date = DateTime.Now.ToString("d MMMM, yyyy");
            Age = "";
            Weight = "";
            Address = "";
        }
    }
}
