using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class ReceiptDetails
    {
        public long Id { get; set; }
        public DrugReceipt drugReceipt { get; set; }
        public DrugBatches drugBatches { get; set; }
        public long Quantity { get; set; }
    }
}
