using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class PatientIntakeViewModel
    {
        public PatientProgram Program { get; set; }
        public Patient Patient { get; set; }

        public long ArtStarted { get; set; }
        public long CptStarted { get; set; }

        public string RegimenStartedOn { get; set; }

        public string SputumSmearDate { get; set; }
        public string GeneXpertDate { get; set; }
        public string HivExamDate { get; set; }
        public string XrayExamDate { get; set; }
        public string ArtStartedOn { get; set; }
        public string CptStartedOn { get; set; }

        public IEnumerable<SelectListItem> Facilities { get; set; }
        public IEnumerable<SelectListItem> Regimens { get; set; }
        public IEnumerable<SelectListItem> DotsBy { get; set; }
        public IEnumerable<SelectListItem> Referees { get; set; }

        public IEnumerable<SelectListItem> SputumSmearItems { get; set; }
        public IEnumerable<SelectListItem> GeneXpertItems { get; set; }
        public IEnumerable<SelectListItem> HivExamItems { get; set; }
        public IEnumerable<SelectListItem> XrayExamItems { get; set; }
        public IEnumerable<SelectListItem> ARTItems { get; set; }
        public IEnumerable<SelectListItem> CPTItems { get; set; }
        public IEnumerable<SelectListItem> BoolOpts { get; set; }

        public PatientRegimen Regimen { get; set; }
        public PatientExamination Examination { get; set; }

        public PatientIntakeViewModel() {
            Program = new PatientProgram();
            Patient = new Patient();
            Regimen = new PatientRegimen();
            Examination = new PatientExamination();

            RegimenStartedOn = DateTime.Now.ToString("d MMMM, yyyy");
            SputumSmearDate = DateTime.Now.ToString("d MMMM, yyyy");
            GeneXpertDate = DateTime.Now.ToString("d MMMM, yyyy");
            HivExamDate = DateTime.Now.ToString("d MMMM, yyyy");
            XrayExamDate = DateTime.Now.ToString("d MMMM, yyyy");
            ArtStartedOn = DateTime.Now.ToString("d MMMM, yyyy");
            CptStartedOn = DateTime.Now.ToString("d MMMM, yyyy");

            InitiazeBoolOpts();
        }

        private void InitiazeBoolOpts()
        {
            BoolOpts = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "YES" },
                new SelectListItem { Value = "0", Text = "NO" }
            };
        }
    }
}
