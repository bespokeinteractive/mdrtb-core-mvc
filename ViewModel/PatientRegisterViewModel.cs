using System;
using System.Collections.Generic;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class PatientRegisterViewModel
    {
        public Patient Patient { get; set; }
        public PatientProgram Program { get; set; }

        public IEnumerable<SelectListItem> Gender { get; set; }
        public IEnumerable<SelectListItem> Facilities { get; set; }
        public IEnumerable<SelectListItem> TBTypes { get; set; }
        public IEnumerable<SelectListItem> TBConfirmation { get; set; }
        public IEnumerable<SelectListItem> TBCategory { get; set; }
        public IEnumerable<SelectListItem> ResistanceProfile { get; set; }

        public String DateOfBirth { get; set; }
        public String DateEnrolled { get; set; }

        public Int64 FacilityId { get; set; }
        public Int64 TypeId { get; set; }
        public Int64 ConfirmationId { get; set; }
        public Int64 ProgramId { get; set; }
        public Int64 CategoryId { get; set; }
        public PersonAddress Address { get; set; }

        public PatientRegisterViewModel()
        {
            Address = new PersonAddress();
            Patient = new Patient();
            Program = new PatientProgram();

            Gender = InitializeGender();

            Facilities = new List<SelectListItem>();
            TBCategory = new List<SelectListItem>();
            TBTypes = new List<SelectListItem>();
            TBConfirmation = new List<SelectListItem>();
            ResistanceProfile = new List<SelectListItem>();

            DateOfBirth = "";
            DateEnrolled = DateTime.Now.ToString("d MMMM, yyyy");
        }

        private List<SelectListItem> InitializeGender(){
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem("Male", "male"));
            gender.Add(new SelectListItem("Female", "female"));

            return gender;
        }
    }
}
