using System;
using System.Collections.Generic;
using EtbSomalia.Models;


namespace EtbSomalia.ViewModel
{
    public class RegistrationAddPatientViewModel
    {
        public Patient Patient { get; set; }
        public List<Gender> Gender { get; set; }
        public String DateOfBirth { get; set; }
        public PersonAddress Address { get; set; }

        public RegistrationAddPatientViewModel()
        {
            Address = new PersonAddress();
            Patient = new Patient();
            Gender = new List<Gender>();
            DateOfBirth = "";

            InitializeGender();
        }

        private void InitializeGender(){
            Gender.Add(new Gender("Male"));
            Gender.Add(new Gender("Female"));
        }
    }
}
