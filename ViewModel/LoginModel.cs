using System;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class LoginModel
    {
        public Users User { get; set; }
        public string Message { get; set; }
        public string ReturnUrl { get; set; }

        public LoginModel()
        {
            User = new Users();
            Message = "";
            ReturnUrl = "/";
        }
    }
}
