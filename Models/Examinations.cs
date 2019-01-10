using System;
namespace EtbSomalia.Models
{
    public class Examinations
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string LabNo { get; set; }
        public Visit Visit { get; set; }
        public Concept Result { get; set; }

        public Examinations() {
            Id = 0;
            Date = DateTime.Now;
            Name = "";
            LabNo = "";
            Visit = new Visit();
            Result = new Concept();
        }
    }
}
