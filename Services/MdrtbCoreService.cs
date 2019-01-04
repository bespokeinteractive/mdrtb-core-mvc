using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Services
{

    public class MdrtbCoreService
    {  
        public MdrtbCoreService() {}

        public MdrtbCoreService(HttpContext context) {
            
        }

        public String GetFacilityPrefix(Facility facility)
        {
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT fc_prefix FROM Facilities WHERE fc_idnt=" + facility.Id);
            if (dr.Read()){
                return dr[0].ToString();
            }
            else {
                return "";
            }
        }

        public String GetNextTbmuNumber(Facility facility, DateTime DateEnrolled)
        {
            String prefix = GetFacilityPrefix(facility);
            Int64 Identity = 0;

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
    }
}
