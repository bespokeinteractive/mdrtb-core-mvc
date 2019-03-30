using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class Drug
    {
        public long Id { get; set; }
        public String Initial { get; set; }
        public String Name { get; set; }
        public String Units { get; set; }
        public DrugCategory Category { get; set; }
        public DrugFormulation Formulation { get; set; }
        public long Created_By { get; set; }
        public DateTime Created_On { get; set; }
        public String Description { get; set; }


    }
}
