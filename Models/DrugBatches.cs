using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugBatches {
        public long Id { get; set; }
        public string BatchNo { get; set; }
        public string Company { get; set; }
        public string Supplier { get; set; }
        public string Manufacture { get; set; }
        public string Expiry { get; set; }
        public double Available { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string Notes { get; set; }
        public Drug Drug { get; set; }
        public Facility Facility { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }

        public DrugBatches() {
            Id = 0;
            BatchNo = "";
            Company = "";
            Supplier = "";
            Notes = "";
            Expiry = "";
            Manufacture = "";
            Available = 0;
            CreatedBy = new Users();
            Drug = new Drug();
            Facility = new Facility();
        }
    }
}
