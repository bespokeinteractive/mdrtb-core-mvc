using System;
namespace EtbSomalia.Models
{
    public class PersonAddress
    {
        public Int64 Id { get; set; }
        public Boolean Default { get; set; }
        public String Mobile { get; set; }
        public String Telephone { get; set; }
        public String Address { get; set; }
        public String PostalCode { get; set; }
        public String Village { get; set; }
        public String State { get; set; }
        public String County { get; set; }

        public PersonAddress()
        {
            Id = 0;
            Default = false;
            Mobile = "";
            Telephone = "";
            Address = "";
            PostalCode = "";
            Village = "";
            State = "";
            County = "";
        }
    }
}
