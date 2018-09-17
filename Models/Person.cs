using System;
namespace EtbSomalia.Models
{
    public class Person
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean AgeEstimate { get; set; }
        public PersonAddress PersonAddress { get; set; }

        public Person()
        {
            Id = 0;
            Name = "";
            Gender = "";
            DateOfBirth = DateTime.Now;
            AgeEstimate = false;

            PersonAddress = new PersonAddress();
        }
    }
}
