using System;
using Microsoft.AspNetCore.Mvc;

using EtbSomalia.Models;
using EtbSomalia.ViewModel;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        [BindProperty]
        public PatientRegisterViewModel ViewModel { get; set; }
        public PatientProgram PatientProgram { get; set; }

        // GET: /<controller>/
        [Route("registration/add")]
        public IActionResult Register(PatientRegisterViewModel model, ConceptService csvc, DashboardService dashboard)
        {
            model.Facilities = dashboard.GetFacilitiesIEnumerable();
            model.TBCategory = csvc.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CATEGORY));
            model.TBTypes = csvc.GetConceptAnswersIEnumerable(new Concept(Constants.TB_TYPE));
            model.TBConfirmation = csvc.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CONFIRMATION));
            model.ResistanceProfile = csvc.GetConceptAnswersIEnumerable(new Concept(Constants.RESISTANCE_PROFILE));

            return View(model);
        }

        [Route("registration/intake/{id}")]
        public IActionResult Intake(Int64 Id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterNewPatient()
        {
            Patient patient = ViewModel.Patient;
            patient.Person.DateOfBirth = DateTime.ParseExact(ViewModel.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            patient.Save();

            PersonAddress address = ViewModel.Address;
            address.Person = patient.Person;
            address.Save();

            PatientProgram program = new PatientProgram(patient);
            program.DateEnrolled = DateTime.Parse(ViewModel.DateEnrolled);
            program.Facility = new Facility(ViewModel.FacilityId);
            program.Type = new Concept(ViewModel.TypeId);
            program.Confirmation = new Concept(ViewModel.ConfirmationId);
            program.Program = new Concept(ViewModel.ProgramId);
            program.Category = new Concept(ViewModel.CategoryId);
            program.Save();
            
            return LocalRedirect("/registration/intake/" + patient.Id);
        }
        
        [AllowAnonymous]
        public string GetBirthdateFromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            try {
                return DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            }
            catch(Exception){
                System.Diagnostics.Debug.WriteLine("...");
            }

            if (IsNumber(value)){
                return DateTime.Now.AddYears(0 - int.Parse(value)).ToString("dd/MM/yyyy");
            }

            if (value.Contains('y')){
                value = value.Replace("y", string.Empty);

                if (IsNumber(value))
                    return DateTime.Now.AddYears(0 - int.Parse(value)) .ToString("dd/MM/yyyy");
                else 
                    return "";
            }
            else if(value.Contains('m')){
                value = value.Replace("m", string.Empty);

                if (IsNumber(value))
                    return DateTime.Now.AddMonths(0 - int.Parse(value)).ToString("dd/MM/yyyy");
                else
                    return "";
            }
            else if (value.Contains('w')){
                value = value.Replace("w", string.Empty);

                if (IsNumber(value))
                    return DateTime.Now.AddDays(0 - (int.Parse(value) * 7)).ToString("dd/MM/yyyy");
                else
                    return "";
            }
            else if (value.Contains('d')){
                value = value.Replace("d", string.Empty);

                if (IsNumber(value))
                    return DateTime.Now.AddDays(0 - int.Parse(value)).ToString("dd/MM/yyyy");
                else
                    return "";
            }

            return "";
        }



        public Boolean IsNumber(String s)
        {
            Boolean value = true;
            foreach (Char c in s.ToCharArray())
            {
                value = value && Char.IsDigit(c);
            }

            return value;
        }
    }
}
