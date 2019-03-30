using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugTransfer
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long Status { get; set; }
        public long Location { get; set; }
        public long Destination { get; set; }
        public string Description { get; set; }
        public long Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public long Accepted_By { get; set; }
        public DateTime Accepted_On { get; set; }
        public string Accepted_Notes { get; set; }
        public long Rejected_By { get; set; }
        public DateTime Rejected_On { get; set; }
        public string Rejected_Reason { get; set; }
    }
}
