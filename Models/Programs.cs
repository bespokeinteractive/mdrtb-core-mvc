using System;
namespace EtbSomalia.Models
{
    public class Programs
    {
        public Int64 Id { get; set; }
        public string Description { get; set; }
        public Concept Concept { get; set; }

        public Programs() {
            Id = 0;
            Concept = new Concept();
        }

        public Programs(Int64 idnt) : this() {
            Id = idnt;
        }

        public Programs(Int64 idnt, string description) : this() {
            Id = idnt;
            Description = description;
        }
    }
}
