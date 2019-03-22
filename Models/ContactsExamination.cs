using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class ContactsExamination
    {
        public long Id { get; set; }
        public Contacts Contact { get; set; }

        public bool Cough { get; set; }
        public bool Fever { get; set; }
        public bool WeightLoss { get; set; }
        public bool NightSweat { get; set; }

        public Concept LTBI { get; set; }
        public Concept SputumSmear { get; set; }
        public Concept GeneXpert { get; set; }
        public Concept XrayExam { get; set; }

        public string PreventiveTherapy { get; set; }
        public DateTime NextScreening { get; set; }
        public DateTime AddedOn { get; set; }
        public Users AddedBy { get; set; }

        public ContactsExamination()
        {
            Id = 0;
            Contact = new Contacts();

            Cough = false;
            Fever = false;
            WeightLoss = false;
            NightSweat = false;

            LTBI = new Concept();
            SputumSmear = new Concept();
            GeneXpert = new Concept();
            XrayExam = new Concept();

            PreventiveTherapy = "";
            NextScreening = DateTime.Now.AddMonths(6);

            AddedOn = DateTime.Now;
            AddedBy = new Users();
        }

        public ContactsExamination Save(HttpContext context) {
            return new PatientService().SaveContactsExamination(this);
        }
    }
}
