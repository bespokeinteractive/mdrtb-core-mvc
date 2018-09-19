using System;
using EtbSomalia.Extensions;

namespace EtbSomalia.Models
{
    public class Person
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean AgeEstimate { get; set; }
        public Person()
        {
            Id = 0;
            Name = "";
            Gender = "";
            DateOfBirth = DateTime.Now;
            AgeEstimate = false;
        }

        public Person Save(){
            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Person(ps_name, ps_gender, ps_dob, ps_estimate) output INSERTED.ps_idnt VALUES ('" + Name + "', '" + Gender + "', getdate(), 1)");

            return this;

        }
    }
}
