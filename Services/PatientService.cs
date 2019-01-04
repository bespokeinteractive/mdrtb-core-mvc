using System;
using System.Data.SqlClient;
using EtbSomalia.Extensions;
using EtbSomalia.Models;

namespace EtbSomalia.Services
{
    public class PatientService
    {
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
            SqlDataReader dr = conn.SqlServerConnect("SELECT pp_idnt, pp_tbmu, pp_enrolled_on, pp_completed_on, pp_patient, pp_facility, pp_progam, prg_description, pp_category, c_catg.cpt_name, pp_type, c_type.cpt_name, pp_confirmation, c_conf.cpt_name, pp_outcome, c_outc.cpt_name FROM PatientProgram INNER JOIN Program ON pp_progam = prg_idnt INNER JOIN Concept AS c_catg ON pp_category = c_catg.cpt_id INNER JOIN Concept AS c_type ON pp_type = c_type.cpt_id INNER JOIN Concept AS c_conf ON pp_confirmation = c_conf.cpt_id LEFT OUTER JOIN Concept AS c_outc ON pp_outcome = c_outc.cpt_id WHERE pp_idnt=" + idnt);
            if (dr.Read()) {
                program = new PatientProgram { 
                    Id = Convert.ToInt64(dr[0]),
                    TbmuNumber = dr[1].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[2])
                };

                if (!string.IsNullOrEmpty(dr[3].ToString())){
                    program.DateCompleted = Convert.ToDateTime(dr[3]);
                }

                program.Patient.Id = Convert.ToInt64(dr[4]);
                program.Facility.Id = Convert.ToInt64(dr[5]);

                program.Program = new Concept(Convert.ToInt64(dr[6]), dr[7].ToString());
                program.Category = new Concept(Convert.ToInt64(dr[8]), dr[9].ToString());
                program.Type = new Concept(Convert.ToInt64(dr[10]), dr[11].ToString());
                program.Confirmation = new Concept(Convert.ToInt64(dr[12]), dr[13].ToString());
                program.Outcome = new Concept(Convert.ToInt64(dr[14]), dr[15].ToString());
            }

            return program;
        }
    }
}
