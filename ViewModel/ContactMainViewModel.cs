using System;
using System.Collections.Generic;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class ContactMainViewModel
    {
        public Contacts Contact { get; set; }

        public ContactMainViewModel()
        {
            Contact = new Contacts();
        }
    }
}
