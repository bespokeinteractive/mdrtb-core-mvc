using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugBatches
    {
        public long Id { get; set; }
        public Drug Drug { get; set; }
        public string Company { get; set; }
        public string Supplier { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }
    }
}
