using EtbSomalia.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EtbSomalia.Models;

namespace EtbSomalia.Services
{
    public class InventoryDrugService
    {

        public List<Drug> GetInventoryDrugs()
        {
            List<Drug> inventoryDrugs = new List<Drug>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("select IDD.drg_idnt, IDD.drg_name, IDC.dc_name , IDF.df_name , Idd.drg_description , IFD.fd_drug ,IFD.fd_facility from InventoryDrug as IDD inner join InventoryDrugCategory as IDC on IDD.drg_category = IDC.dc_idnt inner join InventoryDrugFormulation as IDF  on IDD.drg_formulation = IDF.df_idnt inner join InventoryFacilityDrug as IFD on IDD.drg_idnt = IFD.fd_drug ");
            if (dr.HasRows)
            {
               while (dr.Read())
                {
               
                    Drug drug = new Drug();
                    drug.Id = Convert.ToUInt16(dr[0]);
                    drug.Name = dr[1].ToString();
                    drug.Category.Name = dr[2].ToString();
                    drug.Formulation.Name = dr[3].ToString();                                 
                    inventoryDrugs.Add(drug);
                }

                
            }

            return inventoryDrugs;
        }
    }
}
