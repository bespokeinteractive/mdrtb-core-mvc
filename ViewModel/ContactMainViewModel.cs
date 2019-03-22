using System;
using System.Collections.Generic;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class ContactMainViewModel
    {
        public Contacts Contact { get; set; }
        public List<ContactsExamination> Examinations { get; set; }

        public ContactMainViewModel() {
            Contact = new Contacts();
            Examinations = new List<ContactsExamination>();
        }
    }
}
