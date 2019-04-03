using EtbSomalia.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EtbSomalia.Models;

namespace EtbSomalia.Services
{
    public class InventoryDrugService
    {

        public List<FacilityDrug> GetInventoryDrugs(Facility facility, DrugCategory category, string filter="") {
            List<FacilityDrug> FacilityDrug = new List<FacilityDrug>();
            SqlServerConnection conn = new SqlServerConnection();

            string query = conn.GetQueryString(filter, "drg_initial+'-'+drg_name+'-'+dc_name+'-'+df_name+'-'+df_dosage", "fd_idnt>0");
            if (!(facility is null))
                query += " AND fd_facility=" + facility.Id;
            if (!(category is null))
                query += " AND dc_idnt=" + category.Id;

            SqlDataReader dr = conn.SqlServerConnect("SELECT fd_idnt, fd_reorder, ISNULL(avls,0)fd_avls, drg_idnt, drg_initial, drg_name, dc_idnt, dc_name, df_idnt, df_name, df_dosage FROM InventoryFacilityDrug INNER JOIN InventoryDrug ON fd_drug=drg_idnt INNER JOIN InventoryDrugCategory ON drg_category=dc_idnt INNER JOIN InventoryDrugFormulation ON drg_formulation=df_idnt LEFT OUTER JOIN vDrugsSummary ON fac=fd_facility AND drg=fd_drug " + query + " ORDER BY drg_name");
            if (dr.HasRows) {
               while (dr.Read()) {
                    FacilityDrug.Add(new FacilityDrug {
                        Id = Convert.ToInt64(dr[0]),
                        Reorder = Convert.ToInt64(dr[1]),
                        Available = Convert.ToInt64(dr[2]),
                        Drug = new Drug {
                            Id = Convert.ToInt64(dr[3]),
                            Initial = dr[4].ToString(),
                            Name = dr[5].ToString(),
                            Category = new DrugCategory { 
                                Id = Convert.ToInt64(dr[6]),
                                Name = dr[7].ToString() 
                            },
                            Formulation = new DrugFormulation { 
                                Id = Convert.ToInt64(dr[8]),
                                Name = dr[9].ToString(),
                                Dosage = dr[10].ToString(),
                            }                      
                        },
                    });
                }
            }

            return FacilityDrug;
        }
    }
}
