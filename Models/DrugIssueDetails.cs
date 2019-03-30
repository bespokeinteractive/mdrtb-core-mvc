using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugIssueDetails
    {
        public long Id { get; set; }
        public DrugIssue drugIssue { get; set; }
        public DrugBatches drugBatches { get; set; }
        public long Quantity { get; set; }
        public string Description { get; set; }
    }
}
