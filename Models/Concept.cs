using System;
using System.Collections.Generic;

using EtbSomalia.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EtbSomalia.Models
{
    public class Concept
    {
        public long Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Synonyms { get; set; }
        public string Description { get; set; }
        public Concept Group { get; set; }

        readonly ConceptService service = new ConceptService();

        public Concept()
        {
            Id = 0;
            Uuid = Guid.NewGuid().ToString();
            Name = "";
            Synonyms = "";
            Description = "";
        }

        public Concept(long idx) : this() {
            Id = idx;
        }

        public Concept(long idx, string name) : this() {
            Id = idx;
            Name = name;
        }

        public Boolean HasAnswers(){
            return service.GetIfConceptHasAnswers(this);
        }

        public List<Concept> GetAnswers(){
            return service.GetConceptAnswers(this);
        }

        public IEnumerable<SelectListItem> GetAnswersIEnumerable(){
            return service.GetConceptAnswersIEnumerable(this);
        }
    }
}
