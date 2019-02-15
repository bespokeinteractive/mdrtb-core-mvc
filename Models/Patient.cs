using System;
using EtbSomalia.Extensions;
using EtbSomalia.Services;

namespace EtbSomalia.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public string Uuid { get; set; }
        public Person Person { get; set; }

        public Patient() {
            Id = 0;
            Uuid = "";
            Person = new Person();
        }

        public Patient(long idnt) : this() {
            Id = idnt;
        }

        public Patient(string uuid) : this() {
            Uuid = uuid;
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

        public string GetUuid() {
            Uuid = new PatientService().GetPatientUuid(this.Id);
            return Uuid;
        }

        public Patient Save(){
            Person.Save();

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Patient(pt_person) output INSERTED.pt_idnt VALUES (" + Person.Id + ")");

            return this;
        }
    }
}
