using System;
using Microsoft.AspNetCore.Mvc;

using EtbSomalia.Models;
using EtbSomalia.ViewModel;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Collections.Generic;
using EtbSomalia.Extensions;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        [BindProperty]
        public PatientRegisterViewModel RegisterModel { get; set; }

        [BindProperty]
        public PatientIntakeViewModel IntakeModel { get; set; }

        [BindProperty]
        public PatientVisitsViewModel VisitModel { get; set; }

        [BindProperty]
        public PatientProfileViewModel ProfileModel { get; set; }

        // GET: /<controller>/
        [Route("registration/add")]
        public IActionResult Register(PatientRegisterViewModel model, ConceptService cs) {
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
                return LocalRedirect("/patients/profile/" + model.Program.Patient.GetUuid());
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

        [Route("registration/enroll")]
        public IActionResult Enroll(string p, PatientRegisterViewModel model, ConceptService cs) {
            model.Patient = new PatientService(HttpContext).GetPatient(p);
            model.Program.Category = new Concept(Constants.RELAPSE);

            if (model.Patient.isDead()) {
                return LocalRedirect("/patients/profile/" + model.Patient.GetUuid());
            }

            model.Facilities = new CoreService(HttpContext).GetFacilitiesIEnumerable();
            model.TBCategory = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CATEGORY));
            model.TBTypes = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_TYPE));
            model.TBConfirmation = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TB_CONFIRMATION));
            model.ResistanceProfile = cs.GetConceptAnswersIEnumerable(new Concept(Constants.RESISTANCE_PROFILE));

            return View(model);
        }

        [Route("patients/profile/{uuid}")]
        public IActionResult Profile(string uuid, PatientProfileViewModel model, PatientService ps, ConceptService cs, long program = 0) {
            CoreService core = new CoreService(HttpContext);
            model.Patient = ps.GetPatient(uuid);

            if (program.Equals(0)) {
                model.Program = ps.GetPatientProgram(model.Patient);
            }
            else {
                model.Program = ps.GetPatientProgram(program);
            }

            if (model.Program.DotsBy.Id.Equals(0)){
                return LocalRedirect("/registration/intake/" + model.Program.Id);
            }

            model.ExamOpts = cs.GetConceptAnswersIEnumerable(new Concept(Constants.SPUTUM_SMEAR));
            model.Outcomes = cs.GetConceptAnswersIEnumerable(new Concept(Constants.TREATMENT_OUTCOME));

            model.DateOfBirth = model.Patient.Person.DateOfBirth.ToString("dd/MM/yyyy");
            model.Facility = core.GetFacilitiesIEnumerable();
            model.Regimen = core.GetPatientRegimen(model.Program);
            model.Program.Facility = core.GetFacility(model.Program.Facility.Id);
            model.LatestVitals = ps.GetLatestVitals(model.Patient);
            model.Examinations = core.GetRecentExaminations(model.Program);
            model.Contacts = ps.GetContacts(model.Patient);

            return View(model);
        }

        [Route("patients/visits/add")]
        public IActionResult VisitsAdd(string p, PatientVisitsViewModel model, CoreService core, PatientService ps, ConceptService cs) {
            model.Patient = ps.GetPatient(p);
            model.Program = ps.GetPatientProgram(model.Patient);

            if (model.Program.DateCompleted.HasValue) {
                return LocalRedirect("/patients/profile/" + model.Patient.GetUuid());
            }

            model.Regimen = core.GetPatientRegimen(model.Program);
            model.Visits = core.GetProgramVisitsIEnumerable(model.Program, model.Regimen.Regimen);
            model.Regimens = core.GetRegimensIEnumerable(model.Program.Program);
            model.HivRecent = core.GetRecentHivExamination(model.Program);

            model.Examination = new PatientExamination {
                HivExam = model.HivRecent.Result,
                HivExamDate = model.HivRecent.Date
            };

            //Exam Options
            model.SputumSmearItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.SPUTUM_SMEAR));
            model.HivExamItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.HIV_EXAM));
            model.XrayExamItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.XRAY_EXAM));
            model.GeneXpertItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.GENE_XPERT));

            //HIV Options
            model.ARTItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.ART_STARTED_ON));
            model.CPTItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.CPT_STARTED_ON));

            return View(model);
        }

        [Route("patients/search")]
        public IActionResult Search(PatientSearchViewModel model, string q = "") {
            model.Query = q;

            if (!string.IsNullOrEmpty(q))
                model.Search = new PatientService(HttpContext).SearchPatients(q);

            return View(model);
        }

        [Route("patients/recent")]
        public IActionResult RecentlyAdded(RegisterViewModel model) {
            return View(model);
        }

        [Route("patients/active")]
        public IActionResult Active(PatientActiveViewModel model, long fac = 0) {
            CoreService core = new CoreService(HttpContext);
            model.Facilities = core.GetFacilitiesIEnumerable();
            if (fac.Equals(0))
                model.Active = core.GetFacility(Convert.ToInt64(model.Facilities[0].Value));
            else
                model.Active = core.GetFacility(fac);

            model.PatientSearch = new PatientService(HttpContext).SearchPatients("", "", "", model.Active.Id, true, 0);
            return View(model);
        }

        [Route("patients/register/{type}")]
        public IActionResult RegisterView(string type, RegisterViewModel model, long fac = 0) {
            CoreService core = new CoreService(HttpContext);

            model.Type = type;
            model.Facilities = core.GetFacilitiesIEnumerable();
            if (fac.Equals(0))
                model.Active = core.GetFacility(Convert.ToInt64(model.Facilities[0].Value));
            else
                model.Active = core.GetFacility(fac);

            if (type.Equals("tb"))
                model.Register = new PatientService(HttpContext).GetBmuRegister(model.Active);


            return View(model);
        }

        [Route("patients/summary/{type}")]
        public IActionResult Summary(string type, PatientSummaryViewModel model, PatientService service) {
            model.Title = type.FirstCharToUpper();
            if (type.Equals("national"))
                model.Summary = service.GetDataSummaryNational();
            else if (type.Equals("regional"))
                model.Summary = service.GetDataSummaryRegional();
            else if (type.Equals("facility"))
                model.Summary = service.GetDataSummaryFacility();
            else if (type.Equals("agency"))
                model.Summary = service.GetDataSummaryAgency();

            return View(model);
        }

        [AllowAnonymous]
        public JsonResult SearchPatient(string qString) {
            return Json(new PatientService(HttpContext).SearchPatients(qString));
        }

        [AllowAnonymous]
        public JsonResult GetRecentlyAdded(string start, string stops, string filter = "") {
            if (string.IsNullOrEmpty(filter))
                filter = "";
            return Json(new PatientService(HttpContext).SearchPatients(filter, start, stops));
        }

        [AllowAnonymous]
        public JsonResult GetRecentContacts(string start, string stops, string filter = "") {
            if (string.IsNullOrEmpty(filter))
                filter = "";
            return Json(new PatientService(HttpContext).GetContacts(start, stops, filter));
        }

        [HttpPost]
        public IActionResult RegisterNewPatient() {
            Patient patient = RegisterModel.Patient;
            patient.Person.DateOfBirth = DateTime.ParseExact(RegisterModel.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            patient.Save();

            PersonAddress address = RegisterModel.Address;
            address.Person = patient.Person;
            address.Save(HttpContext);

            PatientProgram program = RegisterModel.Program;
            program.Patient = patient;
            program.DateEnrolled = DateTime.Parse(RegisterModel.DateEnrolled);
            program.Create(HttpContext);
            
            return LocalRedirect("/registration/intake/" + program.Id);
        }

        [HttpPost]
        public IActionResult EnrollExistingPatient() {
            Patient patient = RegisterModel.Patient;

            PatientProgram program = RegisterModel.Program;
            program.Patient = patient;
            program.DateEnrolled = DateTime.Parse(RegisterModel.DateEnrolled);
            program.Create(HttpContext);

            return LocalRedirect("/registration/intake/" + program.Id);
        }

        [HttpPost]
        public IActionResult UpdatePatientDetails() {
            Patient pt = ProfileModel.Patient;
            Person ps = pt.Person;
            ps.DateOfBirth = DateTime.ParseExact(ProfileModel.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ps.Save(HttpContext);

            PersonAddress pa = ps.Address;
            pa.Person = ps;
            pa.Save(HttpContext);

            PatientProgram pp = ProfileModel.Program;
            pp.UpdateFacility();

            return LocalRedirect("/patients/profile/" + pt.GetUuid());
        }

        [HttpPost]
        public IActionResult UpdatePatientOutcome() {
            DateTime date = DateTime.Parse(ProfileModel.DateOfBirth);

            PatientProgram pp = ProfileModel.Program;
            PatientExamination px = ProfileModel.Examination;
            Patient pt = ProfileModel.Patient;

            pp.DateCompleted = date;
            pp.LaboratoryNumber = px.LabNo;
            pp.UpdateOutcome();

            px.SputumSmearDate = date;
            px.Visit = new Visit(25);
            px.Program = pp;
            px.Save(HttpContext);

            if (pp.Outcome.Id.Equals(Constants.PATIENT_DIED)) {
                pt.isDead(true);
                pt.DiedOn = date;
                pt.CauseOfDeath = new Concept(pp.Program.Id);
                pt.Update();
            }

            return LocalRedirect("/patients/profile/" + pt.GetUuid());
        }

        [HttpPost]
        public IActionResult RegisterNewIntake() {
            PatientProgram pp = IntakeModel.Program;
            pp.ArtStartedOn = DateTime.Parse(IntakeModel.ArtStartedOn);
            pp.CptStartedOn = DateTime.Parse(IntakeModel.CptStartedOn);
            pp.UpdateIntake();

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

            return LocalRedirect("/patients/profile/" + pp.Patient.GetUuid());
        }

        [HttpPost]
        public IActionResult AddVisits() {
            PatientProgram pp = VisitModel.Program;
            pp.ArtStartedOn = DateTime.Parse(VisitModel.ArtStartedOn);
            pp.CptStartedOn = DateTime.Parse(VisitModel.CptStartedOn);
            pp.UpdateVisit();

            //Test If Regimen has Changed, then Update
            PatientRegimen pr = VisitModel.Regimen;
            pr.Program = pp;
            pr.Save(HttpContext);

            //Post Visit
            PatientExamination px = VisitModel.Examination;
            px.Program = pp;
            px.LabNo = pp.LaboratoryNumber;
            px.SputumSmearDate = DateTime.Parse(VisitModel.SputumSmearDate);
            px.GeneXpertDate = DateTime.Parse(VisitModel.GeneXpertDate);
            px.HivExamDate = DateTime.Parse(VisitModel.HivExamDate);
            px.XrayExamDate = DateTime.Parse(VisitModel.XrayExamDate);
            px.Save(HttpContext);

            return LocalRedirect("/patients/profile/" + pp.Patient.GetUuid());
        }

        [AllowAnonymous]
        public string GetBirthdateFromString(string value) {
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

        public Boolean IsNumber(String s) {
            Boolean value = true;
            foreach (Char c in s.ToCharArray())
            {
                value = value && Char.IsDigit(c);
            }

            return value;
        }
    }
}
