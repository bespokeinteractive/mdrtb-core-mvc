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
    public class CoreService
    {
        private int Actor { get; set; }
        private string Username { get; set; }
        private ClaimsPrincipal User { get; set; }

        public CoreService() {}
        public CoreService(HttpContext context) {
            Actor = int.Parse(context.User.FindFirst(ClaimTypes.Actor).Value);
            User = context.User;
            Username = context.User.FindFirst(ClaimTypes.UserData).Value;
        }

        private string GetRolesCommand(string label = "", bool where = true) {
            if (User.IsInRole("Super User") || User.IsInRole("Administrator"))
                return "";
            if (User.IsInRole("Regional Admin"))
                return (where ? "WHERE " : "AND ") + label + " IN (SELECT fc_idnt FROM Facilities WHERE fc_region = " + User.FindFirst(ClaimTypes.Thumbprint).Value + ")";
            if (User.IsInRole("Agency Admin"))
                return (where ? "WHERE " : "AND ") + label + " IN (SELECT fc_idnt FROM Facilities WHERE fc_agency = " + User.FindFirst(ClaimTypes.Thumbprint).Value + ")";

            return (where ? "WHERE " : "AND ") + label + " IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + ")";
        }

        public Dashboard GetDashboardSummary() {
            Dashboard ds = new Dashboard();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @yr INT = YEAR(GETDATE()); SELECT COUNT(*)total, ISNULL(SUM(CASE WHEN ps_gender='male' THEN 1 ELSE 0 END),0)males, ISNULL(SUM(CASE WHEN ps_gender='female' THEN 1 ELSE 0 END),0)females, ISNULL(SUM(CASE WHEN age BETWEEN 0 AND 17 THEN 1 ELSE 0 END),0)children, ISNULL(SUM(CASE WHEN age BETWEEN 18 AND 34 THEN 1 ELSE 0 END),0)youths, ISNULL(SUM(CASE WHEN age BETWEEN 35 AND 64 THEN 1 ELSE 0 END),0)adults, ISNULL(SUM(CASE WHEN age>=65 THEN 1 ELSE 0 END),0)seniors, ISNULL(SUM(CASE WHEN pp_enrolled_on>=DATEADD(MONTH,-4,GETDATE()) THEN 1 ELSE 0 END),0)t_new, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=@yr THEN 1 ELSE 0 END),0)t_year, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=(@yr-1) THEN 1 ELSE 0 END),0)t_prev, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=@yr AND pp_category=10 THEN 1 ELSE 0 END),0)new, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=@yr AND pp_category=11 THEN 1 ELSE 0 END),0)relapse, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=@yr AND pp_category=12 THEN 1 ELSE 0 END),0)failr, ISNULL(SUM(CASE WHEN YEAR(pp_enrolled_on)=@yr AND pp_category=13 THEN 1 ELSE 0 END),0)transfr, COUNT(DISTINCT pp_facility)facility FROM (SELECT pp_facility, ps_gender, pp_category, pp_enrolled_on, DATEDIFF(hour,ps_dob,GETDATE())/8766 AS age FROM PatientProgram INNER JOIN Patient ON pt_idnt=pp_patient INNER JOIN Person ON pt_person=ps_idnt " + GetRolesCommand("pp_facility") + ") As Foo");
            if (dr.Read()) {
                ds = new Dashboard {
                    AllPatients = Convert.ToInt64(dr[0]), 
                    Male = Convert.ToInt64(dr[1]),
                    Female = Convert.ToInt64(dr[2]),
                    Children = Convert.ToInt64(dr[3]),
                    Youths = Convert.ToInt64(dr[4]),
                    Adults = Convert.ToInt64(dr[5]),
                    Seniors = Convert.ToInt64(dr[6]),
                    CaseNew = Convert.ToInt64(dr[7]),
                    CaseCurrent = Convert.ToInt64(dr[8]),
                    CasePrevious = Convert.ToInt64(dr[9]),
                    NewCatg = Convert.ToInt64(dr[10]),
                    Relapse = Convert.ToInt64(dr[11]),
                    Failure = Convert.ToInt64(dr[12]),
                    Transfer = Convert.ToInt64(dr[13]),
                    Facility = Convert.ToInt64(dr[14])
                };                    
            }

            if (ds.CaseCurrent == ds.CasePrevious || ds.CaseCurrent == 0)
                ds.CasePercent = 0;
            else if (ds.CasePrevious == 0)
                ds.CasePercent = 100;
            else if (ds.CaseCurrent > ds.CasePrevious)
                ds.CasePercent = (ds.CasePrevious / ds.CaseCurrent) * 100;
            else if (ds.CasePrevious > ds.CaseCurrent) {
                ds.CasePercent = 100 - ((ds.CaseCurrent / ds.CasePrevious) * 100);
                ds.CaseStatus = "Decrease";
            }

            conn = new SqlServerConnection();
            dr = conn.SqlServerConnect("SELECT mnth_prefix, COUNT(*) [count] FROM Months LEFT OUTER JOIN PatientProgram ON mnth_idnt=MONTH(pp_enrolled_on) AND YEAR(pp_enrolled_on)=YEAR(GETDATE()) " + GetRolesCommand("pp_facility", false) + " GROUP BY mnth_idnt, mnth_prefix, mnth_name ORDER BY mnth_idnt");
            if (dr.HasRows) {
                while (dr.Read()) {
                    ds.MonthNames.Add(dr[0].ToString());
                    ds.MonthValue.Add(Convert.ToInt32(dr[1]));
                }
            }

            conn = new SqlServerConnection();
            dr = conn.SqlServerConnect("SELECT COUNT(*) FROM Facilities WHERE fc_void=0 " + GetRolesCommand("fc_idnt", false));
            if (dr.Read()) {
                ds.TBMUs = Convert.ToInt64(dr[0]);
            }

            DashboardStats stats = new DashboardStats(); 
            conn = new SqlServerConnection();
            dr = conn.SqlServerConnect("SELECT mnth_idnt, ISNULL(Pul,0)Pul, ISNULL(Ep,0)Ep, ISNULL(Bc,0)Bc, ISNULL(Cd,0)Cd, ISNULL(Hiv,0)Hiv FROM Months LEFT OUTER JOIN (SELECT DATEPART(QUARTER, pp_enrolled_on)QT, SUM(CASE WHEN pp_type=3 THEN 1 ELSE 0 END)Pul, SUM(CASE WHEN pp_type=4 THEN 1 ELSE 0 END)Ep, SUM(CASE WHEN pp_confirmation=6 THEN 1 ELSE 0 END)Bc, SUM(CASE WHEN pp_confirmation=7 THEN 1 ELSE 0 END)Cd, SUM(CASE WHEN pe_hiv_exam=36 THEN 1 ELSE 0 END)Hiv FROM PatientProgram LEFT OUTER JOIN PatientExamination ON pp_idnt=pe_program WHERE YEAR(pp_enrolled_on)=YEAR(GETDATE()) " + GetRolesCommand("pp_facility", false) + " GROUP BY DATEPART(QUARTER, pp_enrolled_on)) As Foo ON QT=mnth_idnt");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var stat = new DashboardStats
                    {
                        Month = Convert.ToInt64(dr[0]),
                        Pul = Convert.ToInt64(dr[1]),
                        Ep = Convert.ToInt64(dr[2]),
                        Bc = Convert.ToInt64(dr[3]),
                        Cd = Convert.ToInt64(dr[4]),
                        Hiv = Convert.ToInt64(dr[5])
                    };

                    stats.Pul += stat.Pul;
                    stats.Ep += stat.Ep;
                    stats.Bc += stat.Bc;
                    stats.Cd += stat.Cd;
                    stats.Hiv += stat.Hiv;

                    ds.Stats.Add(stat);
                }
            }

            return ds;
        }

        public List<SelectListItem> GetIEnumerable(string query) {
            List<SelectListItem> ienumarable = new List<SelectListItem>();
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect(query);
            if (dr.HasRows) {
                while (dr.Read()) {
                    ienumarable.Add(new SelectListItem {
                        Value = dr[0].ToString(),
                        Text = dr[1].ToString()
                    });
                }
            }

            return ienumarable;
        }

        public string GetFacilityPrefix(Facility facility) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT fc_prefix FROM Facilities WHERE fc_idnt=" + facility.Id);
            if (dr.Read()){
                return dr[0].ToString();
            }
            else {
                return "";
            }
        }

        public string GetNextTbmuNumber(Facility facility, DateTime DateEnrolled) {
            string prefix = GetFacilityPrefix(facility);
            long Identity = 0;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT SUBSTRING(pp_tbmu,13,20)x FROM PatientProgram WHERE SUBSTRING(CAST(pp_enrolled_on AS NVARCHAR),1,7)='" + DateEnrolled.ToString("yyyy-MM") + "' AND pp_facility=" + facility.Id);
            if (dr.Read()) {
                Identity = Convert.ToInt64(dr[0]);
            }

            return prefix + "/" + DateEnrolled.ToString("yy/") + GetQuarter(DateEnrolled) + "/" + (Identity +1).ToString().PadLeft(4,'0') ;
        }

        public static string GetQuarter(DateTime date) {
            if (date.Month >= 1 && date.Month <= 3)
                return "01";
            else if (date.Month >= 4 && date.Month <= 6)
                return "02";
            else if (date.Month >= 7 && date.Month <= 9)
                return "03";
            else
                return "04";
        }


        //FACILITIES
        public Facility GetFacility(long idnt) {
            Facility facility = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT fc_idnt, fc_void, fc_status, fc_prefix, fc_name, fc_description, ISNULL(pp_last_record,'1900-01-01') fc_last, ISNULL(pp_count,0) fc_count, rg_idnt, rg_name, ag_idnt, ag_name FROM Facilities INNER JOIN Agency ON fc_agency=ag_idnt INNER JOIN Regions ON rg_idnt=fc_region LEFT OUTER JOIN vFacilitiesCount ON fc_idnt=pp_facility WHERE fc_idnt=" + idnt);
            if (dr.Read()) {
                facility = new Facility {
                    Id = Convert.ToInt64(dr[0]),
                    Void = Convert.ToBoolean(dr[1]),
                    Status = dr[2].ToString(),
                    Prefix = dr[3].ToString(),
                    Name = dr[4].ToString(),
                    Description = dr[5].ToString(),
                    LastRecord = Convert.ToDateTime(dr[6]),
                    Count = Convert.ToInt32(dr[7]),
                    Region = new Region (Convert.ToInt64(dr[8]), dr[9].ToString()),
                    Agency = new Agency (Convert.ToInt64(dr[10]), dr[11].ToString()),
                };
            }

            return facility;
        }

        public int CheckIfFacilityExists(Facility facility) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT COUNT(*) FROM Facilities WHERE fc_idnt<>" + facility.Id + " AND (fc_prefix LIKE '" + facility.Prefix + "' OR fc_name LIKE '" + facility.Name + "')");
            if (dr.Read())
            {
                return Convert.ToInt32(dr[0]);
            }

            return 0;
        }

        public List<Facility> GetFacilities(string filter = "", string agency = "", string region = "", bool includeVoided = true) {
            List<Facility> facilities = new List<Facility>();
            SqlServerConnection conn = new SqlServerConnection();

            string query = conn.GetQueryString(filter, "fc_status+'-'+fc_prefix+'-'+fc_name+'-'+fc_description+'-'+rg_name+'-'+ag_name", "fc_idnt>0");
            if (!string.IsNullOrWhiteSpace(agency))
                query += " AND fc_agency IN (" + agency + ")";
            if (!string.IsNullOrWhiteSpace(region))
                query += " AND fc_region IN (" + region + ")";
            if (!includeVoided)
                query += " AND fc_void=0";

            SqlDataReader dr = conn.SqlServerConnect("SELECT fc_idnt, fc_void, fc_status, fc_prefix, fc_name, fc_description, ISNULL(pp_last_record,'1900-01-01') fc_last, ISNULL(pp_count,0) fc_count, rg_idnt, rg_name, ag_idnt, ag_name FROM Facilities INNER JOIN Agency ON fc_agency=ag_idnt INNER JOIN Regions ON rg_idnt=fc_region LEFT OUTER JOIN vFacilitiesCount ON fc_idnt=pp_facility " + query + " ORDER BY rg_idnt, fc_prefix, fc_name");
            if (dr.HasRows) {
                while (dr.Read()) {
                    facilities.Add(new Facility {
                        Id = Convert.ToInt64(dr[0]),
                        Void = Convert.ToBoolean(dr[1]),
                        Status = dr[2].ToString(),
                        Prefix = dr[3].ToString(),
                        Name = dr[4].ToString(),
                        Description = dr[5].ToString(),
                        LastRecord = Convert.ToDateTime(dr[6]),
                        Count = Convert.ToInt32(dr[7]),
                        Region = new Region(Convert.ToInt64(dr[8]), dr[9].ToString()),
                        Agency = new Agency(Convert.ToInt64(dr[10]), dr[11].ToString()),
                    });
                }
            }

            return facilities;
        }

        public List<Facility> GetFacilitiesRandom(int count = 0) {
            List<Facility> facilities = new List<Facility>();
            string top = "";
            if (count > 0)
                top = "TOP(" + count + ")";

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT " + top + " pp_facility, fc_name, fc_prefix, ag_name, COUNT(*)pp_sum, SUM(CASE WHEN pp_outcome=0 THEN 0 ELSE 1 END) pp_outcome FROM PatientProgram INNER JOIN Facilities ON fc_idnt=pp_facility INNER JOIN Agency ON fc_agency=ag_idnt GROUP BY pp_facility, fc_prefix, fc_name, ag_name ORDER BY NEWID()");
            if (dr.HasRows) {
                while (dr.Read()) {
                    facilities.Add(new Facility {
                        Id = Convert.ToInt64(dr[0]),
                        Name = dr[1].ToString(),
                        Prefix = dr[2].ToString(),
                        Agency = new Agency { Name = dr[3].ToString() },
                        Count = Convert.ToInt32(dr[4]),
                        Outcomes = Convert.ToInt32(dr[5]),
                    });
                }
            }

            return facilities;
        }

        public List<SelectListItem> GetFacilitiesIEnumerable() {
            return GetIEnumerable("SELECT fc_idnt, fc_name FROM Facilities WHERE fc_status='active' AND fc_idnt IN (SELECT uf_facility FROM UsersFacilities WHERE uf_user=" + Actor + ") ORDER BY fc_name");
        }

        public List<SelectListItem> GetAllOtherCentersIEnumerable(Facility fac) {
            return GetIEnumerable("SELECT fc_idnt, fc_name FROM Facilities WHERE fc_status='active' AND fc_idnt<>" + fac.Id + " ORDER BY fc_name");
        }

        //AGENCIES
        public List<SelectListItem> GetAgenciesIEnumerable() {
            return GetIEnumerable("SELECT ag_idnt, ag_name FROM Agency WHERE ag_idnt<>0 ORDER BY ag_name");
        }

        //REGIONS
        public List<SelectListItem> GetRegionsIEnumerable() {
            return GetIEnumerable("SELECT rg_idnt, rg_name FROM Regions");
        }

        public Regimen GetRegimen(Int64 idnt) {
            Regimen regimen = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT rg_idnt, rg_program, rg_name, rg_description FROM Regimen WHERE rg_idnt=" + idnt);
            if (dr.Read()) {
                regimen = new Regimen
                {
                    Id = Convert.ToInt64(dr[0]),
                    Program = new Programs(Convert.ToInt64(dr[1])),
                    Name = dr[2].ToString(),
                    Description = dr[3].ToString()
                };
            }

            return regimen;
        }

        //Read: Regimen
        public List<Regimen> GetRegimens(Programs program) {
            List<Regimen> regimens = new List<Regimen>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT rg_idnt, rg_name, rg_description FROM Regimen WHERE rg_program=" + program.Id);
            if (dr.Read()) {
                Regimen regimen = new Regimen {
                    Id = Convert.ToInt64(dr[0]),
                    Name = dr[1].ToString(),
                    Description = dr[2].ToString()
                };

                regimens.Add(regimen);
            }

            return regimens;
        }

        public List<SelectListItem> GetRegimensIEnumerable(Programs program) {
            List<SelectListItem> regimens = new List<SelectListItem>();
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT rg_idnt, rg_name FROM Regimen WHERE rg_program=" + program.Id);
            if (dr.HasRows) {
                while (dr.Read()) {
                    SelectListItem option = new SelectListItem {
                        Value = dr[0].ToString(),
                        Text = dr[1].ToString()
                    };

                    regimens.Add(option);
                }
            }

            return regimens;
        }

        public PatientRegimen GetPatientRegimen(long idnt) {
            PatientRegimen regimen = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pr_idnt, pr_default, pr_program, pr_started_on, pr_added_on, pr_added_by, pr_notes, rg_idnt, rg_name, rg_description, prg_idnt, prg_description FROM PatientRegimens INNER JOIN Regimen ON pr_regimen=rg_idnt INNER JOIN Program ON rg_program=prg_idnt WHERE pr_idnt=" + idnt);
            if (dr.Read()) {
                regimen = new PatientRegimen {
                    Id = Convert.ToInt64(dr[0]),
                    Default = Convert.ToBoolean(dr[1]),
                    Program = new PatientProgram(Convert.ToInt64(dr[2])),
                    StartedOn = Convert.ToDateTime(dr[3]),
                    CreatedOn = Convert.ToDateTime(dr[4]),
                    CreatedBy = new Users(Convert.ToInt64(dr[5])),
                    Notes = dr[6].ToString()
                };

                regimen.Regimen = new Regimen {
                    Id = Convert.ToInt64(dr[7]),
                    Name = dr[8].ToString(),
                    Description = dr[9].ToString(),
                    Program = new Programs(Convert.ToInt64(dr[10]), dr[11].ToString())
                };
            }

            return regimen;
        }

        public PatientRegimen GetPatientRegimen(PatientProgram pp) {
            PatientRegimen regimen = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pr_idnt, pr_default, pr_program, pr_started_on, pr_added_on, pr_added_by, pr_notes, rg_idnt, rg_name, rg_description, prg_idnt, prg_description FROM PatientRegimens INNER JOIN Regimen ON pr_regimen=rg_idnt INNER JOIN Program ON rg_program=prg_idnt WHERE pr_default=1 AND pr_program=" + pp.Id);
            if (dr.Read()) {
                regimen = new PatientRegimen {
                    Id = Convert.ToInt64(dr[0]),
                    Default = Convert.ToBoolean(dr[1]),
                    Program = new PatientProgram(Convert.ToInt64(dr[2])),
                    StartedOn = Convert.ToDateTime(dr[3]),
                    CreatedOn = Convert.ToDateTime(dr[4]),
                    CreatedBy = new Users(Convert.ToInt64(dr[5])),
                    Notes = dr[6].ToString()
                };

                regimen.Regimen = new Regimen {
                    Id = Convert.ToInt64(dr[7]),
                    Name = dr[8].ToString(),
                    Description = dr[9].ToString(),
                    Program = new Programs(Convert.ToInt64(dr[10]), dr[11].ToString())
                };
            }

            return regimen;
        }

        //Read: Visits
        public List<SelectListItem> GetProgramVisitsIEnumerable(PatientProgram pp, Regimen regimen, bool includeVisited = false, bool includeLastVisit = false) {
            string query = "WHERE px_program=" + pp.Program.Id + " AND ex_regimen LIKE '%" + regimen.Id + "%'";

            if (!includeVisited)
                query += " AND ex_idnt NOT IN (SELECT pe_visit FROM PatientExamination WHERE pe_program=" + pp.Id + ")";

            if (!includeLastVisit)
                query += " AND ex_idnt<>25";

            return GetIEnumerable("SELECT ex_idnt, ex_name FROM ProgramExaminations INNER JOIN Examinations ON px_exam=ex_idnt " + query);
        }

        //Read: PatientExamination
        public PatientExamination GetPatientExamination(long idnt) {
            PatientExamination px = null;
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pe_idnt, pe_weight, pe_height, pe_muac, pe_bmi, pe_program, pe_visit, pe_sputum_exam, sp.cpt_name, pe_sputum_date, pe_genexpert_exam, gx.cpt_name, pe_genexpert_date, pe_hiv_exam, hv.cpt_name, pe_hiv_date, pe_xray_exam, xr.cpt_name, pe_xray_date, pe_created_on, pe_created_by, pe_notes FROM PatientExamination INNER JOIN Concept sp ON pe_sputum_exam=sp.cpt_id INNER JOIN Concept gx ON pe_genexpert_exam=gx.cpt_id INNER JOIN Concept hv ON pe_hiv_exam=hv.cpt_id INNER JOIN Concept xr ON pe_xray_exam=xr.cpt_id WHERE pe_idnt=" + idnt);
            if (dr.Read()) {
                px = new PatientExamination {
                    Id = Convert.ToInt64(dr[0]),
                    Weight = Convert.ToDouble(dr[1]),
                    Height = Convert.ToDouble(dr[2]),
                    MUAC = Convert.ToDouble(dr[3]),
                    BMI = Convert.ToDouble(dr[4]),
                    Program = new PatientProgram(Convert.ToInt64(dr[5])),
                    Visit = new Visit(Convert.ToInt64(dr[6])),
                    SputumSmear = new Concept(Convert.ToInt64(dr[7]), dr[8].ToString()),
                    SputumSmearDate = Convert.ToDateTime(dr[9]),
                    GeneXpert = new Concept(Convert.ToInt64(dr[10]), dr[11].ToString()),
                    GeneXpertDate = Convert.ToDateTime(dr[12]),
                    HivExam = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString()),
                    HivExamDate = Convert.ToDateTime(dr[15]),
                    XrayExam = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString()),
                    XrayExamDate = Convert.ToDateTime(dr[18]),
                    CreatedOn = Convert.ToDateTime(dr[19]),
                    CreatedBy = new Users(Convert.ToInt64(dr[20])),
                    Notes = dr[21].ToString()
                };
            }

            return px;
        }

        public PatientExamination GetPatientExamination(PatientProgram pp, Visit visit) {
            PatientExamination px = null;
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pe_idnt, pe_weight, pe_height, pe_muac, pe_bmi, pe_program, pe_visit, pe_sputum_exam, sp.cpt_name, pe_sputum_date, pe_genexpert_exam, gx.cpt_name, pe_genexpert_date, pe_hiv_exam, hv.cpt_name, pe_hiv_date, pe_xray_exam, xr.cpt_name, pe_xray_date, pe_created_on, pe_created_by, pe_notes FROM PatientExamination INNER JOIN Concept sp ON pe_sputum_exam=sp.cpt_id INNER JOIN Concept gx ON pe_genexpert_exam=gx.cpt_id INNER JOIN Concept hv ON pe_hiv_exam=hv.cpt_id INNER JOIN Concept xr ON pe_xray_exam=xr.cpt_id WHERE pe_program=" + pp.Id + " AND pe_visit=" + visit.Id);
            if (dr.Read()) {
                px = new PatientExamination {
                    Id = Convert.ToInt64(dr[0]),
                    Weight = Convert.ToDouble(dr[1]),
                    Height = Convert.ToDouble(dr[2]),
                    MUAC = Convert.ToDouble(dr[3]),
                    BMI = Convert.ToDouble(dr[4]),
                    Program = new PatientProgram(Convert.ToInt64(dr[5])),
                    Visit = new Visit(Convert.ToInt64(dr[6])),
                    SputumSmear = new Concept(Convert.ToInt64(dr[7]), dr[8].ToString()),
                    SputumSmearDate = Convert.ToDateTime(dr[9]),
                    GeneXpert = new Concept(Convert.ToInt64(dr[10]), dr[11].ToString()),
                    GeneXpertDate = Convert.ToDateTime(dr[12]),
                    HivExam = new Concept(Convert.ToInt64(dr[13]), dr[14].ToString()),
                    HivExamDate = Convert.ToDateTime(dr[15]),
                    XrayExam = new Concept(Convert.ToInt64(dr[16]), dr[17].ToString()),
                    XrayExamDate = Convert.ToDateTime(dr[18]),
                    CreatedOn = Convert.ToDateTime(dr[19]),
                    CreatedBy = new Users(Convert.ToInt64(dr[20])),
                    Notes = dr[21].ToString()
                };
            }

            return px;
        }

        public int GetPatientExaminationVisitsCount(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT COUNT(pe_program)x FROM PatientExamination WHERE pe_program=" + pp.Id);
            if (dr.Read()) {
                return Convert.ToInt32(dr[0]);
            }
            return 0;
        }

        //Examinations (Model)
        public Examinations GetRecentHivExamination(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @prog INT=" + pp.Id + "; SELECT TOP(1) pe_idnt, pe_hiv_date, 'HIV/AIDS TEST' pe_examination, pe_visit, ex_name, pe_labno, pe_hiv_exam, cpt_name FROM PatientExamination INNER JOIN Concept ON cpt_id=pe_hiv_exam INNER JOIN Examinations ON pe_visit=ex_idnt WHERE pe_program=@prog ORDER BY pe_hiv_date DESC, pe_idnt DESC");
            if (dr.Read()) {
                return new Examinations {
                    Id = Convert.ToInt64(dr[0]),
                    Date = Convert.ToDateTime(dr[1]),
                    Name = dr[2].ToString(),
                    Visit = new Visit { 
                        Id = Convert.ToInt64(dr[3]), 
                        Name = dr[4].ToString() 
                    },
                    LabNo = dr[5].ToString(),
                    Result = new Concept {
                        Id = Convert.ToInt64(dr[6]),
                        Name = dr[7].ToString()
                    }
                };
            }

            return null;
        }

        public List<Examinations> GetRecentExaminations(PatientProgram pp) {
            List<Examinations> exams = new List<Examinations>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @prog INT=" + pp.Id + "; SELECT * FROM ( SELECT * FROM ( SELECT TOP(1)pe_idnt, pe_sputum_date pe_exam_date, 'SPUTUM SMEAR' pe_examination, pe_visit, ex_name, pe_labno, pe_sputum_exam, cpt_name FROM PatientExamination INNER JOIN Concept ON cpt_id=pe_sputum_exam INNER JOIN Examinations ON pe_visit=ex_idnt WHERE pe_sputum_exam<>38 AND pe_program=@prog ORDER BY pe_sputum_date DESC, pe_idnt DESC ) As Foo UNION ALL SELECT * FROM ( SELECT TOP(1)pe_idnt, pe_genexpert_date, 'GENEXPERT' pe_examination, pe_visit, ex_name, pe_labno, pe_genexpert_exam, cpt_name FROM PatientExamination INNER JOIN Concept ON cpt_id=pe_genexpert_exam INNER JOIN Examinations ON pe_visit=ex_idnt WHERE pe_genexpert_exam<>38 AND pe_program=@prog ORDER BY pe_genexpert_date DESC, pe_idnt DESC) As Foo UNION ALL SELECT * FROM (SELECT TOP(1)pe_idnt, pe_hiv_date, 'HIV/AIDS TEST' pe_examination, pe_visit, ex_name, pe_labno, pe_hiv_exam, cpt_name FROM PatientExamination INNER JOIN Concept ON cpt_id=pe_hiv_exam INNER JOIN Examinations ON pe_visit=ex_idnt WHERE pe_hiv_exam NOT IN (38,42) AND pe_program=@prog ORDER BY pe_hiv_date DESC, pe_idnt DESC) As Foo UNION ALL SELECT * FROM (SELECT TOP(1)pe_idnt, pe_xray_date, 'X-RAY EXAM' pe_examination, pe_visit, ex_name, pe_labno, pe_xray_exam, cpt_name FROM PatientExamination INNER JOIN Concept ON cpt_id=pe_xray_exam INNER JOIN Examinations ON pe_visit=ex_idnt WHERE pe_xray_exam<>38 AND pe_program=@prog ORDER BY pe_xray_date DESC, pe_idnt DESC) As Foo) As Foo ORDER BY pe_exam_date DESC");
            if (dr.HasRows) {
                while (dr.Read()) {
                    exams.Add(new Examinations {
                        Id = Convert.ToInt64(dr[0]),
                        Date = Convert.ToDateTime(dr[1]),
                        Name = dr[2].ToString(),
                        Visit = new Visit {
                            Id = Convert.ToInt64(dr[3]),
                            Name = dr[4].ToString()
                        },
                        LabNo = dr[5].ToString(),
                        Result = new Concept {
                            Id = Convert.ToInt64(dr[6]),
                            Name = dr[7].ToString()
                        }
                    });
                }
            }

            return exams;
        }

        public List<SelectListItem> GetRolesIEnumerable() {
            return GetIEnumerable("SELECT rl_idnt, rl_name FROM Roles");
        }

        public AccountAdminViewModel InitializeAdminModel(AccountAdminViewModel model) {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("DECLARE @user INT, @facilty INT, @region INT, @agency INT; SELECT @user=COUNT(*) FROM Login; SELECT @facilty=COUNT(*) FROM Facilities WHERE fc_void=0; SELECT @region=COUNT(*) FROM Regions; SELECT @agency=COUNT(*) FROM Agency; SELECT @user usr, @facilty fc, @region rg, @agency ag");
            if (dr.Read()) {
                model.Users = Convert.ToInt32(dr[0]);
                model.Facilities = Convert.ToInt32(dr[1]);
                model.Regions = Convert.ToInt32(dr[2]);
                model.Agencies = Convert.ToInt32(dr[3]);
            }

            return model;
        }


        //Data Write
        public PatientProgram CreatePatientProgram(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            pp.Id = conn.SqlServerUpdate("INSERT INTO PatientProgram (pp_tbmu, pp_patient, pp_facility, pp_progam, pp_category, pp_type, pp_confirmation, pp_enrolled_on, pp_created_by) output INSERTED.pp_idnt VALUES ('" + pp.TbmuNumber + "', " + pp.Patient.Id + ", " + pp.Facility.Id + ", " + pp.Program.Id + ", " + pp.Category.Id + ", " + pp.Type.Id + ", " + pp.Confirmation.Id + ", '" + pp.DateEnrolled.Date + "', " + Actor + ")");

            conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_default=0 WHERE pp_patient=" + pp.Patient.Id + " AND pp_idnt<>" + pp.Id);

            return pp;
        }

        public void UpdateFacility(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_facility=" + pp.Facility.Id + " WHERE pp_idnt=" + pp.Id);
        }

        public void UpdateIntake(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_registeer_no='" + pp.RegisterNumber + "', pp_laboratory_no='" + pp.LaboratoryNumber + "', pp_dots_by=" + pp.DotsBy.Id + ", pp_referred_by=" + pp.ReferredBy.Id + ", pp_facility=" + pp.Facility.Id + ", pp_supporter='" + pp.TreatmentSupporter + "', pp_supporter_contacts='" + pp.SupporterContacts + "', pp_art_started=" + pp.ArtStarted + ", pp_art_started_on='" + pp.ArtStartedOn + "', pp_cpt_started=" + pp.CptStarted + ", pp_cpt_started_on='" + pp.CptStartedOn + "' WHERE pp_idnt=" + pp.Id);
        }

        public void UpdateVisit(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_laboratory_no='" + pp.LaboratoryNumber + "', pp_art_started=" + pp.ArtStarted + ", pp_art_started_on='" + pp.ArtStartedOn + "', pp_cpt_started=" + pp.CptStarted + ", pp_cpt_started_on='" + pp.CptStartedOn + "' WHERE pp_idnt=" + pp.Id);
        }

        public void UpdateOutcome(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_laboratory_no='" + pp.LaboratoryNumber + "', pp_completed_on='" + pp.DateCompleted + "', pp_outcome=" + pp.Outcome.Id + " WHERE pp_idnt=" + pp.Id);
        }

        public void UpdateTransfer(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_completed_on='" + pp.DateCompleted + "', pp_outcome=79 WHERE pp_idnt=" + pp.Id);
        }

        public PatientRegimen SavePatientRegimen(PatientRegimen pr) {
            SqlServerConnection conn = new SqlServerConnection();
            pr.Id = conn.SqlServerUpdate("DECLARE @prid INT=" + pr.Id + ", @prog INT=" + pr.Program.Id + ", @regm INT=" + pr.Regimen.Id + ", @user INT=" + Actor + ", @date DATE='" + pr.StartedOn.Date + "'; IF NOT EXISTS (SELECT pr_idnt FROM PatientRegimens WHERE pr_idnt=@prid AND pr_regimen=@regm) BEGIN INSERT INTO PatientRegimens (pr_program, pr_regimen, pr_started_on, pr_added_by) output INSERTED.pr_idnt VALUES (@prog, @regm, @date, @user) END ELSE BEGIN UPDATE PatientRegimens SET pr_started_on=@date output INSERTED.pr_idnt WHERE pr_idnt=@prid AND pr_regimen=@regm END");

            conn = new SqlServerConnection();
            conn.SqlServerUpdate("DECLARE @prid INT=" + pr.Id + ", @prog INT=" + pr.Program.Id + "; UPDATE PatientRegimens SET pr_default=0 WHERE pr_program=@prog AND pr_idnt<>@prid");

            return pr;
        }

        public PatientExamination SavePatientExamination(PatientExamination px) {
            SqlServerConnection conn = new SqlServerConnection();
            px.Id = conn.SqlServerUpdate("IF NOT EXISTS (SELECT pe_idnt FROM PatientExamination WHERE pe_idnt=" + px.Id + ") BEGIN INSERT INTO PatientExamination (pe_program, pe_visit, pe_labno, pe_weight, pe_height, pe_muac, pe_bmi, pe_sputum_exam, pe_sputum_date, pe_genexpert_exam, pe_genexpert_date, pe_hiv_exam, pe_hiv_date, pe_xray_exam, pe_xray_date, pe_created_by) output INSERTED.pe_idnt VALUES (" + px.Program.Id + ", " + px.Visit.Id + ", '" + px.LabNo + "', " + px.Weight + ", " + px.Height + ", " + px.MUAC + ", " + px.BMI + ", " + px.SputumSmear.Id + ", '" + px.SputumSmearDate + "', " + px.GeneXpert.Id + ", '" + px.GeneXpertDate + "', " + px.HivExam.Id + ", '" + px.HivExamDate + "', " + px.XrayExam.Id + ", '" + px.XrayExamDate + "', " + Actor + ") END ELSE BEGIN UPDATE PatientExamination SET pe_labno='" + px.LabNo + "', pe_weight=" + px.Weight + ", pe_height=" + px.Height + ", pe_muac=" + px.MUAC + ", pe_bmi=" + px.BMI + ", pe_sputum_exam=" + px.SputumSmear.Id + ", pe_sputum_date='" + px.SputumSmearDate + "', pe_genexpert_exam=" + px.GeneXpert.Id + ", pe_genexpert_date='" + px.GeneXpertDate + "', pe_hiv_exam=" + px.HivExam.Id + ", pe_hiv_date='" + px.HivExamDate + "', pe_xray_exam=" + px.XrayExam.Id + ", pe_xray_date='" + px.XrayExamDate + "' output INSERTED.pe_idnt WHERE pe_idnt=" + px.Id + " END");

            return px;
        }

        public Facility SaveFacility(Facility fac) {
            SqlServerConnection conn = new SqlServerConnection();
            fac.Id = conn.SqlServerUpdate("DECLARE @idnt INT=" + fac.Id + ", @rgns INT=" + fac.Region.Id + ", @agnt INT=" + fac.Agency.Id + ", @prfx NVARCHAR(50)='" + fac.Prefix + "', @name NVARCHAR(250)='" + fac.Name + "', @desc NVARCHAR(MAX)='" + fac.Description + "'; IF NOT EXISTS (SELECT fc_idnt FROM Facilities WHERE fc_idnt=@idnt) BEGIN INSERT INTO Facilities (fc_region, fc_agency, fc_prefix, fc_name, fc_description) output INSERTED.fc_idnt VALUES (@rgns, @agnt, @prfx, @name, @desc) END ELSE BEGIN UPDATE Facilities SET fc_region=@rgns, fc_agency=@agnt, fc_prefix=@prfx, fc_name=@name, fc_description=@desc output INSERTED.fc_idnt WHERE fc_idnt=@idnt END");

            return fac;
        }

        public void DeleteFacility(Facility fac) {
            SqlServerConnection conn = new SqlServerConnection();
            fac.Id = conn.SqlServerUpdate("DELETE FROM Facilities WHERE fc_idnt=" + fac.Id);
        }
    }
}
