using System;
using System.Net.Mail;
using EtbSomalia.Extensions;
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
        public string LastSeen { get; set; }
        public DateTime AddedOn { get; set; }

        public Roles Role { get; set; }
        public Users AddedBy { get; set; }

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
            LastSeen = "N/A";
            AddedOn = DateTime.Now;

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

        public void ResetPassword() {
            this.Password = new CrytoUtilsExtensions().Encrypt("pass");
            this.UpdatePassword(1);

            if (!string.IsNullOrEmpty(Email)) {
                MailSendExtensions mail = new MailSendExtensions();
                mail.SendTo.Add(new MailAddress(Email, Name));
                
                string message = "Dear " + Name + System.Environment.NewLine + System.Environment.NewLine;
                message += "The password for your Account on EtbSomalia System has been reset. Your login credentials are as below" + System.Environment.NewLine;
                message += "URL: http://etbsomalia.worldvision.or.ke" + System.Environment.NewLine;
                message += "Username: " + Username + System.Environment.NewLine;
                message += "Password: pass" + System.Environment.NewLine + System.Environment.NewLine;
                message += "You will be prompted to change the password after the first login. Provide a password of your liking." + System.Environment.NewLine + System.Environment.NewLine;
                message += "Regards," + System.Environment.NewLine;
                message += "System Admin" + System.Environment.NewLine + System.Environment.NewLine;
                message += "P.S. This is a system generated Email. Do not respond to it.";
                
                mail.Message = message;
                mail.Send();
            }
        }

        public void EnableAccount(bool opts = true) {
            new UserService().EnableAccount(this, opts);
        }

        public void UpdatePassword(int changepw = 0) {
            new UserService().UpdateUserPassword(this, changepw);
        }

        public void UpdateLastAccess() {
            new UserService().UpdateLastAccess(this);
        }
    }
}
