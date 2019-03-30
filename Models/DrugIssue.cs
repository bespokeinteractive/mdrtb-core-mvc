using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugIssue
    {
        public long Id { get; set; }
        public long Facility { get; set; }
        public DateTime Date { get; set; }
        public string Account { get; set; }
        public string  Description { get; set; }
        public long Created_By { get; set; }
        public DateTime Created_On { get; set; }
    }
}
