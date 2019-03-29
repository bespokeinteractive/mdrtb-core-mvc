using System;
namespace EtbSomalia.Models
{
    public class Region
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }

        public Region() {
            Id = 0;
            Name = "";
            Prefix = "";
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
