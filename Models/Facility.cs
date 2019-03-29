using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

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
        public int Count { get; set; }
        public int Outcomes { get; set; }
        public DateTime LastRecord { get; set; }
        public Region Region { get; set; }
        public Agency Agency { get; set; }

        public Facility() {
            Id = 0;
            Void = false;
            Status = "";
            Prefix = "";
            Name = "";
            Description = "";
            Count = 0;
            Outcomes = 0;
            LastRecord = new DateTime(1900, 1, 1);

            Region = new Region();
            Agency = new Agency();
        }

        public Facility(long idnt) : this() {
            Id = idnt;
        }

        public Facility Save(HttpContext context) {
            return new CoreService(context).SaveFacility(this);
        }

        public void Delete() {
            new CoreService().DeleteFacility(this);
        }
    }
}
