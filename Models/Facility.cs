using System;
namespace EtbSomalia.Models
{
    public class Facility
    {
        public long Id { get; set; }
        public bool Void { get; set; }
        public string Status { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Region Region { get; set; }
        public Agency Agency { get; set; }

        public Facility() {
            Id = 0;
            Void = false;
            Status = "";
            Prefix = "";
            Name = "";
            Description = "";

            Region = new Region();
            Agency = new Agency();
        }

        public Facility(Int64 idx) : this() {
            Id = idx;
        }
    }
}
