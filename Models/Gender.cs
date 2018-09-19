using System;
namespace EtbSomalia.Models
{
    public class Gender
    {
        public String Name { get; set; }

        public Gender()
        {
            Name = "";
        }

        public Gender(String name)
        {
            Name = name;
        }
    }
}
