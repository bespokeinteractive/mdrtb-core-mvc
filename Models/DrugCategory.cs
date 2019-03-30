using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtbSomalia.Models
{
    public class DrugCategory
    {

        public long Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public long Created_By { set; get; }
        public DateTime Created_On { set; get; }



    }
}
