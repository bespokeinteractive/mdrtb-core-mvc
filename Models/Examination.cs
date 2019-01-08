using System;
namespace EtbSomalia.Models
{
    public class Examination
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Examination() {
            Id = 0;
            Name = "";
            Description = "";
        }

        public Examination(long idnt) : this()
        {
            Id = idnt;
        }
    }
}
