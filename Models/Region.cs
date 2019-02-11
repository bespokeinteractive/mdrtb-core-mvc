using System;
namespace EtbSomalia.Models
{
    public class Region
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Region() {
            Id = 0;
            Name = "";
        }

        public Region(long idnt) : this() {
            Id = idnt;
        }

        public Region(long idnt, string name) : this() {
            Id = idnt;
            Name = name;
        }
    }
}
