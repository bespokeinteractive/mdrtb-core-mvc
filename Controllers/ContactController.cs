using System;
using System.Collections.Generic;
using System.Globalization;
using EtbSomalia.DataModel;
using EtbSomalia.Models;
using EtbSomalia.Services;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtbSomalia.Controllers
{
    [Authorize]
    public class ContactController : Controller 
    {
        [BindProperty]
        public ContactAddViewModel ContactAddModel { get; set; }

        [Route("contacts")]
        public IActionResult Index() {
            List<Contacts> contacts = new List<Contacts>(new PatientService(HttpContext).GetContacts());
            return View(contacts);
        }

        [Route("contacts/add")]
        public IActionResult Add(ContactAddViewModel model, PatientService ps, ConceptService cs, long p = 0)
        {
            model.Program = ps.GetPatientProgram(p);
            model.Program.Patient = ps.GetPatient(model.Program.Patient.Id);

            model.Location = cs.GetConceptAnswersIEnumerable(new Concept(Constants.LOCATION_OF_CONTACT));
            model.Relation = cs.GetConceptAnswersIEnumerable(new Concept(Constants.RELATION_TO_INDEX));
            model.Proximity = cs.GetConceptAnswersIEnumerable(new Concept(Constants.PROXIMITY_TO_INDEX));
            model.DeseaseAfterExposure = cs.GetConceptAnswersIEnumerable(new Concept(Constants.DESEASE_AFTER_EXPOSURE));
            model.PreviouslyTreated = cs.GetConceptAnswersIEnumerable(new Concept(Constants.PREVIOUSLY_TREATED));

            //Exam Options
            model.LatentTBI = cs.GetConceptAnswersIEnumerable(new Concept(Constants.LATENT_TB_INFECTION));
            model.SputumSmearItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.SPUTUM_SMEAR));
            model.GeneXpertItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.GENE_XPERT));
            model.XrayExamItems = cs.GetConceptAnswersIEnumerable(new Concept(Constants.XRAY_EXAM));

            return View(model);
        }

        [Route("contacts/{uuid}")]
        public IActionResult Main(string uuid, ContactMainViewModel model)
        {
            model.Contact = new PatientService(HttpContext).GetContact(uuid);
            model.Contact.Person.Address = model.Contact.Person.GetPersonAddress();
            return View(model);
        }

        [Route("contacts/register")]
        public IActionResult Register() {
            return View(new PatientService(HttpContext).GetContactsRegister());
        }

        [HttpPost]
        public IActionResult RegisterNewContact()
        {
            Contacts contact = ContactAddModel.Contact;
            contact.Index = ContactAddModel.Program;
            contact.Person.DateOfBirth = DateTime.ParseExact(ContactAddModel.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            contact.ExposedOn = ContactAddModel.Program.DateEnrolled;
            contact.NextVisit = DateTime.Parse(ContactAddModel.NextVisit);
            contact.Register(HttpContext);

            PersonAddress address = ContactAddModel.Contact.Person.Address;
            address.Person = contact.Person;
            address.Save();

            ContactsExamination exam = ContactAddModel.Examination;
            exam.Contact = contact;
            exam.NextScreening = DateTime.Parse(ContactAddModel.NextVisit);
            exam.Save(HttpContext);

            return LocalRedirect("/contacts/" + contact.GetUuid());
        }
    }
}
