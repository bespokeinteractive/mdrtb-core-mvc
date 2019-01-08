using System;
namespace EtbSomalia.Models
{
    public class Regimen
    {
        public long Id { get; set; }
        public Programs Program { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Regimen() {
            Id = 0;
            Program = new Programs(1);
            Name = "";
            Description = "";
        }

        public Regimen(long idnt) : this() {
            Id = idnt;
        }
    }
}
