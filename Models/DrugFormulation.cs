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
        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }

        public DrugFormulation() {
            Id = 0;
            Name = "";
            Dosage = "";
            Description = "";
            CreatedBy = new Users();
        }

    }
}
