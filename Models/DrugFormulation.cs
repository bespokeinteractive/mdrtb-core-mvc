using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugFormulation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Description { get; set; }
        public DateTime Created_On { get; set; }
        public long Created_By { get; set; }


    }
}
