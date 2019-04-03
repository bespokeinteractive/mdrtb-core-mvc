using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugIssue
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Facility Facility { get; set; }
        public string Account { get; set; }
        public string  Description { get; set; }
        public Users CreatedBy { get; set; }
        public DateTime Created_On { get; set; }

        public DrugIssue() {
            Id = 0;            
            Account = "";
            Description = "";
            Facility = new Facility();
            CreatedBy = new Users();

        }
    }
}
