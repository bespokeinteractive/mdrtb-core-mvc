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
        public int Dead { get; set; }
        public DateTime? DiedOn { get; set; }
        public Concept CauseOfDeath { get; set; }

        public Patient() {
            Id = 0;
            Uuid = "";
            Person = new Person();
            Dead = 0;
            CauseOfDeath = new Concept();
        }
        
        public Patient(long idnt) : this() {
            Id = idnt;
        }
        
        public Patient(string uuid) : this() {
            Uuid = uuid;
        }

        public bool isDead() {
            if (Dead.Equals(1))
                return true;
            return false;
        }

        public void isDead(bool opts) {
            if (opts)
                Dead = 1;
            else
                Dead = 0;
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
            if (string.IsNullOrEmpty(Uuid))
                Uuid = new PatientService().GetPatientUuid(this.Id);
            return Uuid;
        }

        public Patient Save(){
            Person.Save();

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Patient(pt_person) output INSERTED.pt_idnt VALUES (" + Person.Id + ")");

            return this;
        }

        public void Update() {
            new PatientService().UpdatePatient(this);
        }
    }
}
