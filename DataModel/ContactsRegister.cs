using System;
using EtbSomalia.Models;

namespace EtbSomalia.DataModel
{
    public class ContactsRegister
    {
        public Contacts Contact { get; set; }
        public ContactsExamination Examination { get; set; }

        public ContactsRegister() {
            Contact = new Contacts();
            Examination = new ContactsExamination();
        }
    }
}
