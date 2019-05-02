using System;
using EtbSomalia.Extensions;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool AgeEstimate { get; set; }
        public PersonAddress Address { get; set; }

        public Person() {
            Id = 0;
            Name = "";
            Gender = "";
            DateOfBirth = DateTime.Now;
            AgeEstimate = false;
        }

        public string GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            int mnth = Convert.ToInt32(DateTime.Now.Subtract(DateOfBirth).Days / (365.25 / 12));
            int days = Convert.ToInt32((DateTime.Now - DateOfBirth).TotalDays);

            if (age > 2)
                return age + "yrs";
            if (mnth > 2)
                return mnth + "mnths";
            if (days == 1)
                return "1 day";
            return days + "days";
        }

        public int GetAgeInYears()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            return age;
        }

        public PersonAddress GetPersonAddress() {
            return new PatientService().GetPersonAddress(this);
        }

        public Person Save(){
            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Person(ps_name, ps_gender, ps_dob, ps_estimate) output INSERTED.ps_idnt VALUES ('" + Name + "', '" + Gender + "', '" + DateOfBirth.Date + "', 1)");

            return this;
        }

        public Person Save(HttpContext context) {
            return new PatientService(context).SavePerson(this);
        }
    }
}
