using System;
using Microsoft.AspNetCore.Mvc;

using EtbSomalia.Models;
using EtbSomalia.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EtbSomalia.Controllers
{
    public class PatientController : Controller
    {
        [BindProperty]
        public RegistrationAddPatientViewModel PatientAddModel { get; set; }

        // GET: /<controller>/
        [Route("registration/add")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("registration/intake/{id}")]
        public IActionResult Intake(Int64 Id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterNewPatient()
        {


            return LocalRedirect("/registration/intake/12" );
        }
    }
}
