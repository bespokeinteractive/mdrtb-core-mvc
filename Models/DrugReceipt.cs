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
        public string Receipt_Number { get; set; }
        public string Description { get; set; }
        public long Created_By { get; set; }
        public DateTime Created_On { get; set; }

    }
}
