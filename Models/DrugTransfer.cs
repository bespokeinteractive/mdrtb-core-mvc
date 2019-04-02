using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models {
    public class DrugTransfer {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long Status { get; set; }
        public Facility Location { get; set; }
        public Facility Destination { get; set; }
        public string Description { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users AcceptedBy { get; set; }
        public DateTime AcceptedOn { get; set; }
        public string AcceptNotes { get; set; }
        public Users RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectReason { get; set; }
    }
}
