using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugTransferDetails
    {
        public long Id { get; set; }
        public DrugTransfer Transfer { get; set; }
        public DrugBatches Batches { get; set; }
        public long Quantity { get; set; }
        public string Comments { get; set; }

        public DrugTransferDetails() {
            Id = 0;
            Quantity = 0;
            Comments = "";
            Transfer = new DrugTransfer();
            Batches = new DrugBatches();
        }
    }
}
