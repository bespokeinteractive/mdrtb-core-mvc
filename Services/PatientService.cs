using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Services
{
    public class PatientService
    {
        public int Actor { get; set; }
        public string Username { get; set; }

        public PatientService() { }
        public PatientService(HttpContext context){
            Actor = int.Parse(context.User.FindFirst(ClaimTypes.Actor).Value);
            Username = context.User.FindFirst(ClaimTypes.UserData).Value;
        }

        public List<SelectListItem> InitializeGender()
        {
            List<SelectListItem> gender = new List<SelectListItem> {
                new SelectListItem("Male", "male"),
                new SelectListItem("Female", "female")
            };

            return gender;
        }

        public Patient GetPatient(long idnt){
            Patient patient = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pt_idnt, ps_idnt, ps_name, ps_gender, ps_dob, ps_estimate, pa_idnt, pa_default, pa_mobile, pa_telephone, pa_address, pa_postalcode, pa_village, pa_state, pa_county FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PersonAddress ON ps_idnt=pa_person WHERE pt_idnt=" + idnt);
            if (dr.Read()) {
                patient = new Patient {
                    Id = Convert.ToInt64(dr[0])
                };

                patient.Person = new Person { 
                    Id = Convert.ToInt64(dr[1]),
                    Name = dr[2].ToString(),
                    Gender = dr[3].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[4]),
                    AgeEstimate = Convert.ToBoolean(dr[5])
                };

                patient.Person.Address = new PersonAddress { 
                    Id = Convert.ToInt64(dr[6]),
                    Default = Convert.ToBoolean(dr[7]),
                    Mobile = dr[8].ToString(),
                    Telephone = dr[9].ToString(),
                    Address = dr[10].ToString(),
                    PostalCode = dr[11].ToString(),
                    Village = dr[12].ToString(),
                    State = dr[13].ToString(),
                    County = dr[14].ToString()
                };
            }

            return patient;
        }

        public PatientProgram GetPatientProgram(long idnt) {
            PatientProgram program = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pp_idnt, pp_tbmu, pp_registeer_no, pp_laboratory_no, pp_supporter, pp_supporter_contacts, pp_enrolled_on, pp_completed_on, pp_patient, pp_facility, pp_progam, prg_description, pp_category, c_catg.cpt_name, pp_type, c_type.cpt_name, pp_confirmation, c_conf.cpt_name, pp_outcome, c_outc.cpt_name, pp_dots_by, c_dots.cpt_name, pp_referred_by, c_refs.cpt_name, pp_default, pp_art_started, pp_cpt_started, pp_art_started_on, pp_cpt_started_on, pp_created_on, pp_created_by FROM PatientProgram INNER JOIN Program ON pp_progam=prg_idnt INNER JOIN Concept AS c_catg ON pp_category=c_catg.cpt_id INNER JOIN Concept AS c_type ON pp_type=c_type.cpt_id INNER JOIN Concept AS c_conf ON pp_confirmation=c_conf.cpt_id LEFT OUTER JOIN Concept AS c_outc ON pp_outcome=c_outc.cpt_id LEFT OUTER JOIN Concept AS c_dots ON pp_dots_by=c_dots.cpt_id LEFT OUTER JOIN Concept AS c_refs ON pp_referred_by=c_refs.cpt_id WHERE pp_idnt=" + idnt);
            if (dr.Read()) {
                program = new PatientProgram { 
                    Id = Convert.ToInt64(dr[0]),
                    TbmuNumber = dr[1].ToString(),
                    RegisterNumber = dr[2].ToString(),
                    LaboratoryNumber = dr[3].ToString(),
                    TreatmentSupporter = dr[4].ToString(),
                    SupporterContacts = dr[5].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[6])
                };

                if (!string.IsNullOrEmpty(dr[7].ToString())){
                    program.DateCompleted = Convert.ToDateTime(dr[7]);
                }

                program.Patient.Id = Convert.ToInt64(dr[8]);
                program.Facility.Id = Convert.ToInt64(dr[9]);

                program.Program = new Programs(Convert.ToInt64(dr[10]), dr[11].ToString());
                program.Category = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                program.Type = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
                program.Confirmation = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString());
                program.Outcome = new Concept(Convert.ToInt64(dr[18]), dr[19].ToString());
                program.DotsBy = new Concept(Convert.ToInt64(dr[20]), dr[21].ToString());
                program.ReferredBy = new Concept(Convert.ToInt64(dr[22]), dr[23].ToString());

                program.Default = Convert.ToBoolean(dr[24]);
                program.ArtStarted = Convert.ToInt32(dr[25]);
                program.CptStarted = Convert.ToInt32(dr[26]);

                program.ArtStartedOn = Convert.ToDateTime(dr[27]);
                program.CptStartedOn = Convert.ToDateTime(dr[28]);

                program.CreatedOn = Convert.ToDateTime(dr[29]);
                program.CreatedBy = new Users(Convert.ToInt64(dr[30]));
            }

            return program;
        }

        public PatientProgram GetPatientProgram(Patient patient)
        {
            PatientProgram program = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT TOP(1) pp_idnt, pp_tbmu, pp_registeer_no, pp_laboratory_no, pp_supporter, pp_supporter_contacts, pp_enrolled_on, pp_completed_on, pp_patient, pp_facility, pp_progam, prg_description, pp_category, c_catg.cpt_name, pp_type, c_type.cpt_name, pp_confirmation, c_conf.cpt_name, pp_outcome, c_outc.cpt_name, pp_dots_by, c_dots.cpt_name, pp_referred_by, c_refs.cpt_name, pp_default, pp_art_started, pp_cpt_started, pp_art_started_on, pp_cpt_started_on, pp_created_on, pp_created_by FROM PatientProgram INNER JOIN Program ON pp_progam=prg_idnt INNER JOIN Concept AS c_catg ON pp_category=c_catg.cpt_id INNER JOIN Concept AS c_type ON pp_type=c_type.cpt_id INNER JOIN Concept AS c_conf ON pp_confirmation=c_conf.cpt_id LEFT OUTER JOIN Concept AS c_outc ON pp_outcome=c_outc.cpt_id LEFT OUTER JOIN Concept AS c_dots ON pp_dots_by=c_dots.cpt_id LEFT OUTER JOIN Concept AS c_refs ON pp_referred_by=c_refs.cpt_id WHERE pp_default=1 AND pp_patient=" + patient.Id + " ORDER BY pp_idnt DESC");
            if (dr.Read()) {
                program = new PatientProgram {
                    Id = Convert.ToInt64(dr[0]),
                    TbmuNumber = dr[1].ToString(),
                    RegisterNumber = dr[2].ToString(),
                    LaboratoryNumber = dr[3].ToString(),
                    TreatmentSupporter = dr[4].ToString(),
                    SupporterContacts = dr[5].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[6])
                };

                if (!string.IsNullOrEmpty(dr[7].ToString())) {
                    program.DateCompleted = Convert.ToDateTime(dr[7]);
                }

                program.Patient.Id = Convert.ToInt64(dr[8]);
                program.Facility.Id = Convert.ToInt64(dr[9]);

                program.Program = new Programs(Convert.ToInt64(dr[10]), dr[11].ToString());
                program.Category = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                program.Type = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
                program.Confirmation = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString());
                program.Outcome = new Concept(Convert.ToInt64(dr[18]), dr[19].ToString());
                program.DotsBy = new Concept(Convert.ToInt64(dr[20]), dr[21].ToString());
                program.ReferredBy = new Concept(Convert.ToInt64(dr[22]), dr[23].ToString());

                program.Default = Convert.ToBoolean(dr[24]);
                program.ArtStarted = Convert.ToInt32(dr[25]);
                program.CptStarted = Convert.ToInt32(dr[26]);

                program.ArtStartedOn = Convert.ToDateTime(dr[27]);
                program.CptStartedOn = Convert.ToDateTime(dr[28]);

                program.CreatedOn = Convert.ToDateTime(dr[29]);
                program.CreatedBy = new Users(Convert.ToInt64(dr[30]));
            }

            return program;
        }

        public Vitals GetLatestVitals(Patient patient) {
            Vitals vitals = new Vitals();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT TOP(1) pe_weight, pe_created_on FROM PatientExamination INNER JOIN PatientProgram ON pe_program=pp_idnt WHERE pe_weight>0 AND pp_patient=" + patient.Id +  " ORDER BY pe_created_on DESC, pe_idnt DESC");
            if (dr.Read()) {
                vitals.Weight = dr[0].ToString() + "KGS";
                vitals.WeightOn = Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy");
            }

            conn = new SqlServerConnection();
            dr = conn.SqlServerConnect("SELECT TOP(1) pe_height, pe_created_on FROM PatientExamination INNER JOIN PatientProgram ON pe_program=pp_idnt WHERE pe_height>0 AND pp_patient=" + patient.Id + " ORDER BY pe_created_on DESC, pe_idnt DESC");
            if (dr.Read()) {
                vitals.Height = dr[0].ToString() + "CMS";
                vitals.HeightOn = Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy");
            }

            return vitals;
        }

        public List<PatientSearch> SearchPatients(string filter)
        {
            List<PatientSearch> search = new List<PatientSearch>();

            SqlServerConnection conn = new SqlServerConnection();
            string query = conn.GetQueryString(filter, "ps_name+'-'+ps_gender+'-'+pp_tbmu+'-'+ISNULL(cpt_name, prg_description)", "pt_idnt IN (SELECT pp_patient FROM PatientProgram WHERE pp_facility IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + "))", false);

            SqlDataReader dr = conn.SqlServerConnect("SELECT TOP(50) pt_idnt, ps_name, ps_gender, ps_dob, pp_idnt, pp_tbmu, ISNULL(cpt_name, prg_description)x, fc_name FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PatientProgram ON pt_idnt=pp_patient INNER JOIN Program ON pp_progam=prg_idnt INNER JOIN Facilities ON pp_facility=fc_idnt LEFT OUTER JOIN Concept ON pp_outcome=cpt_id " + query + " ORDER BY ps_name");
            if (dr.HasRows) {
                while (dr.Read()) {
                    PatientSearch ps = new PatientSearch();
                    ps.Patient.Id = Convert.ToInt64(dr[0]);
                    ps.Patient.Person.Name = dr[1].ToString();
                    ps.Patient.Person.Gender = dr[2].ToString().FirstCharToUpper();
                    ps.Patient.Person.DateOfBirth = Convert.ToDateTime(dr[3]);

                    ps.Program.Id = Convert.ToInt64(dr[4]);
                    ps.Program.TbmuNumber = dr[5].ToString();

                    ps.Status = dr[6].ToString();
                    ps.Facility = dr[7].ToString();

                    ps.age = ps.Patient.GetAge();

                    search.Add(ps);
                }
            }

            return search;
        }

        public Contacts GetContact(long idnt)
        {
            Contacts contact = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_idnt, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id WHERE ct_idnt=" + idnt);
            if (dr.Read())
            {
                contact = new Contacts
                {
                    Id = Convert.ToInt64(dr[0]),
                    Identifier = dr[1].ToString(),
                    Notes = dr[2].ToString(),
                    ExposedOn = Convert.ToDateTime(dr[3]),
                    AddedOn = Convert.ToDateTime(dr[4]),
                    AddedBy = new Users(Convert.ToInt64(dr[5])),
                    Patient = new Patient(Convert.ToInt64(dr[6])),
                    NextVisit = Convert.ToDateTime(dr[7])
                };

                contact.Person = new Person
                {
                    Id = Convert.ToInt64(dr[8]),
                    Name = dr[9].ToString(),
                    Gender = dr[10].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[11])
                };

                contact.Status = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                contact.Location = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
                contact.Relation = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString());
                contact.Proximity = new Concept(Convert.ToInt64(dr[18]), dr[19].ToString());
                contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[20]), dr[21].ToString());
                contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[22]), dr[23].ToString());

                contact.Index = new PatientProgram
                {
                    Id = Convert.ToInt64(dr[24]),
                    TbmuNumber = dr[25].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[26]),
                    Patient = new Patient(Convert.ToInt64(dr[27]))
                };

                contact.Index.Patient.Person = new Person
                {
                    Id = Convert.ToInt64(dr[28]),
                    Name = dr[29].ToString(),
                    Gender = dr[30].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[31]),
                };
            }

            return contact;
        }

        public List<Contacts> GetContacts()
        {
            List<Contacts> contacts = new List<Contacts>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_idnt, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id ORDER BY ct_identifier, p.ps_name");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Contacts contact = new Contacts
                    {
                        Id = Convert.ToInt64(dr[0]),
                        Identifier = dr[1].ToString(),
                        Notes = dr[2].ToString(),
                        ExposedOn = Convert.ToDateTime(dr[3]),
                        AddedOn = Convert.ToDateTime(dr[4]),
                        AddedBy = new Users(Convert.ToInt64(dr[5])),
                        Patient = new Patient(Convert.ToInt64(dr[6])),
                        NextVisit = Convert.ToDateTime(dr[7])
                    };

                    contact.Person = new Person
                    {
                        Id = Convert.ToInt64(dr[8]),
                        Name = dr[9].ToString(),
                        Gender = dr[10].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[11])
                    };

                    contact.Status = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                    contact.Location = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
                    contact.Relation = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString());
                    contact.Proximity = new Concept(Convert.ToInt64(dr[18]), dr[19].ToString());
                    contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[20]), dr[21].ToString());
                    contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[22]), dr[23].ToString());

                    contact.Index = new PatientProgram
                    {
                        Id = Convert.ToInt64(dr[24]),
                        TbmuNumber = dr[25].ToString(),
                        DateEnrolled = Convert.ToDateTime(dr[26]),
                        Patient = new Patient(Convert.ToInt64(dr[27]))
                    };

                    contact.Index.Patient.Person = new Person
                    {
                        Id = Convert.ToInt64(dr[28]),
                        Name = dr[29].ToString(),
                        Gender = dr[30].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[31]),
                    };

                    contacts.Add(contact);
                }
            }

            return contacts;
        }

        public List<Contacts> GetContacts(Patient patient)
        {
            List<Contacts> contacts = new List<Contacts>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_idnt, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id WHERE pt_idnt=" + patient.Id + " ORDER BY ct_identifier, p.ps_name");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Contacts contact = new Contacts
                    {
                        Id = Convert.ToInt64(dr[0]),
                        Identifier = dr[1].ToString(),
                        Notes = dr[2].ToString(),
                        ExposedOn = Convert.ToDateTime(dr[3]),
                        AddedOn = Convert.ToDateTime(dr[4]),
                        AddedBy = new Users(Convert.ToInt64(dr[5])),
                        Patient = new Patient(Convert.ToInt64(dr[6])),
                        NextVisit = Convert.ToDateTime(dr[7])
                    };

                    contact.Person = new Person
                    {
                        Id = Convert.ToInt64(dr[8]),
                        Name = dr[9].ToString(),
                        Gender = dr[10].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[11])
                    };

                    contact.Status = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                    contact.Location = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
                    contact.Relation = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString());
                    contact.Proximity = new Concept(Convert.ToInt64(dr[18]), dr[19].ToString());
                    contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[20]), dr[21].ToString());
                    contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[22]), dr[23].ToString());

                    contact.Index = new PatientProgram
                    {
                        Id = Convert.ToInt64(dr[24]),
                        TbmuNumber = dr[25].ToString(),
                        DateEnrolled = Convert.ToDateTime(dr[26]),
                        Patient = new Patient(Convert.ToInt64(dr[27]))
                    };

                    contact.Index.Patient.Person = new Person
                    {
                        Id = Convert.ToInt64(dr[28]),
                        Name = dr[29].ToString(),
                        Gender = dr[30].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[31]),
                    };

                    contacts.Add(contact);
                }
            }

            return contacts;
        }

        public PersonAddress GetPersonAddress(Person person)
        {
            PersonAddress address = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pa_idnt, pa_default, pa_mobile, pa_telephone, pa_address, pa_postalcode, pa_village, pa_state, pa_county FROM PersonAddress WHERE pa_default=1 AND pa_person=" + person.Id);
            if (dr.Read())
            {
                address = new PersonAddress {
                    Id = Convert.ToInt64(dr[0]),
                    Default = Convert.ToBoolean(dr[1]),
                    Mobile = dr[2].ToString(),
                    Telephone = dr[3].ToString(),
                    Address = dr[4].ToString(),
                    PostalCode = dr[5].ToString(),
                    Village = dr[6].ToString(),
                    State = dr[7].ToString(),
                    County = dr[8].ToString(),
                };

                address.Person = person;
            }

            return address;
        }

        //Data Writers
        public Contacts RegisterContact(Contacts ctx)
        {
            SqlServerConnection conn = new SqlServerConnection();
            ctx.Id = conn.SqlServerUpdate("INSERT INTO Contacts (ct_person, ct_index, ct_exposed_from, ct_identifier, ct_location, ct_relationship, ct_proximity, ct_desease_after, ct_prev_treated, ct_next_screening, ct_added_by) output INSERTED.ct_idnt VALUES (" + ctx.Person.Id + ", " + ctx.Index.Id + ", '" + ctx.ExposedOn.Date + "', '" + ctx.Identifier + "', " + ctx.Location.Id + ", " + ctx.Relation.Id + ", " + ctx.Proximity.Id + ", " + ctx.DiseaseAfter.Id + ", " + ctx.PrevouslyTreated.Id + ", '" + ctx.NextVisit + "', " + Actor + ")");

            return ctx;
        }

        public ContactsExamination SaveContactsExamination(ContactsExamination cx)
        {
            SqlServerConnection conn = new SqlServerConnection();
            cx.Id = conn.SqlServerUpdate("INSERT INTO ContactsExaminations (ce_contact, ce_cough, ce_fever, ce_weight_loss, ce_night_sweats, ce_ltbi, ce_sputum_smear, ce_genxpert, ce_xray_exam, ce_preventive_regimen, ce_next_screening, ce_added_by) output INSERTED.ce_idnt VALUES (" + cx.Contact.Id + ", '" + cx.Cough + "', '" + cx.Fever + "', '" + cx.WeightLoss + "', '" + cx.NightSweat + "', " + cx.LTBI.Id + ", " + cx.SputumSmear.Id + ", " + cx.GeneXpert.Id + ", " + cx.XrayExam.Id + ", '" + cx.PreventiveTherapy + "', '" + cx.NextScreening.Date + "', " + Actor + ")");

            return cx;
        }
    }
}
