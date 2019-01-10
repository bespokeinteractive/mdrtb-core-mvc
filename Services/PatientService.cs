using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using EtbSomalia.ViewModel;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Services
{
    public class PatientService
    {
        public int dbo { get; set; }
        public string dba { get; set; }

        public PatientService() { }
        public PatientService(HttpContext context){
            dbo = int.Parse(context.User.FindFirst(ClaimTypes.Dsa).Value);
            dba = context.User.FindFirst(ClaimTypes.Dns).Value;
        }

        public Patient GetPatient(long idnt){
            Patient patient = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pt_idnt, ps_idnt, ps_name, ps_gender, ps_dob, ps_estimate, pa_idnt, pa_default, pa_mobile, pa_telephone, pa_address, pa_postalcode, pa_village, pa_state, pa_county FROM Patient INNER JOIN Person ON pt_person = ps_idnt INNER JOIN PersonAddress ON ps_idnt = pa_person WHERE pt_idnt=" + idnt);
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
            SqlDataReader dr = conn.SqlServerConnect("SELECT TOP(50) pt_idnt, ps_name, ps_gender, ps_dob, pp_idnt, pp_tbmu, ISNULL(cpt_name, prg_description)x, fc_name FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PatientProgram ON pt_idnt=pp_patient INNER JOIN Program ON pp_progam=prg_idnt INNER JOIN Facilities ON pp_facility=fc_idnt LEFT OUTER JOIN Concept ON pp_outcome=cpt_id " + conn.GetQueryString(filter, "ps_name+'-'+ps_gender+'-'+pp_tbmu+'-'+ISNULL(cpt_name, prg_description)", "",false) + " ORDER BY ps_name");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
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
    }
}
