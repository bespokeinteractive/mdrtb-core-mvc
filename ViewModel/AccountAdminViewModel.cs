using System;
using EtbSomalia.Services;

namespace EtbSomalia.ViewModel
{
    public class AccountAdminViewModel
    {
        public int Users { get; set; }
        public int Agencies { get; set; }
        public int Regions { get; set; }
        public int Facilities { get; set; }

        public AccountAdminViewModel() {
            Users = 0;
            Agencies = 0;
            Regions = 0;
            Facilities = 0;

            Initialize();
        }

        public void Initialize() {
            new CoreService().InitializeAdminModel(this);
        }
    }
}
