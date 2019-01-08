using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Services
{

    public class MdrtbCoreService
    {
        public int dbo { get; set; }
        public string dba { get; set; }

        public MdrtbCoreService() {}
        public MdrtbCoreService(HttpContext context) {
            dbo = int.Parse(context.User.FindFirst(ClaimTypes.Dsa).Value);
            dba = context.User.FindFirst(ClaimTypes.Dns).Value;
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

        public string GetNextTbmuNumber(Facility facility, DateTime DateEnrolled)
        {
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

        public List<Concept> GetFacilities() {
            List<Concept> answers = new List<Concept>();

            return answers;
        }

        public List<SelectListItem> GetFacilitiesIEnumerable()
        {
            List<SelectListItem> facilities = new List<SelectListItem>();
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT fc_idnt, fc_name FROM Facilities WHERE fc_status='active' ORDER BY fc_name");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    SelectListItem option = new SelectListItem
                    {
                        Value = dr[0].ToString(),
                        Text = dr[1].ToString()
                    };

                    facilities.Add(option);
                }
            }

            return facilities;
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
            SqlDataReader dr = conn.SqlServerConnect("SELECT pr_idnt, pr_default, pr_program, pr_regimen, pr_started_on, pr_added_on, pr_added_by, pr_notes FROM PatientRegimens WHERE pr_idnt=" + idnt);
            if (dr.Read()) {
                regimen = new PatientRegimen {
                    Id = Convert.ToInt64(dr[0]),
                    Default = Convert.ToBoolean(dr[1]),
                    Program = new PatientProgram(Convert.ToInt64(dr[2])),
                    Regimen = new Regimen(Convert.ToInt64(dr[3])),
                    StartedOn = Convert.ToDateTime(dr[4]),
                    CreatedOn = Convert.ToDateTime(dr[5]),
                    CreatedBy = new Users(Convert.ToInt64(dr[6])),
                    Notes = dr[6].ToString()
                };
            }

            return regimen;
        }

        public PatientRegimen GetPatientRegimen(PatientProgram pp) {
            PatientRegimen regimen = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pr_idnt, pr_default, pr_program, pr_regimen, pr_started_on, pr_added_on, pr_added_by, pr_notes FROM PatientRegimens WHERE pr_default=1 AND pr_program=" + pp.Id);
            if (dr.Read()) {
                regimen = new PatientRegimen {
                    Id = Convert.ToInt64(dr[0]),
                    Default = Convert.ToBoolean(dr[1]),
                    Program = new PatientProgram(Convert.ToInt64(dr[2])),
                    Regimen = new Regimen(Convert.ToInt64(dr[3])),
                    StartedOn = Convert.ToDateTime(dr[4]),
                    CreatedOn = Convert.ToDateTime(dr[5]),
                    CreatedBy = new Users(Convert.ToInt64(dr[6])),
                    Notes = dr[7].ToString()
                };
            }

            return regimen;
        }

        //Read: PatientExamination
        public PatientExamination GetPatientExamination(long idnt) {
            PatientExamination px = null;
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pe_idnt, pe_weight, pe_height, pe_muac, pe_bmi, pe_program, pe_exam, pe_sputum_exam, sp.cpt_name, pe_sputum_date, pe_genexpert_exam, gx.cpt_name, pe_genexpert_date, pe_hiv_exam, hv.cpt_name, pe_hiv_date, pe_xray_exam, xr.cpt_name, pe_xray_date, pe_created_on, pe_created_by, pe_notes FROM PatientExamination INNER JOIN Concept sp ON pe_sputum_exam=sp.cpt_id INNER JOIN Concept gx ON pe_genexpert_exam=gx.cpt_id INNER JOIN Concept hv ON pe_hiv_exam=hv.cpt_id INNER JOIN Concept xr ON pe_xray_exam=xr.cpt_id WHERE pe_idnt=" + idnt);
            if (dr.Read()) {
                px = new PatientExamination {
                    Id = Convert.ToInt64(dr[0]),
                    Weight = Convert.ToDouble(dr[1]),
                    Height = Convert.ToDouble(dr[2]),
                    MUAC = Convert.ToDouble(dr[3]),
                    BMI = Convert.ToDouble(dr[4]),
                    Program = new PatientProgram(Convert.ToInt64(dr[5])),
                    Exam = new Examination(Convert.ToInt64(dr[6])),
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

        public PatientExamination GetPatientExamination(PatientProgram pp, Examination ex) {
            PatientExamination px = null;
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT pe_idnt, pe_weight, pe_height, pe_muac, pe_bmi, pe_program, pe_exam, pe_sputum_exam, sp.cpt_name, pe_sputum_date, pe_genexpert_exam, gx.cpt_name, pe_genexpert_date, pe_hiv_exam, hv.cpt_name, pe_hiv_date, pe_xray_exam, xr.cpt_name, pe_xray_date, pe_created_on, pe_created_by, pe_notes FROM PatientExamination INNER JOIN Concept sp ON pe_sputum_exam=sp.cpt_id INNER JOIN Concept gx ON pe_genexpert_exam=gx.cpt_id INNER JOIN Concept hv ON pe_hiv_exam=hv.cpt_id INNER JOIN Concept xr ON pe_xray_exam=xr.cpt_id WHERE pe_program=" + pp.Id + " AND pe_exam=" + ex.Id);
            if (dr.Read()) {
                px = new PatientExamination {
                    Id = Convert.ToInt64(dr[0]),
                    Weight = Convert.ToDouble(dr[1]),
                    Height = Convert.ToDouble(dr[2]),
                    MUAC = Convert.ToDouble(dr[3]),
                    BMI = Convert.ToDouble(dr[4]),
                    Program = new PatientProgram(Convert.ToInt64(dr[5])),
                    Exam = new Examination(Convert.ToInt64(dr[6])),
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

        //Data Write
        public PatientProgram CreatePatientProgram(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            pp.Id = conn.SqlServerUpdate("INSERT INTO PatientProgram (pp_tbmu, pp_patient, pp_facility, pp_progam, pp_category, pp_type, pp_confirmation, pp_enrolled_on, pp_created_by) output INSERTED.pp_idnt VALUES ('" + pp.TbmuNumber + "', " + pp.Patient.Id + ", " + pp.Facility.Id + ", " + pp.Program.Id + ", " + pp.Category.Id + ", " + pp.Type.Id + ", " + pp.Confirmation.Id + ", '" + pp.DateEnrolled.Date + "', " + dbo + ")");

            conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_default=0 WHERE pp_patient=" + pp.Patient.Id + " AND pp_idnt<>" + pp.Id);

            return pp;
        }

        public PatientProgram UpdateIntake(PatientProgram pp) {
            SqlServerConnection conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientProgram SET pp_registeer_no='" + pp.RegisterNumber + "', pp_laboratory_no='" + pp.LaboratoryNumber + "', pp_dots_by=" + pp.DotsBy.Id + ", pp_referred_by=" + pp.ReferredBy.Id + ", pp_facility=" + pp.Facility.Id + ", pp_supporter='" + pp.TreatmentSupporter + "', pp_supporter_contacts='" + pp.SupporterContacts + "' output INSERTED.pp_idnt WHERE pp_idnt=" + pp.Id);

            return pp;
        }

        public PatientRegimen SavePatientRegimen(PatientRegimen pr) {
            SqlServerConnection conn = new SqlServerConnection();
            pr.Id = conn.SqlServerUpdate("IF NOT EXISTS (SELECT pr_idnt FROM PatientRegimens WHERE pr_idnt=" + pr.Id + ") BEGIN INSERT INTO PatientRegimens (pr_program, pr_regimen, pr_started_on, pr_added_by) output INSERTED.pr_idnt VALUES (" + pr.Program.Id + ", " + pr.Regimen.Id + ", '" + pr.StartedOn.Date + "', " + dbo + ") END ELSE BEGIN UPDATE PatientRegimens SET pr_regimen=" + pr.Regimen.Id + ", pr_started_on='" + pr.StartedOn.Date + "' output INSERTED.pr_idnt WHERE pr_idnt=" + pr.Id + " END");

            conn = new SqlServerConnection();
            conn.SqlServerUpdate("UPDATE PatientRegimens SET pr_default=0 WHERE pr_program=" + pr.Program.Id + " AND pr_idnt<>" + pr.Id);

            return pr;
        }

        public PatientExamination SavePatientExamination(PatientExamination px) {
            SqlServerConnection conn = new SqlServerConnection();
            px.Id = conn.SqlServerUpdate("IF NOT EXISTS (SELECT pe_idnt FROM PatientExamination WHERE pe_idnt=" + px.Id + ") BEGIN INSERT INTO PatientExamination (pe_program, pe_exam, pe_weight, pe_height, pe_muac, pe_bmi, pe_sputum_exam, pe_sputum_date, pe_genexpert_exam, pe_genexpert_date, pe_hiv_exam, pe_hiv_date, pe_xray_exam, pe_xray_date, pe_created_by) output INSERTED.pe_idnt VALUES (" + px.Program.Id + ", " + px.Exam.Id + ", " + px.Weight + ", " + px.Height + ", " + px.MUAC + ", " + px.BMI + ", " + px.SputumSmear.Id + ", '" + px.SputumSmearDate + "', " + px.GeneXpert.Id + ", '" + px.GeneXpertDate + "', " + px.HivExam.Id + ", '" + px.HivExamDate + "', " + px.XrayExam.Id + ", '" + px.XrayExamDate + "', " + dbo + ") END ELSE BEGIN UPDATE PatientExamination SET pe_weight=" + px.Weight + ", pe_height=" + px.Height + ", pe_muac=" + px.MUAC + ", pe_bmi=" + px.BMI + ", pe_sputum_exam=" + px.SputumSmear.Id + ", pe_sputum_date='" + px.SputumSmearDate + "', pe_genexpert_exam=" + px.GeneXpert.Id + ", pe_genexpert_date='" + px.GeneXpertDate + "', pe_hiv_exam=" + px.HivExam.Id + ", pe_hiv_date='" + px.HivExamDate + "', pe_xray_exam=" + px.XrayExam.Id + ", pe_xray_date='" + px.XrayExamDate + "' output INSERTED.pe_idnt WHERE pe_idnt=" + px.Id + " END");

            return px;
        }
    }
}
