using System;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class PatientRegimen
    {
        public long Id { get; set; }
        public bool Default { get; set; }
        public PatientProgram Program { get; set; }
        public Regimen Regimen { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }
        public string Notes { get; set; }
            
        public PatientRegimen() {
            Id = 0;
            Default = true;
            Program = new PatientProgram();
            Regimen = new Regimen();
            StartedOn = DateTime.Now;
            CreatedOn = DateTime.Now;
            CreatedBy = new Users();
            Notes = "";
        }

        public PatientRegimen Save(HttpContext Context) {
            MdrtbCoreService core = new MdrtbCoreService(Context);
            return core.SavePatientRegimen(this);
        }
    }
}
