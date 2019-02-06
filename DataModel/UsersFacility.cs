using System;
namespace EtbSomalia.DataModel
{
    public class UsersFacility
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }

        public UsersFacility() {
            Id = 0;
            Status = 0;

            Prefix = "";
            Name = "";
            Region = "";
        }
    }
}
