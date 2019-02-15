using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class Contacts
    {
        public long Id { get; set; }
        public string Uuid { get; set; }
        public string Identifier { get; set; }

        public Patient Patient { get; set; }
        public Person Person { get; set; }
        public PatientProgram Index { get; set; }

        public Concept Status { get; set; }
        public Concept Location { get; set; }
        public Concept Relation { get; set; }
        public Concept Proximity { get; set; }
        public Concept DiseaseAfter { get; set; }
        public Concept PrevouslyTreated { get; set; }

        public DateTime NextVisit { get; set; }
        public DateTime ExposedOn { get; set; }
        public DateTime AddedOn { get; set; }
        public Users AddedBy { get; set; }

        public string Notes { get; set; }

        public Contacts() {
            Id = 0;
            Uuid = "";
            Identifier = "";
            Person = new Person();
            Index = new PatientProgram();

            Status = new Concept();
            Location = new Concept();
            Relation = new Concept();
            Proximity = new Concept();
            DiseaseAfter = new Concept();
            PrevouslyTreated = new Concept();

            NextVisit = DateTime.Now;
            ExposedOn = DateTime.Now;
            AddedOn = DateTime.Now;
            AddedBy = new Users();
        }

        public Contacts(long idnt) : this() {
            Id = idnt;
        }

        public Contacts(string uuid) : this() {
            Uuid = uuid;
        }

        public string GetName() {
            return Person.Name;
        }

        public string GetRiskFactor() {
            int age = Person.GetAgeInYears();

            if (age <= 5)
                return "Under 5";
            else if (age >= 65)
                return "Over 65";
            return "None";
        }

        public string GetAge() {
            return Person.GetAge();
        }

        public int GetAgeInYears() {
            return Person.GetAgeInYears();
        }

        public DateTime GetLastScreening() {
            return new PatientService().GetContactsLastScreening(this);
        }

        public void GenerateIdentifier()
        {
            Random random = new Random();
            int rIdnt = random.Next(1, 100);

            Identifier = "CTX/" + Index.Program.Id + "/" + Convert.ToInt32(random.NextDouble() * 100).ToString().PadLeft(4, '0');
        }

        public Contacts Register(HttpContext context)
        {
            this.GenerateIdentifier();
            Person.Save();

            return new PatientService(context).RegisterContact(this);
        }
    }
}
