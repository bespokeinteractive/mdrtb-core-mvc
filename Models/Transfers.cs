using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class Transfers
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public PatientProgram Program { get; set; }
        public Facility Facility { get; set; }
        public int Status { get; set; }
        public int Flag { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users AcceptedBy { get; set; }
        public DateTime AcceptedOn { get; set; }
        public Users RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectedReason { get; set; }
        public string Description { get; set; }

        public Transfers() {
            Id = 0;
            Date = DateTime.Now;
            Program = new PatientProgram();
            Facility = new Facility();
            Status = 0;
            Flag = 0;

            CreatedBy = new Users();
            AcceptedBy = new Users();
            RejectedBy = new Users();

            CreatedOn = DateTime.Now;
            AcceptedOn = DateTime.Now;
            RejectedOn = DateTime.Now;

            RejectedReason = "";
            Description = "";
        }

        public Transfers(long idnt) : this() {
            Id = idnt;
        }

        public Transfers Save(HttpContext Context) {
            return new PatientService(Context).SaveTransfers(this);
        }
    }
}
