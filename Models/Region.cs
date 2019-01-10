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
    }
}
