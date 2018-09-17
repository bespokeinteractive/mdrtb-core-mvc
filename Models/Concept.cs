using System;
using System.Collections.Generic;

using EtbSomalia.Services;

namespace EtbSomalia.Models
{
    public class Concept
    {
        public Int64 Id { get; set; }
        public String Uuid { get; set; }
        public String Name { get; set; }
        public String Synonyms { get; set; }
        public String Description { get; set; }
        public Concept Group { get; set; }

        readonly ConceptService svc = new ConceptService();

        public Concept()
        {
            Id = 0;
            Uuid = Guid.NewGuid().ToString();
            Name = "";
            Synonyms = "";
            Description = "";
        }

        public Concept(Int64 idx){
            Id = idx;
            Uuid = Guid.NewGuid().ToString();
            Name = "";
            Synonyms = "";
            Description = "";
        }

        public Boolean HasAnswers(){
            return svc.GetIfConceptHasAnswers(this);
        }

        public List<Concept> GetAnswers(){
            return svc.GetConceptAnswers(this);
        }
    }
}
