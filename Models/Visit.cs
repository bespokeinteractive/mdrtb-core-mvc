using System;
namespace EtbSomalia.Models
{
    public class Visit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Visit() {
            Id = 0;
            Name = "";
            Description = "";
        }

        public Visit(long idnt) : this() {
            Id = idnt;
        }

        public Visit (long idnt, string name):this() {
            Id = idnt;
            Name = name;
        }
    }
}
