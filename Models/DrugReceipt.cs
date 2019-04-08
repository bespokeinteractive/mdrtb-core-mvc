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
        public string DateString { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public DrugReceipt() {
            Id = 0;
            DateString = "";
            Number = "";
            Description = "";
            Date = DateTime.Now;
            CreatedBy = new Users();

        }

    }
}
