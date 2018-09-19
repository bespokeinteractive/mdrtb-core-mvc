using System;

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

            //Save patient

            return this;
        }
    }
}
