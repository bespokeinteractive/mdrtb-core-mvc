using System;
namespace EtbSomalia.Models
{
    public class Agency
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Agency() {
            Id = 0;
            Name = "";
            Description = "";
        }

        public Agency(long idnt) : this() {
            Id = idnt;
        }

        public Agency(long idnt, string name) : this() {
            Id = idnt;
            Name = name;
        }
    }
}
