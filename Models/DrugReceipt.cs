using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugReceipt
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string ReceiptNumber { get; set; }
        public string Description { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DrugReceipt() {
            Id = 0;
            ReceiptNumber = "";
            Description = "";
            CreatedBy = new Users();

        }

    }
}
