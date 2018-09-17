using System;
namespace EtbSomalia.Models
{
    public class Program
    {
        public Int64 Id { get; set; }
        public Concept Concept { get; set; }

        public Program()
        {
            Id = 0;
            Concept = new Concept();
        }
    }
}
