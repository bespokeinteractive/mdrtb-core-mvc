using System;
using System.Collections.Generic;
using EtbSomalia.DataModel;
using EtbSomalia.Models;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.ViewModel
{
    public class AccountUsersAddEditViewModel
    {
        private readonly CoreService service = new CoreService();
        private readonly UserService users = new UserService();

        public Users User { get; set; }
        public string Facility { get; set; }
        public int Agency { get; set; }
        public int Region { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Agencies { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }
        public List<UsersFacility> Facilities { get; set; }

        public AccountUsersAddEditViewModel() {
            User = new Users();
            Facility = "";
            Agency = 0;
            Region = 0;

            Roles = service.GetRolesIEnumerable();
            Agencies = service.GetAgenciesIEnumerable();
            Regions = service.GetRegionsIEnumerable();
            Facilities = users.GetUsersFacilitiesAll(User);
        }
    }
}
