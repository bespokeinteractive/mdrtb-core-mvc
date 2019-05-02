using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using EtbSomalia.DataModel;
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

        public List<SelectListItem> InitializeGender() {
            List<SelectListItem> gender = new List<SelectListItem> {
                new SelectListItem("Male", "male"),
                new SelectListItem("Female", "female")
            };

            return gender;
        }

        public Patient GetPatient(long idnt) {
            Patient patient = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pt_idnt, pt_uuid, ps_idnt, ps_name, ps_gender, ps_dob, ps_estimate, pa_idnt, pa_default, pa_mobile, pa_telephone, pa_address, pa_postalcode, pa_village, pa_state, pa_county FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PersonAddress ON ps_idnt=pa_person WHERE pt_idnt=" + idnt);
            if (dr.Read()) {
                patient = new Patient {
                    Id = Convert.ToInt64(dr[0]),
                    Uuid = dr[1].ToString()
                };

                patient.Person = new Person { 
                    Id = Convert.ToInt64(dr[2]),
                    Name = dr[3].ToString(),
                    Gender = dr[4].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[5]),
                    AgeEstimate = Convert.ToBoolean(dr[6])
                };

                patient.Person.Address = new PersonAddress { 
                    Id = Convert.ToInt64(dr[7]),
                    Default = Convert.ToBoolean(dr[8]),
                    Mobile = dr[9].ToString(),
                    Telephone = dr[10].ToString(),
                    Address = dr[11].ToString(),
                    PostalCode = dr[12].ToString(),
                    Village = dr[13].ToString(),
                    State = dr[14].ToString(),
                    County = dr[15].ToString()
                };
            }

            return patient;
        }

        public Patient GetPatient(string uuid) {
            Patient patient = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pt_idnt, pt_uuid, ps_idnt, ps_name, ps_gender, ps_dob, ps_estimate, pa_idnt, pa_default, pa_mobile, pa_telephone, pa_address, pa_postalcode, pa_village, pa_state, pa_county FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PersonAddress ON ps_idnt=pa_person WHERE pt_uuid COLLATE SQL_Latin1_General_CP1_CS_AS LIKE '" + uuid + "'");
            if (dr.Read()) {
                patient = new Patient {
                    Id = Convert.ToInt64(dr[0]),
                    Uuid = dr[1].ToString()
                };

                patient.Person = new Person {
                    Id = Convert.ToInt64(dr[2]),
                    Name = dr[3].ToString(),
                    Gender = dr[4].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[5]),
                    AgeEstimate = Convert.ToBoolean(dr[6])
                };

                patient.Person.Address = new PersonAddress {
                    Id = Convert.ToInt64(dr[7]),
                    Default = Convert.ToBoolean(dr[8]),
                    Mobile = dr[9].ToString(),
                    Telephone = dr[10].ToString(),
                    Address = dr[11].ToString(),
                    PostalCode = dr[12].ToString(),
                    Village = dr[13].ToString(),
                    State = dr[14].ToString(),
                    County = dr[15].ToString()
                };
            }

            return patient;
        }

        public string GetPatientUuid(long idnt) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pt_uuid FROM patient WHERE pt_idnt=" + idnt);
            if (dr.Read())
                return dr[0].ToString();

            return "";
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

        public PatientProgram GetPatientProgram(Patient patient) {
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

        public List<PatientSearch> SearchPatients(string filter, string start = "", string stop = "", long facility = 0, bool active = false, int limit = 50) {
            List<PatientSearch> search = new List<PatientSearch>();

            SqlServerConnection conn = new SqlServerConnection();
            string count = "";
            string query = conn.GetQueryString(filter, "ps_name+'-'+ps_gender+'-'+pp_tbmu+'-'+fc_name+'-'+ISNULL(cpt_name, prg_description)", "pt_idnt IN (SELECT pp_patient FROM PatientProgram WHERE pp_facility IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + "))", true);
            string order = " ORDER BY ps_name";

            if (!string.IsNullOrEmpty(start)) {
                query += " AND pp_enrolled_on BETWEEN '" + DateTime.Parse(start) + "' AND '" + DateTime.Parse(stop) + "'";
                order = " ORDER BY pp_enrolled_on DESC, ps_name";
            }
            if (!limit.Equals(0))
                count = "TOP(" + limit + ")";
            if (!facility.Equals(0))
                query += " AND pp_facility=" + facility;
            if (active)
                query += " AND pp_completed_on IS NULL";

            SqlDataReader dr = conn.SqlServerConnect("SELECT " + count + " pt_idnt, pt_uuid, ps_name, ps_gender, ps_dob, pp_idnt, pp_tbmu, pp_enrolled_on, pp_completed_on, ISNULL(cpt_name, prg_description)x, fc_name, pp_enrolled_on FROM Patient INNER JOIN Person ON pt_person=ps_idnt INNER JOIN PatientProgram ON pt_idnt=pp_patient INNER JOIN Program ON pp_progam=prg_idnt INNER JOIN Facilities ON pp_facility=fc_idnt LEFT OUTER JOIN Concept ON pp_outcome=cpt_id " + query + order);
            if (dr.HasRows) {
                while (dr.Read()) {
                    PatientSearch ps = new PatientSearch();
                    ps.Patient.Id = Convert.ToInt64(dr[0]);
                    ps.Patient.Uuid = dr[1].ToString();
                    ps.Patient.Person.Name = dr[2].ToString();
                    ps.Patient.Person.Gender = dr[3].ToString().FirstCharToUpper();
                    ps.Patient.Person.DateOfBirth = Convert.ToDateTime(dr[4]);

                    ps.Program.Id = Convert.ToInt64(dr[5]);
                    ps.Program.TbmuNumber = dr[6].ToString();
                    ps.Program.DateEnrolled = Convert.ToDateTime(dr[7]);

                    if (dr[8] != DBNull.Value)
                        ps.Program.DateCompleted = Convert.ToDateTime(dr[8]);

                    ps.Status = dr[9].ToString();
                    ps.Facility = dr[10].ToString();

                    ps.Age = ps.Patient.GetAge();
                    ps.AddedOn = Convert.ToDateTime(dr[11]).ToString("dd/MM/yyyy");

                    search.Add(ps);
                }
            }

            return search;
        }

        public List<Register> GetBmuRegister(Facility facility) {
            List<Register> registers = new List<Register>();
            SqlServerConnection conn = new SqlServerConnection();

            SqlDataReader dr = conn.SqlServerConnect("SELECT pp_idnt, ISNULL(NULLIF(pp_tbmu,'N/A'), pp_registeer_no)pp_tbmux, ISNULL(NULLIF(pp_supporter,''),'—')pp_supporter, pp_created_on, pp_enrolled_on, pp_completed_on, pp_category, c_catg.cpt_name, pp_type, CASE WHEN pp_type=3 THEN 'PB' ELSE 'EP' END pp_type, pp_confirmation, CASE WHEN pp_confirmation=6 THEN 'BC' ELSE 'CD' END pp_confirmation, pp_outcome, ISNULL(c_outc.cpt_name, '—')outcome, pp_dots_by, c_dots.cpt_name, pp_referred_by, c_refs.cpt_name, fc_idnt, fc_name, pp_art_started, pp_cpt_started, pt_uuid, ps_name, UPPER(SUBSTRING(ps_gender,1,1))ps_gender, ps_dob, pa_address, rg_date_1, rg_sputum_1, rg_hiv, rg_xray, rg_date_2, rg_sputum_2, rg_date_3, rg_sputum_3, rg_date_4, rg_sputum_4 FROM PatientProgram INNER JOIN Program ON pp_progam=prg_idnt AND pp_progam=1 INNER JOIN Facilities ON pp_facility=fc_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ON ps_idnt=pt_person LEFT OUTER JOIN PersonAddress ON pa_person=pt_person AND pa_default=1 LEFT OUTER JOIN Concept AS c_catg ON pp_category=c_catg.cpt_id LEFT OUTER JOIN Concept AS c_outc ON pp_outcome=c_outc.cpt_id LEFT OUTER JOIN Concept AS c_dots ON pp_dots_by=c_dots.cpt_id LEFT OUTER JOIN Concept AS c_refs ON pp_referred_by=c_refs.cpt_id LEFT OUTER JOIN vRegisterBmu ON rg_program=pp_idnt WHERE pp_facility=" + facility.Id + " ORDER BY pp_tbmux");
            if (dr.HasRows) {
                while (dr.Read()) {
                    Register register = new Register();
                    if (dr[5] != DBNull.Value)
                        register.DateCompleted = Convert.ToDateTime(dr[5]).ToString("dd/MM/yyyy");

                    register.Program = new PatientProgram {
                        Id = Convert.ToInt64(dr[0]),
                        TbmuNumber = dr[1].ToString(),
                        TreatmentSupporter = dr[2].ToString(),
                        CreatedOn = Convert.ToDateTime(dr[3]),
                        DateEnrolled = Convert.ToDateTime(dr[4]),
                        Category = new Concept {
                            Id = Convert.ToInt64(dr[6]),
                            Name = dr[7].ToString(),
                        },
                        Type = new Concept {
                            Id = Convert.ToInt64(dr[8]),
                            Name = dr[9].ToString(),
                        },
                        Confirmation = new Concept {
                            Id = Convert.ToInt64(dr[10]),
                            Name = dr[11].ToString(),
                        },
                        Outcome = new Concept {
                            Id = Convert.ToInt64(dr[12]),
                            Name = dr[13].ToString(),
                        },
                        DotsBy = new Concept {
                            Id = Convert.ToInt64(dr[14]),
                            Name = dr[15].ToString(),
                        },
                        ReferredBy = new Concept {
                            Id = Convert.ToInt64(dr[16]),
                            Name = dr[17].ToString(),
                        },
                        Facility = new Facility {
                            Id = Convert.ToInt64(dr[18]),
                            Name = dr[19].ToString(),
                        },
                        ArtStarted = Convert.ToInt32(dr[20]),
                        CptStarted = Convert.ToInt32(dr[21]),
                        Patient = new Patient {
                            Uuid = dr[22].ToString(),
                            Person = new Person {
                                Name = dr[23].ToString(),
                                Gender = dr[24].ToString(),
                                DateOfBirth = Convert.ToDateTime(dr[25]),
                                Address = new PersonAddress {
                                    Address = dr[26].ToString(),
                                }
                            }
                        }
                    };

                    register.Start = new RegisterExam {
                        SputumSmear = dr[28].ToString(),
                        HivExam = dr[29].ToString(),
                        XrayExam = dr[30].ToString(),
                    };

                    if (dr[27] != DBNull.Value)
                        register.Start.Date = Convert.ToDateTime(dr[27]).ToString("dd.MM.yy");

                    register.Two = new RegisterExam {
                        SputumSmear = dr[32].ToString()
                    };

                    if (dr[31] != DBNull.Value)
                        register.Two.Date = Convert.ToDateTime(dr[31]).ToString("dd.MM.yy");

                    register.Five = new RegisterExam {
                        SputumSmear = dr[34].ToString()
                    };

                    if (dr[33] != DBNull.Value)
                        register.Five.Date = Convert.ToDateTime(dr[33]).ToString("dd.MM.yy");

                    register.Last = new RegisterExam {
                        SputumSmear = dr[36].ToString()
                    };

                    if (dr[35] != DBNull.Value)
                        register.Last.Date = Convert.ToDateTime(dr[35]).ToString("dd.MM.yy");

                    registers.Add(register);
                }
            }

            return registers;
        }

        private List<DataSummaryModel> GetDataSummary(string query) {
            List<DataSummaryModel> model = new List<DataSummaryModel>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect(query);
            if (dr.HasRows) {
                while (dr.Read()) {
                    model.Add(new DataSummaryModel {
                        Year = Convert.ToInt32(dr[0]),
                        Males = Convert.ToInt32(dr[1]),
                        Females = Convert.ToInt32(dr[2]),
                        Tb = Convert.ToInt32(dr[3]),
                        Mdr = Convert.ToInt32(dr[4]),
                        Pulmonary = Convert.ToInt32(dr[5]),
                        ExtraPulmonary = Convert.ToInt32(dr[6]),
                        BacterialConfirmed = Convert.ToInt32(dr[7]),
                        ClinicalDiagnosed = Convert.ToInt32(dr[8]),
                        Complete = Convert.ToInt32(dr[9]),
                        Outcomes = Convert.ToInt32(dr[10]),
                        Name = dr[11].ToString()
                    });
                }
            }

            return model;
        }

        public List<DataSummaryModel> GetDataSummaryNational() {
            return this.GetDataSummary("SELECT TOP(5) YR, SUM(males)males, SUM(females)females, SUM(tb)tb, SUM(mdr)mdr, SUM(pb)pb, SUM(ep)ep, SUM(bc)bc, SUM(cd)cd, SUM(complete)complete, SUM(outcomes)outcomes, ''x FROM (SELECT YEAR(pp_enrolled_on)YR, CASE WHEN ps_gender='male' THEN 1 ELSE 0 END males, CASE WHEN ps_gender='female' THEN 1 ELSE 0 END females, CASE WHEN pp_progam=1 THEN 1 ELSE 0 END tb, CASE WHEN pp_progam=2 THEN 1 ELSE 0 END mdr, CASE WHEN pp_type=3 THEN 1 ELSE 0 END pb, CASE WHEN pp_type=4 THEN 1 ELSE 0 END ep, CASE WHEN pp_confirmation=6 THEN 1 ELSE 0 END bc, CASE WHEN pp_confirmation=7 THEN 1 ELSE 0 END cd, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END complete, CASE WHEN pp_outcome=0 THEN 0 ELSE 1 END outcomes FROM PatientProgram INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ON pt_person=ps_idnt WHERE pp_enrolled_on IS NOT NULL) As Foo GROUP BY YR ORDER BY YR DESC");
        }

        public List<DataSummaryModel> GetDataSummaryRegional() {
            return this.GetDataSummary("SELECT YR, SUM(males)males, SUM(females)females, SUM(tb)tb, SUM(mdr)mdr, SUM(pb)pb, SUM(ep)ep, SUM(bc)bc, SUM(cd)cd, SUM(complete)complete, SUM(outcomes)outcomes, rg_name FROM (SELECT YEAR(pp_enrolled_on)YR, CASE WHEN ps_gender='male' THEN 1 ELSE 0 END males, CASE WHEN ps_gender='female' THEN 1 ELSE 0 END females, CASE WHEN pp_progam=1 THEN 1 ELSE 0 END tb, CASE WHEN pp_progam=2 THEN 1 ELSE 0 END mdr, CASE WHEN pp_type=3 THEN 1 ELSE 0 END pb, CASE WHEN pp_type=4 THEN 1 ELSE 0 END ep, CASE WHEN pp_confirmation=6 THEN 1 ELSE 0 END bc, CASE WHEN pp_confirmation=7 THEN 1 ELSE 0 END cd, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END complete, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END outcomes, rg_name FROM PatientProgram INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ON pt_person=ps_idnt INNER JOIN Facilities ON fc_idnt=pp_facility INNER JOIN Regions ON rg_idnt=fc_region WHERE pp_enrolled_on IS NOT NULL AND YEAR(pp_enrolled_on)>(YEAR(GETDATE())-5)) As Foo GROUP BY YR, rg_name ORDER BY YR DESC, rg_name");
        }

        public List<DataSummaryModel> GetDataSummaryAgency() {
            return this.GetDataSummary("SELECT YR, SUM(males)males, SUM(females)females, SUM(tb)tb, SUM(mdr)mdr, SUM(pb)pb, SUM(ep)ep, SUM(bc)bc, SUM(cd)cd, SUM(complete)complete, SUM(outcomes)outcomes, ag_name FROM (SELECT YEAR(pp_enrolled_on)YR, CASE WHEN ps_gender='male' THEN 1 ELSE 0 END males, CASE WHEN ps_gender='female' THEN 1 ELSE 0 END females, CASE WHEN pp_progam=1 THEN 1 ELSE 0 END tb, CASE WHEN pp_progam=2 THEN 1 ELSE 0 END mdr, CASE WHEN pp_type=3 THEN 1 ELSE 0 END pb, CASE WHEN pp_type=4 THEN 1 ELSE 0 END ep, CASE WHEN pp_confirmation=6 THEN 1 ELSE 0 END bc, CASE WHEN pp_confirmation=7 THEN 1 ELSE 0 END cd, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END complete, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END outcomes, ag_name FROM PatientProgram INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ON pt_person=ps_idnt INNER JOIN Facilities ON fc_idnt=pp_facility INNER JOIN Agency ON ag_idnt=fc_agency WHERE pp_enrolled_on IS NOT NULL AND YEAR(pp_enrolled_on)>(YEAR(GETDATE())-5)) As Foo GROUP BY YR, ag_name ORDER BY YR DESC, ag_name");
        }

        public List<DataSummaryModel> GetDataSummaryFacility() {
            return this.GetDataSummary("SELECT YR, SUM(males)males, SUM(females)females, SUM(tb)tb, SUM(mdr)mdr, SUM(pb)pb, SUM(ep)ep, SUM(bc)bc, SUM(cd)cd, SUM(complete)complete, SUM(outcomes)outcomes, fc_name FROM (SELECT YEAR(pp_enrolled_on)YR, CASE WHEN ps_gender='male' THEN 1 ELSE 0 END males, CASE WHEN ps_gender='female' THEN 1 ELSE 0 END females, CASE WHEN pp_progam=1 THEN 1 ELSE 0 END tb, CASE WHEN pp_progam=2 THEN 1 ELSE 0 END mdr, CASE WHEN pp_type=3 THEN 1 ELSE 0 END pb, CASE WHEN pp_type=4 THEN 1 ELSE 0 END ep, CASE WHEN pp_confirmation=6 THEN 1 ELSE 0 END bc, CASE WHEN pp_confirmation=7 THEN 1 ELSE 0 END cd, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END complete, CASE WHEN pp_dots_by=0 THEN 1 ELSE 0 END outcomes, fc_name FROM PatientProgram INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ON pt_person=ps_idnt INNER JOIN Facilities ON fc_idnt=pp_facility WHERE pp_enrolled_on IS NOT NULL AND YEAR(pp_enrolled_on)>(YEAR(GETDATE())-5)) As Foo GROUP BY YR, fc_name ORDER BY YR DESC, fc_name");
        }

        public Contacts GetContact(long idnt) {
            Contacts contact = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_uuid, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_uuid, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id WHERE ct_idnt=" + idnt);
            if (dr.Read())
            {
                contact = new Contacts {
                    Id = Convert.ToInt64(dr[0]),
                    Uuid = dr[1].ToString(),
                    Identifier = dr[2].ToString(),
                    Notes = dr[3].ToString(),
                    ExposedOn = Convert.ToDateTime(dr[4]),
                    AddedOn = Convert.ToDateTime(dr[5]),
                    AddedBy = new Users(Convert.ToInt64(dr[6])),
                    Patient = new Patient(Convert.ToInt64(dr[7])),
                    NextVisit = Convert.ToDateTime(dr[8])
                };

                contact.Person = new Person {
                    Id = Convert.ToInt64(dr[9]),
                    Name = dr[10].ToString(),
                    Gender = dr[11].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[12])
                };

                contact.Status = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString());
                contact.Location = new Concept(Convert.ToInt64(dr[15]), dr[16].ToString());
                contact.Relation = new Concept(Convert.ToInt64(dr[17]), dr[18].ToString());
                contact.Proximity = new Concept(Convert.ToInt64(dr[19]), dr[20].ToString());
                contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[21]), dr[22].ToString());
                contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[23]), dr[24].ToString());

                contact.Index = new PatientProgram {
                    Id = Convert.ToInt64(dr[25]),
                    TbmuNumber = dr[26].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[27]),
                    Patient = new Patient(dr[28].ToString())
                };

                contact.Index.Patient.Person = new Person {
                    Id = Convert.ToInt64(dr[29]),
                    Name = dr[30].ToString(),
                    Gender = dr[31].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[32]),
                };
            }

            return contact;
        }

        public Contacts GetContact(string uuid) {
            Contacts contact = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_uuid, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_uuid, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id WHERE ct_uuid COLLATE SQL_Latin1_General_CP1_CS_AS LIKE '" + uuid + "'");
            if (dr.Read()) {
                contact = new Contacts {
                    Id = Convert.ToInt64(dr[0]),
                    Uuid = dr[1].ToString(),
                    Identifier = dr[2].ToString(),
                    Notes = dr[3].ToString(),
                    ExposedOn = Convert.ToDateTime(dr[4]),
                    AddedOn = Convert.ToDateTime(dr[5]),
                    AddedBy = new Users(Convert.ToInt64(dr[6])),
                    Patient = new Patient(Convert.ToInt64(dr[7])),
                    NextVisit = Convert.ToDateTime(dr[8])
                };

                contact.Person = new Person {
                    Id = Convert.ToInt64(dr[9]),
                    Name = dr[10].ToString(),
                    Gender = dr[11].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[12])
                };

                contact.Status = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString());
                contact.Location = new Concept(Convert.ToInt64(dr[15]), dr[16].ToString());
                contact.Relation = new Concept(Convert.ToInt64(dr[17]), dr[18].ToString());
                contact.Proximity = new Concept(Convert.ToInt64(dr[19]), dr[20].ToString());
                contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[21]), dr[22].ToString());
                contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[23]), dr[24].ToString());

                contact.Index = new PatientProgram {
                    Id = Convert.ToInt64(dr[25]),
                    TbmuNumber = dr[26].ToString(),
                    DateEnrolled = Convert.ToDateTime(dr[27]),
                    Patient = new Patient(dr[28].ToString())
                };

                contact.Index.Patient.Person = new Person {
                    Id = Convert.ToInt64(dr[29]),
                    Name = dr[30].ToString(),
                    Gender = dr[31].ToString().FirstCharToUpper(),
                    DateOfBirth = Convert.ToDateTime(dr[32]),
                };
            }

            return contact;
        }

        public string GetContactUuid(Contacts contact) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_uuid FROM Contacts WHERE ct_idnt=" + contact.Id);
            if (dr.Read())
                return dr[0].ToString();
            return "";
        }

        public List<Contacts> GetContacts(string start = "", string stop = "", string filter = "") {
            List<Contacts> contacts = new List<Contacts>();
            SqlServerConnection conn = new SqlServerConnection();

            string query = conn.GetQueryString(filter, "ct_identifier+'-'+ct_notes+'-'+p.ps_name+'-'+p.ps_gender+'-'+cs.cpt_name+'-'+cl.cpt_name+'-'+cr.cpt_name+'-'+cp.cpt_name+'-'+pp_tbmu+'-'+ps.ps_name", "pt_idnt IN (SELECT pp_patient FROM PatientProgram WHERE pp_facility IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + "))", true);
            string order = " ORDER BY ct_identifier, p.ps_name";

            if (!string.IsNullOrEmpty(start)) {
                query += " AND ct_added_on BETWEEN '" + DateTime.Parse(start) + "' AND '" + DateTime.Parse(stop) + "'";
                order = " ORDER BY CAST(ct_added_on AS DATE) DESC, ct_identifier, p.ps_name";
            }

            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_uuid, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_uuid, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id" + query + order);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Contacts contact = new Contacts {
                        Id = Convert.ToInt64(dr[0]),
                        Uuid = dr[1].ToString(),
                        Identifier = dr[2].ToString(),
                        Notes = dr[3].ToString(),
                        ExposedOn = Convert.ToDateTime(dr[4]),
                        AddedOn = Convert.ToDateTime(dr[5]),
                        AddedBy = new Users(Convert.ToInt64(dr[6])),
                        Patient = new Patient(Convert.ToInt64(dr[7])),
                        NextVisit = Convert.ToDateTime(dr[8])
                    };

                    contact.Person = new Person {
                        Id = Convert.ToInt64(dr[9]),
                        Name = dr[10].ToString(),
                        Gender = dr[11].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[12])
                    };

                    contact.Status = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString());
                    contact.Location = new Concept(Convert.ToInt64(dr[15]), dr[16].ToString());
                    contact.Relation = new Concept(Convert.ToInt64(dr[17]), dr[18].ToString());
                    contact.Proximity = new Concept(Convert.ToInt64(dr[19]), dr[20].ToString());
                    contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[21]), dr[22].ToString());
                    contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[23]), dr[24].ToString());

                    contact.Index = new PatientProgram {
                        Id = Convert.ToInt64(dr[25]),
                        TbmuNumber = dr[26].ToString(),
                        DateEnrolled = Convert.ToDateTime(dr[27]),
                        Patient = new Patient(dr[28].ToString())
                    };

                    contact.Index.Patient.Person = new Person {
                        Id = Convert.ToInt64(dr[29]),
                        Name = dr[30].ToString(),
                        Gender = dr[31].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[32]),
                    };

                    contact.Age = contact.GetAge();

                    contacts.Add(contact);
                }
            }

            return contacts;
        }

        public List<Contacts> GetContacts(Patient patient) {
            List<Contacts> contacts = new List<Contacts>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_uuid, ct_identifier, ct_notes, ct_exposed_from, ct_added_on, ct_added_by, ct_patient_id, ct_next_screening, p.ps_idnt, p.ps_name, p.ps_gender, p.ps_dob, cs.cpt_id, cs.cpt_name [status], cl.cpt_id, cl.cpt_name [location], cr.cpt_id, cr.cpt_name [relation], cp.cpt_id, cp.cpt_name [proximity], ct_desease_after, cd.cpt_name[disease_after], ct_prev_treated, ct.cpt_name[previously_treated], pp_idnt, pp_tbmu, pp_enrolled_on, pt_uuid, ps.ps_idnt, ps.ps_name, ps.ps_gender, ps.ps_dob FROM Contacts INNER JOIN Person p ON ct_person=p.ps_idnt INNER JOIN PatientProgram ON ct_index=pp_idnt INNER JOIN Patient ON pp_patient=pt_idnt INNER JOIN Person ps ON pt_person=ps.ps_idnt INNER JOIN Concept cs ON ct_status= cs.cpt_id INNER JOIN Concept cl ON ct_location= cl.cpt_id INNER JOIN Concept cr ON ct_relationship= cr.cpt_id INNER JOIN Concept cp ON ct_proximity=cp.cpt_id INNER JOIN Concept cd ON ct_desease_after=cd.cpt_id INNER JOIN Concept ct ON ct_prev_treated=ct.cpt_id WHERE pt_idnt=" + patient.Id + " ORDER BY ct_identifier, p.ps_name");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Contacts contact = new Contacts
                    {
                        Id = Convert.ToInt64(dr[0]),
                        Uuid = dr[1].ToString(),
                        Identifier = dr[2].ToString(),
                        Notes = dr[3].ToString(),
                        ExposedOn = Convert.ToDateTime(dr[4]),
                        AddedOn = Convert.ToDateTime(dr[5]),
                        AddedBy = new Users(Convert.ToInt64(dr[6])),
                        Patient = new Patient(Convert.ToInt64(dr[7])),
                        NextVisit = Convert.ToDateTime(dr[8])
                    };

                    contact.Person = new Person
                    {
                        Id = Convert.ToInt64(dr[9]),
                        Name = dr[10].ToString(),
                        Gender = dr[11].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[12])
                    };

                    contact.Status = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString());
                    contact.Location = new Concept(Convert.ToInt64(dr[15]), dr[16].ToString());
                    contact.Relation = new Concept(Convert.ToInt64(dr[17]), dr[18].ToString());
                    contact.Proximity = new Concept(Convert.ToInt64(dr[19]), dr[20].ToString());
                    contact.DiseaseAfter = new Concept(Convert.ToInt64(dr[21]), dr[22].ToString());
                    contact.PrevouslyTreated = new Concept(Convert.ToInt64(dr[23]), dr[24].ToString());

                    contact.Index = new PatientProgram {
                        Id = Convert.ToInt64(dr[25]),
                        TbmuNumber = dr[26].ToString(),
                        DateEnrolled = Convert.ToDateTime(dr[27]),
                        Patient = new Patient(dr[28].ToString())
                    };

                    contact.Index.Patient.Person = new Person {
                        Id = Convert.ToInt64(dr[29]),
                        Name = dr[30].ToString(),
                        Gender = dr[31].ToString().FirstCharToUpper(),
                        DateOfBirth = Convert.ToDateTime(dr[32]),
                    };

                    contacts.Add(contact);
                }
            }

            return contacts;
        }

        public DateTime GetContactsLastScreening(Contacts contact) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT MAX(ce_added_on)x FROM ContactsExaminations WHERE ce_contact=" + contact.Id);
            if (dr.Read()) {
                return Convert.ToDateTime(dr[0]);
            }

            return new DateTime(1900,1,1);
        }

        public ContactsExamination GetContactsExaminations(long idnt) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ce_idnt, ce_contact, ce_cough, ce_fever, ce_weight_loss, ce_night_sweats, ce_sputum_smear, sp.cpt_name, ce_ltbi, lt.cpt_name, ce_genxpert, gx.cpt_name, ce_xray_exam, xr.cpt_name, ISNULL(NULLIF(ce_preventive_regimen,'No'),'N/A')pt, ce_next_screening, ce_added_on, ce_added_by FROM ContactsExaminations INNER JOIN Concept sp ON sp.cpt_id=ce_sputum_smear INNER JOIN Concept lt ON lt.cpt_id=ce_ltbi INNER JOIN Concept gx ON gx.cpt_id=ce_genxpert INNER JOIN Concept xr ON xr.cpt_id=ce_xray_exam WHERE ce_idnt=" + idnt + " ORDER BY ce_added_on DESC, ce_idnt");           
            if (dr.Read()) {
                return new ContactsExamination {
                    Id = Convert.ToInt64(dr[0]),
                    Contact = new Contacts { Id = Convert.ToInt64(dr[1]) },
                    Cough = Convert.ToBoolean(dr[2]),
                    Fever = Convert.ToBoolean(dr[3]),
                    WeightLoss = Convert.ToBoolean(dr[4]),
                    NightSweat = Convert.ToBoolean(dr[5]),
                    SputumSmear = new Concept {
                        Id = Convert.ToInt64(dr[6]),
                        Name = dr[7].ToString()
                    },
                    LTBI = new Concept {
                        Id = Convert.ToInt64(dr[8]),
                        Name = dr[9].ToString()
                    },
                    GeneXpert = new Concept {
                        Id = Convert.ToInt64(dr[10]),
                        Name = dr[11].ToString()
                    },
                    XrayExam = new Concept {
                        Id = Convert.ToInt64(dr[12]),
                        Name = dr[13].ToString()
                    },
                    PreventiveTherapy = dr[14].ToString(),
                    NextScreening = Convert.ToDateTime(dr[15]),
                    AddedOn = Convert.ToDateTime(dr[16]),
                    AddedBy = new Users(Convert.ToInt64(dr[17]))
                };
            }

            return new ContactsExamination();
        }

        public List<ContactsExamination> GetContactsExaminations(Contacts contact) {
            List<ContactsExamination> examinations = new List<ContactsExamination>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ce_idnt, ce_contact, ce_cough, ce_fever, ce_weight_loss, ce_night_sweats, ce_sputum_smear, sp.cpt_name, ce_ltbi, lt.cpt_name, ce_genxpert, gx.cpt_name, ce_xray_exam, xr.cpt_name, ISNULL(NULLIF(ce_preventive_regimen,'No'),'N/A')pt, ce_next_screening, ce_added_on, ce_added_by FROM ContactsExaminations INNER JOIN Concept sp ON sp.cpt_id=ce_sputum_smear INNER JOIN Concept lt ON lt.cpt_id=ce_ltbi INNER JOIN Concept gx ON gx.cpt_id=ce_genxpert INNER JOIN Concept xr ON xr.cpt_id=ce_xray_exam WHERE ce_contact=" + contact.Id + " ORDER BY ce_added_on DESC, ce_idnt");
            if (dr.HasRows) { 
                while (dr.Read()) {
                    examinations.Add(new ContactsExamination {
                        Id = Convert.ToInt64(dr[0]),
                        Contact = new Contacts { Id = Convert.ToInt64(dr[1]) },
                        Cough = Convert.ToBoolean(dr[2]),
                        Fever = Convert.ToBoolean(dr[3]),
                        WeightLoss = Convert.ToBoolean(dr[4]),
                        NightSweat = Convert.ToBoolean(dr[5]),
                        SputumSmear = new Concept { 
                            Id = Convert.ToInt64(dr[6]),
                            Name = dr[7].ToString()
                        },
                        LTBI = new Concept {
                            Id = Convert.ToInt64(dr[8]),
                            Name = dr[9].ToString()
                        },
                        GeneXpert = new Concept
                        {
                            Id = Convert.ToInt64(dr[10]),
                            Name = dr[11].ToString()
                        },
                        XrayExam = new Concept
                        {
                            Id = Convert.ToInt64(dr[12]),
                            Name = dr[13].ToString()
                        },
                        PreventiveTherapy = dr[14].ToString(),
                        NextScreening = Convert.ToDateTime(dr[15]),
                        AddedOn = Convert.ToDateTime(dr[16]),
                        AddedBy = new Users(Convert.ToInt64(dr[17]))
                    });
                }
            }

            return examinations;
        }

        public List<ContactsRegister> GetContactsRegister() {
            List<ContactsRegister> register = new List<ContactsRegister>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ct_idnt, ct_uuid, ct_identifier, ct_exposed_from, st.cpt_name, lc.cpt_name, rl.cpt_name, da.cpt_name, pv.cpt_name, p.ps_name, p.ps_gender, p.ps_dob, pp_tbmu, pt_uuid, ps.ps_name, ce_cough, ce_fever, ce_weight_loss, ce_night_sweats, sp.cpt_name, lt.cpt_name, gx.cpt_name, xr.cpt_name, ce_preventive_regimen, ce_next_screening FROM Contacts INNER JOIN Person AS p ON ct_person = p.ps_idnt INNER JOIN PatientProgram ON ct_index = pp_idnt INNER JOIN Patient ON pp_patient = pt_idnt INNER JOIN Person AS ps ON pt_person=ps.ps_idnt INNER JOIN ContactsExaminations ON ce_contact=ct_idnt INNER JOIN Concept st ON st.cpt_id=ct_status INNER JOIN Concept lc ON lc.cpt_id=ct_location INNER JOIN Concept rl ON rl.cpt_id=ct_relationship INNER JOIN Concept da ON da.cpt_id=ct_desease_after INNER JOIN Concept pv ON pv.cpt_id=ct_prev_treated INNER JOIN Concept sp ON sp.cpt_id=ce_sputum_smear INNER JOIN Concept lt ON lt.cpt_id=ce_ltbi INNER JOIN Concept gx ON gx.cpt_id=ce_genxpert INNER JOIN Concept xr ON xr.cpt_id=ce_xray_exam WHERE pt_idnt IN (SELECT pp_patient FROM PatientProgram WHERE pp_facility IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + ")) ORDER BY ct_identifier, p.ps_name, ct_idnt");
            if (dr.HasRows) { 
                while (dr.Read()) {
                    register.Add(new ContactsRegister {
                        Contact = new Contacts {
                            Id = Convert.ToInt32(dr[0]),
                            Uuid = dr[1].ToString(),
                            Identifier = dr[2].ToString(),
                            ExposedOn = Convert.ToDateTime(dr[3]),
                            Status = new Concept { Name = dr[4].ToString() },
                            Location = new Concept { Name = dr[5].ToString() },
                            Relation = new Concept { Name = dr[6].ToString() },
                            DiseaseAfter = new Concept { Name = dr[7].ToString() },
                            PrevouslyTreated = new Concept { Name = dr[8].ToString() },
                            Person = new Person {
                                Name = dr[9].ToString(),
                                Gender = dr[10].ToString(),
                                DateOfBirth = Convert.ToDateTime(dr[11]),
                            },
                            Index = new PatientProgram {
                                TbmuNumber = dr[12].ToString(),
                                Patient = new Patient {
                                    Uuid = dr[13].ToString(),
                                    Person = new Person { Name = dr[14].ToString() }
                                }
                            }
                        },
                        Examination = new ContactsExamination {
                            Cough = Convert.ToBoolean(dr[15]),
                            Fever = Convert.ToBoolean(dr[16]),
                            WeightLoss = Convert.ToBoolean(dr[17]),
                            NightSweat = Convert.ToBoolean(dr[18]),
                            SputumSmear = new Concept { Name = dr[19].ToString() },
                            LTBI = new Concept { Name = dr[20].ToString() },
                            GeneXpert = new Concept { Name = dr[21].ToString() },
                            XrayExam = new Concept { Name = dr[22].ToString() },
                            PreventiveTherapy = dr[23].ToString(),
                            NextScreening = Convert.ToDateTime(dr[24]),
                        }
                    });
                }
            }
            return register;
        }

        public PersonAddress GetPersonAddress(Person person) {
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
        public Contacts RegisterContact(Contacts ctx) {
            SqlServerConnection conn = new SqlServerConnection();
            ctx.Id = conn.SqlServerUpdate("INSERT INTO Contacts (ct_person, ct_index, ct_exposed_from, ct_identifier, ct_location, ct_relationship, ct_proximity, ct_desease_after, ct_prev_treated, ct_next_screening, ct_added_by) output INSERTED.ct_idnt VALUES (" + ctx.Person.Id + ", " + ctx.Index.Id + ", '" + ctx.ExposedOn.Date + "', '" + ctx.Identifier + "', " + ctx.Location.Id + ", " + ctx.Relation.Id + ", " + ctx.Proximity.Id + ", " + ctx.DiseaseAfter.Id + ", " + ctx.PrevouslyTreated.Id + ", '" + ctx.NextVisit + "', " + Actor + ")");

            return ctx;
        }

        public Person SavePerson(Person ps) {
            SqlServerConnection conn = new SqlServerConnection();
            ps.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + ps.Id + ", @name NVARCHAR(250)='" + ps.Name + "', @gender NVARCHAR(10)='" + ps.Gender + "', @dob DATE='" + ps.DateOfBirth + "', @actor INT=" + Actor + "; IF NOT EXISTS (SELECT ps_idnt FROM Person WHERE ps_idnt=@idnt) BEGIN INSERT INTO Person(ps_name, ps_gender, ps_dob, ps_added_by, ps_estimate) output INSERTED.ps_idnt VALUES (@name, @gender, @dob, @actor, 1) END ELSE BEGIN UPDATE Person SET ps_name=@name, ps_gender=@gender, ps_dob=@dob output INSERTED.ps_idnt WHERE ps_idnt=@idnt END");

            return ps;
        }

        public PersonAddress SavePersonAddress(PersonAddress pa) {
            SqlServerConnection conn = new SqlServerConnection();
            pa.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + pa.Id + ", @psid INT=" + pa.Person.Id + ", @tels NVARCHAR(250)='" + pa.Telephone + "', @adds NVARCHAR(250)='" + pa.Address + "'; IF NOT EXISTS (SELECT pa_idnt FROM PersonAddress WHERE pa_idnt=@idnt) BEGIN INSERT INTO PersonAddress(pa_person, pa_default, pa_telephone, pa_address) output INSERTED.pa_idnt VALUES (@psid, 1, @tels, @adds) END ELSE BEGIN UPDATE PersonAddress SET pa_telephone=@tels, pa_address=@adds output INSERTED.pa_idnt WHERE pa_idnt=@idnt END");

            return pa;
        }

        public ContactsExamination SaveContactsExamination(ContactsExamination cx) {
            SqlServerConnection conn = new SqlServerConnection();
            cx.Id = conn.SqlServerUpdate("INSERT INTO ContactsExaminations (ce_contact, ce_cough, ce_fever, ce_weight_loss, ce_night_sweats, ce_ltbi, ce_sputum_smear, ce_genxpert, ce_xray_exam, ce_preventive_regimen, ce_next_screening, ce_added_by) output INSERTED.ce_idnt VALUES (" + cx.Contact.Id + ", '" + cx.Cough + "', '" + cx.Fever + "', '" + cx.WeightLoss + "', '" + cx.NightSweat + "', " + cx.LTBI.Id + ", " + cx.SputumSmear.Id + ", " + cx.GeneXpert.Id + ", " + cx.XrayExam.Id + ", '" + cx.PreventiveTherapy + "', '" + cx.NextScreening.Date + "', " + Actor + ")");

            return cx;
        }
    }
}
