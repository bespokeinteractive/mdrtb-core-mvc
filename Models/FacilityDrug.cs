using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models 
{
    public class FacilityDrug {
        public long Id { get; set; }
        public Facility Facility { get; set; }
        public Drug Drug { get; set; }
        public int Reorder { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }

        public FacilityDrug() {
            Id = 0;
            Reorder = 0;
            Facility = new Facility();
            Drug = new Drug();
            CreatedBy = new Users();

        }
    }
}
