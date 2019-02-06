using System;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class AccountUsersViewModel
    {
        public Users User { get; set; }

        public AccountUsersViewModel() {
            User = new Users();
        }
    }
}
