using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EtbSomalia.Extensions;
using EtbSomalia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Services
{
    public class ConceptService
    {
        public List<Concept> GetConceptAnswers(Concept concept){
            List<Concept> answers = new List<Concept>();

            return answers;
        }

        public List<SelectListItem>GetConceptAnswersIEnumerable(Concept concept)
        {
            List<SelectListItem> answers = new List<SelectListItem>();
            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT ca_answer, cpt_name FROM ConceptAnswers INNER JOIN Concept ON ca_answer = cpt_id WHERE ca_concept=" + concept.Id + " ORDER BY ca_order, ca_idnt");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    SelectListItem option = new SelectListItem();
                    option.Value = dr[0].ToString();
                    option.Text = dr[1].ToString();

                    answers.Add(option);
                }
            }

            return answers;
        }

        public Boolean GetIfConceptHasAnswers(Concept concept) {
            return false;
        }
    }
}
