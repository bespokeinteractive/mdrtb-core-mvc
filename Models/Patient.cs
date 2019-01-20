using System;
using EtbSomalia.Extensions;

namespace EtbSomalia.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public Person Person { get; set; }

        public Patient() {
            Id = 0;
            Person = new Person();
        }

        public Patient(long idnt) : this() {
            Id = idnt;
        }

        public string GetName() {
            return Person.Name;
        }

        public string GetAge(){
            return Person.GetAge();
        }

        public int GetAgeInYears() {
            return Person.GetAgeInYears();
        }

        public Patient Save(){
            Person.Save();

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Patient(pt_person) output INSERTED.pt_idnt VALUES (" + Person.Id + ")");

            return this;
        }
    }
}
