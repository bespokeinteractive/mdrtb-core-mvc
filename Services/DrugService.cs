using EtbSomalia.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EtbSomalia.Models;

namespace EtbSomalia.Services
{
    public class DrugService {
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

        public List<DrugBatches> GetDrugBatches(Facility facility, DrugCategory category, DateTime? expiry, bool includeExpired = true, bool includeFlagged=true, string filter = "") {
            List<DrugBatches> batches = new List<DrugBatches>();
            SqlServerConnection conn = new SqlServerConnection();

            string query = conn.GetQueryString(filter, "idb_batch+'-'+idb_company+'-'+idb_supplier+'-'+drg_name+'-'+dc_name+'-'+df_name+'-'+df_dosage+'-'+fc_prefix+'-'+fc_name", "idb_idnt>0");
            if (!(facility is null))
                query += " AND fc_idnt=" + facility.Id;
            if (!(category is null))
                query += " AND dc_idnt=" + category.Id;
            if (expiry.HasValue)
                query += " AND idb_expiry<='" + Convert.ToDateTime(expiry).Date + "'";
            if (!includeExpired)
                query += " AND idb_expiry>'" + DateTime.Now.Date + "'";
            if (!includeFlagged)
                query += " AND idb_flag=0";

            SqlDataReader dr = conn.SqlServerConnect("SELECT idb_idnt, idb_batch, idb_company, idb_supplier, idb_manufacture, idb_expiry, idb_notes, drg_idnt, drg_initial, drg_name, dc_idnt, dc_name, df_idnt, df_name, df_dosage, fc_idnt, fc_prefix, fc_name, ISNULL(avls,0)x FROM InventoryDrugBatches INNER JOIN InventoryDrug ON idb_drug=drg_idnt INNER JOIN InventoryDrugCategory ON drg_category=dc_idnt INNER JOIN InventoryDrugFormulation ON drg_formulation=df_idnt INNER JOIN Facilities ON fc_idnt=idb_facility LEFT OUTER JOIN vBatchSummary ON batch=idb_idnt " + query + " ORDER BY idb_expiry, df_name, idb_facility");
            if (dr.HasRows) {
                while (dr.Read()) {
                    batches.Add(new DrugBatches {
                        Id = Convert.ToInt64(dr[0]),
                        BatchNo = dr[1].ToString(),
                        Company = dr[2].ToString(),
                        Supplier = dr[3].ToString(),
                        Manufacture = Convert.ToDateTime(dr[4]).ToString("MM/yy"),
                        DateOfManufacture = Convert.ToDateTime(dr[4]),
                        Expiry = Convert.ToDateTime(dr[5]).ToString("MM/yy"),
                        DateOfExpiry = Convert.ToDateTime(dr[5]),
                        Notes = dr[6].ToString(),

                        Drug = new Drug {
                            Id = Convert.ToInt64(dr[7]),
                            Initial = dr[8].ToString(),
                            Name = dr[9].ToString(),
                            Category = new DrugCategory {
                                Id = Convert.ToInt64(dr[10]),
                                Name = dr[11].ToString()
                            },

                            Formulation = new DrugFormulation {
                                Id = Convert.ToInt64(dr[12]),
                                Name = dr[13].ToString(),
                                Dosage = dr[14].ToString(),
                            }
                        },
                        Facility = new Facility {
                            Id = Convert.ToInt64(dr[15]),
                            Prefix = dr[16].ToString(),
                            Name = dr[17].ToString(),
                        },
                        Available = Convert.ToDouble(dr[18])
                    });
                }
            }
            return batches;
        }

        public List<DrugReceiptDetails> GetDrugReceiptDetails(Facility facility, DrugCategory category, DateTime? start_date, DateTime? end_date, string filter = "") {
            List<DrugReceiptDetails> receipts = new List<DrugReceiptDetails>();

            SqlServerConnection conn = new SqlServerConnection();

            string query = conn.GetQueryString(filter, "idb_batch+'-'+idb_company+'-'+idb_supplier+'-'+drg_name+'-'+dc_name+'-'+df_name+'-'+df_dosage+'-'+fc_prefix+'-'+fc_name", "idb_idnt>0");
            if (!(facility is null))
                query += " AND fc_idnt=" + facility.Id;
            if (!(category is null))
                query += " AND dc_idnt=" + category.Id;
            if (start_date.HasValue)
                query += " AND idr_date>='" + Convert.ToDateTime(start_date).Date + "'";
            if (end_date.HasValue)
                query += " AND idr_date<='" + Convert.ToDateTime(end_date).Date + "'";

            SqlDataReader dr = conn.SqlServerConnect("SELECT drd_idnt, drd_quantity, idr_idnt, idr_date, idr_receipt_no, idb_idnt, idb_batch, idb_company, idb_supplier, idb_manufacture, idb_expiry, drg_idnt, drg_initial, drg_name, df_idnt, df_name, df_dosage, dc_idnt, dc_name FROM InventoryDrugReceiptDetails INNER JOIN InventoryDrugReceipt ON drd_receipt=idr_idnt INNER JOIN InventoryDrugBatches ON drd_batch=idb_idnt INNER JOIN InventoryDrug ON idb_drug=drg_idnt INNER JOIN InventoryDrugFormulation ON drg_formulation=df_idnt INNER JOIN InventoryDrugCategory ON drg_category=dc_idnt " + query + " ORDER BY drd_idnt");
            if (dr.HasRows) {
                while (dr.Read()) {
                    receipts.Add(new DrugReceiptDetails {
                        Id = Convert.ToInt64(dr[0]),
                        Quantity = Convert.ToInt64(dr[1]),
                        Receipt = new DrugReceipt {
                            Id = Convert.ToInt64(dr[2]),
                            Date = Convert.ToDateTime(dr[3]),
                            Number = dr[4].ToString(),
                        },
                        Batch = new DrugBatches {
                            Id = Convert.ToInt64(dr[5]),
                            BatchNo = dr[6].ToString(),
                            Company = dr[7].ToString(),
                            Supplier = dr[8].ToString(),
                            DateOfManufacture = Convert.ToDateTime(dr[9]),
                            Manufacture = Convert.ToDateTime(dr[9]).ToString("dd/MM"),
                            DateOfExpiry = Convert.ToDateTime(dr[10]),
                            Expiry = Convert.ToDateTime(dr[10]).ToString("dd/MM"),
                            Drug = new Drug {
                                Id = Convert.ToInt64(dr[11]),
                                Initial = dr[12].ToString(),
                                Name = dr[13].ToString(),
                                Formulation = new DrugFormulation {
                                    Id = Convert.ToInt64(dr[14]),
                                    Name = dr[15].ToString(),
                                    Dosage = dr[16].ToString()
                                },
                                Category = new DrugCategory {
                                    Id = Convert.ToInt64(dr[17]),
                                    Name = dr[18].ToString()
                                }
                            }
                        }
                    });
                }
            }

            return receipts;
        }
    }
}
