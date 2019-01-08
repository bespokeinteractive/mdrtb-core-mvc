using System;
namespace EtbSomalia.Models
{
    public class Users
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public bool ToChange { get; set; }
        public long AdminLevel { get; set; }
        public string AccessLevel { get; set; }
        public string Message { get; set; }

        public Users()
        {
            Id = 0;
            Name = "";
            Email = "";
            Username = "";
            Password = "";
            Enabled = true;
            ToChange = false;
            AdminLevel = 0;
            AccessLevel = "";

            Message = "";
        }

        public Users(long idnt) : this() {
            Id = idnt;
        }

        public Users(long idnt, string name) : this() {
            Id = idnt;
            Name = name;
        }
    }
}
