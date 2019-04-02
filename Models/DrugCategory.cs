using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugCategory {

        public long Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public Users CreatedBy { set; get; }
        public DateTime CreatedOn { set; get; }
    }
}
