using System;

namespace EtbSomalia.Models
{
    public class Roles
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Roles() {
            Id = 0;
            Name = "";
            Description = "";
        }

        public Roles(long idnt) : this() {
            Id = idnt;
        }

        public Roles(long idnt, string name) : this() {
            Id = idnt;
            Name = name;
        }
    }
}
