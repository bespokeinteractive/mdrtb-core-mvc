using System;
using Microsoft.AspNetCore.Mvc;

using EtbSomalia.Models;
using EtbSomalia.ViewModel;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Collections.Generic;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        [BindProperty]
        public PatientRegisterViewModel RegisterModel { get; set; }

        [BindProperty]
        public PatientIntakeViewModel IntakeModel { get; set; }

        // GET: /<controller>/
        [Route("registration/add")]
        public IActionResult Register(PatientRegisterViewModel model, ConceptService cs)
        {
            CoreService service = new CoreService(HttpContext);
            model.Facilities = service.GetFacilitiesIEnumerable();

            model.TBCategory = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CATEGORY));
            model.TBTypes = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_TYPE));
            model.TBConfirmation = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CONFIRMATION));
            model.ResistanceProfile = cs.GetConceptAnswersIEnumerable(new Concept(Constants.RESISTANCE_PROFILE));

            return View(model);
        }

        [Route("registration/intake/{idnt}")]
        public IActionResult Intake(long idnt, PatientIntakeViewModel model, PatientService ps, ConceptService cs)
        {
            CoreService core = new CoreService(HttpContext);
            model.Program = ps.GetPatientProgram(idnt);
            if (!model.Program.DotsBy.Id.Equals(0)) {
                return LocalRedirect("/patients/profile/" + model.Program.Patient.Id);
            }

            //Other Fields
            model.Patient = ps.GetPatient(model.Program.Patient.Id);
            model.Regimens = core.GetRegimensIEnumerable(model.Program.Program);
            model.DotsBy = cs.GetConceptAnswersIEnumerable(new Concept(Constants.DOTS_BY));
            model.Referees = cs.GetConceptAnswersIEnumerable(new Concept(Constants.REFERRED_BY));

            //Exam Options
            model.SputumSmearItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.SPUTUM_SMEAR));
            model.GeneXpertItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.GENE_XPERT));
            model.HivExamItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.HIV_EXAM));
            model.XrayExamItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.XRAY_EXAM));

            //HIV Options
            model.ARTItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.ART_STARTED_ON));
            model.CPTItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.CPT_STARTED_ON));

            model.ArtStartedOn = model.Program.ArtStartedOn.ToString("d MMMM, yyyy");
            model.CptStartedOn = model.Program.CptStartedOn.ToString("d MMMM, yyyy");

            //Facilities/Regimen
            model.Facilities = core.GetFacilitiesIEnumerable();
            model.Regimen = core.GetPatientRegimen(model.Program);
            if (model.Regimen is null) {
                model.Regimen = new PatientRegimen();
            }
            else {
                model.RegimenStartedOn = model.Regimen.StartedOn.ToString("d MMMM, yyyy");
            }

            //Patient Examination
            model.Examination = core.GetPatientExamination(model.Program, new Visit(1));
            if (model.Examination is null) {
                model.Examination = new PatientExamination();
            }
            else {
                model.SputumSmearDate = model.Examination.SputumSmearDate.ToString("d MMMM, yyyy");
                model.GeneXpertDate = model.Examination.GeneXpertDate.ToString("d MMMM, yyyy");
                model.HivExamDate = model.Examination.HivExamDate.ToString("d MMMM, yyyy");
                model.XrayExamDate = model.Examination.XrayExamDate.ToString("d MMMM, yyyy");
            }

            return View(model);
        }

        [Route("patients/profile/{idnt}")]
        public IActionResult Profile(long idnt, PatientProfileViewModel model, PatientService ps, CoreService core, long program = 0) {
            model.Patient = ps.GetPatient(idnt);

            if (program.Equals(0)) {
                model.Program = ps.GetPatientProgram(model.Patient);
            }
            else {
                model.Program = ps.GetPatientProgram(program);
            }

            if (model.Program.DotsBy.Id.Equals(0)){
                return LocalRedirect("/registration/intake/" + model.Program.Id);
            }

            model.Regimen = core.GetPatientRegimen(model.Program);
            model.Program.Facility = core.GetFacility(model.Program.Facility.Id);
            model.LatestVitals = ps.GetLatestVitals(model.Patient);
            model.Examinations = core.GetRecentExaminations(model.Program);
            model.Contacts = ps.GetContacts(model.Patient);

            return View(model);
        }

        [Route("patients/search")]
        public IActionResult Search(PatientSearchViewModel model, string q = "") {
            model.Query = q;
            return View(model);
        }

        [AllowAnonymous]
        public JsonResult SearchPatient(string qString) {
            PatientService ps = new PatientService(HttpContext);
            return Json(ps.SearchPatients(qString));
        }

        [HttpPost]
        public IActionResult RegisterNewPatient() {
            Patient patient = RegisterModel.Patient;
            patient.Person.DateOfBirth = DateTime.ParseExact(RegisterModel.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            patient.Save();

            PersonAddress address = RegisterModel.Address;
            address.Person = patient.Person;
            address.Save();

            PatientProgram program = new PatientProgram(patient) {
                DateEnrolled = DateTime.Parse(RegisterModel.DateEnrolled),
                Facility = new Facility(RegisterModel.FacilityId),
                Type = new Concept(RegisterModel.TypeId),
                Confirmation = new Concept(RegisterModel.ConfirmationId),
                Program = new Programs(RegisterModel.ProgramId),
                Category = new Concept(RegisterModel.CategoryId)
            };

            program.Create(HttpContext);
            
            return LocalRedirect("/registration/intake/" + program.Id);
        }

        [HttpPost]
        public IActionResult RegisterNewIntake()
        {
            PatientProgram pp = IntakeModel.Program;
            pp.ArtStartedOn = DateTime.Parse(IntakeModel.ArtStartedOn);
            pp.CptStartedOn = DateTime.Parse(IntakeModel.CptStartedOn);
            pp.UpdateIntake(HttpContext);

            PatientRegimen pr = IntakeModel.Regimen;
            pr.Program = pp;
            pr.Save(HttpContext);

            PatientExamination px = IntakeModel.Examination;
            px.Program = pp;
            px.Visit = new Visit(1);
            px.LabNo = pp.LaboratoryNumber;
            px.SputumSmearDate = DateTime.Parse(IntakeModel.SputumSmearDate);
            px.GeneXpertDate = DateTime.Parse(IntakeModel.GeneXpertDate);
            px.HivExamDate = DateTime.Parse(IntakeModel.HivExamDate);
            px.XrayExamDate = DateTime.Parse(IntakeModel.XrayExamDate);
            px.Save(HttpContext);

            return LocalRedirect("/patients/profile/" + pp.Patient.Id);
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
