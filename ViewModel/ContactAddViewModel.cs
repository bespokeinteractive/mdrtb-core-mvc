using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class ContactAddViewModel
    {
        public Contacts Contact { get; set; }
        public ContactsExamination Examination { get; set; }
        public PatientProgram Program { get; set; }


        public IEnumerable<SelectListItem> Gender { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        public IEnumerable<SelectListItem> Location { get; set; }
        public IEnumerable<SelectListItem> Relation { get; set; }
        public IEnumerable<SelectListItem> Proximity { get; set; }
        public IEnumerable<SelectListItem> PreviouslyTreated { get; set; }
        public IEnumerable<SelectListItem> DeseaseAfterExposure { get; set; }

        public IEnumerable<SelectListItem> LatentTBI { get; set; }
        public IEnumerable<SelectListItem> SputumSmearItems { get; set; }
        public IEnumerable<SelectListItem> GeneXpertItems { get; set; }
        public IEnumerable<SelectListItem> XrayExamItems { get; set; }

        public string DateOfBirth { get; set; }
        public string NextVisit { get; set; }

        public ContactAddViewModel()
        {
            Contact = new Contacts();
            Program = new PatientProgram();
            Examination = new ContactsExamination();

            Contact.Person.Address = new PersonAddress();

            DateOfBirth = "";
            NextVisit = DateTime.Now.AddMonths(6).ToString("d MMM, yyyy");

            Gender = new PatientService().InitializeGender();

        }
    }
}
