using System;
namespace EtbSomalia.Models
{
    public class Facility
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Facility()
        {
            Id = 0;
            Name = "";
        }

        public Facility(Int64 idx)
        {
            Id = idx;
            Name = "";
        }
    }
}
