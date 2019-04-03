using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugIssueDetails
    {
        public long Id { get; set; }
        public DrugIssue Issue { get; set; }
        public DrugBatches Batches { get; set; }
        public long Quantity { get; set; }
        public string Description { get; set; }

        public DrugIssueDetails() {
            Id = 0;
            Quantity = 0;
            Description = "";
            Issue = new DrugIssue();
            Batches = new DrugBatches();

        }
    }
}
