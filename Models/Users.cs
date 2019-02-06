using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

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
        public int AdminRole { get; set; }
        public int AdminLevel { get; set; }
        public string AccessLevel { get; set; }
        public string Message { get; set; }
        public Roles Role { get; set; }

        public Users() {
            Id = 0;
            Name = "";
            Email = "";
            Username = "";
            Password = "";
            Enabled = true;
            ToChange = false;
            AdminRole = 0;
            AdminLevel = 0;
            AccessLevel = "";
            Message = "";

            Role = new Roles();
        }

        public Users(long idnt) : this() {
            Id = idnt;
        }

        public Users(long idnt, string name) : this() {
            Id = idnt;
            Name = name;
        }

        public Users Save(HttpContext context) {
            return new UserService(context).SaveUser(this);
        }

        public void UpdatePassword() {
            new UserService().UpdateUserPassword(this);
        }

        public void UpdateLastAccess() {
            new UserService().UpdateLastAccess(this);
        }
    }
}
