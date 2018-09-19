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
            Person = new Person();
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
            //Id = conn.SqlServerUpdate("DECLARE @fac INT=" + Facility.Id + " , @norm INT=" + Item.Id + ", @val INT=" + Value + "; IF NOT EXISTS (SELECT nr_idnt FROM Norms WHERE nr_facility=@fac AND nr_norm=@norm) BEGIN INSERT INTO Norms(nr_facility, nr_norm, nr_available) output INSERTED.nr_idnt VALUES (@fac, @norm, @val) END ELSE BEGIN UPDATE Norms SET nr_available=@val output INSERTED.nr_idnt WHERE nr_facility=@fac AND nr_norm=@norm END");

            return this;
        }

        public PersonAddress Update()
        {
            return this;
        }
    }
}
