using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugBatches
    {
        public long Id { get; set; }
        public  Drug Drug { get; set; }

        public string Company { get; set; }
        public string Supplier { get; set; }
        public DateTime Manufacture { get; set; }
        public DateTime Expiry { get; set; }
        public String Notes { get; set; }
        public DateTime Created_On { get; set; }
        public long Created_By { get; set; }
    }
}
