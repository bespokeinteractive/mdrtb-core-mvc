using System;
using EtbSomalia.Extensions;

namespace EtbSomalia.Models
{
    public class Patient
    {
        public Int64 Id { get; set; }
        public Person Person { get; set; }

        public Patient()
        {
            Id = 0;
            Person = new Person();
        }

        public Patient Save(){
            Person.Save();

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO Patient(pt_person) output INSERTED.pt_idnt VALUES (" + Person.Id + ")");

            return this;
        }
    }
}
