using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class PatientExamination
    {
        public long Id { get; set; }

        public string LabNo { get; set; }
        public double Height { get; set; } 
        public double Weight { get; set; }
        public double MUAC { get; set; }
        public double BMI { get; set; }

        public PatientProgram Program { get; set; }
        public Visit Visit { get; set; }

        public Concept SputumSmear { get; set; }
        public Concept GeneXpert { get; set; }
        public Concept HivExam { get; set; }
        public Concept XrayExam { get; set; }

        public DateTime SputumSmearDate { get; set; }
        public DateTime GeneXpertDate { get; set; }
        public DateTime HivExamDate { get; set; }
        public DateTime XrayExamDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }
        public string Notes { get; set; }

        public PatientExamination() {
            Id = 0;

            LabNo = "";
            Height = 0;
            Weight = 0;
            MUAC = 0;
            BMI = 0;

            Program = new PatientProgram();
            Visit = new Visit();

            SputumSmear = new Concept();
            GeneXpert = new Concept();
            HivExam = new Concept();
            XrayExam = new Concept();

            SputumSmearDate = DateTime.Now;
            GeneXpertDate = DateTime.Now;
            HivExamDate = DateTime.Now;
            XrayExamDate = DateTime.Now;

            CreatedOn = DateTime.Now;
            CreatedBy = new Users();
            Notes = "";
        }

        public PatientExamination Save(HttpContext context) {
            CoreService service = new CoreService(context);
            return service.SavePatientExamination(this); 
        }
    }
}
