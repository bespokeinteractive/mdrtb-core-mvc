using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Services
{
    public class DashboardService
    {
        public List<Concept> GetFacilities()
        {
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
                    SelectListItem option = new SelectListItem();
                    option.Value = dr[0].ToString();
                    option.Text = dr[1].ToString();

                    facilities.Add(option);
                }
            }

            return facilities;
        }
    }
}
