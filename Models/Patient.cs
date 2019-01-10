using System;
using EtbSomalia.Extensions;

namespace EtbSomalia.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public Person Person { get; set; }

        public Patient()
        {
            Id = 0;
            Person = new Person();
        }

        public string GetName() {
            return Person.Name;
        }

        public string GetAge(){
            int age = DateTime.Now.Year - Person.DateOfBirth.Year;
            if (Person.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            int mnth = Convert.ToInt32(DateTime.Now.Subtract(Person.DateOfBirth).Days / (365.25 / 12));
            int days = Convert.ToInt32((DateTime.Now - Person.DateOfBirth).TotalDays);

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
            int age = DateTime.Now.Year - Person.DateOfBirth.Year;
            if (Person.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            return age;
        }

        public Patient Save(){
            Person.Save();

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Patient(pt_person) output INSERTED.pt_idnt VALUES (" + Person.Id + ")");

            return this;
        }
    }
}
