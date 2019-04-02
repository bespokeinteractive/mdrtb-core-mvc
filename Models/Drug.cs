using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class Drug {
        public long Id { get; set; }
        public string Initial { get; set; }
        public string Name { get; set; }
        public DrugCategory Category { get; set; }
        public DrugFormulation Formulation { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }
    }
}
