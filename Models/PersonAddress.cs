using System;
using EtbSomalia.Extensions;


namespace EtbSomalia.Models
{
    public class PersonAddress
    {
        public Int64 Id { get; set; }
        public Person Person { get; set; }
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

        public PersonAddress Save(){
            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO PersonAddress(pa_person, pa_default, pa_telephone, pa_address) output INSERTED.pa_idnt VALUES ('" + Person.Id + "', 1, '" + Telephone + "', '" + Address + "')");

            return this;
        }

        public PersonAddress Update()
        {
            return this;
        }
    }
}
