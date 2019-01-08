
using System;
using EtbSomalia.Extensions;
using EtbSomalia.Services;
using Microsoft.AspNetCore.Http;

namespace EtbSomalia.Models
{
    public class PatientProgram
    {
        public long Id { get; set; }
        public bool Default { get; set; }
        public string TbmuNumber { get; set; }
        public string RegisterNumber { get; set; }
        public string LaboratoryNumber { get; set; }
        public string TreatmentSupporter { get; set; }
        public string SupporterContacts { get; set; }

        public Patient Patient { get; set; }
        public Facility Facility { get; set; }
        public Programs Program { get; set; }
        public PatientProgram Referral { get; set; }

        public Concept ReferredBy { get; set; }
        public Concept DotsBy { get; set; }
        public Concept Confirmation { get; set; }
        public Concept Outcome { get; set; }
        public Concept Type { get; set; }
        public Concept Category { get; set; }

        public DateTime DateEnrolled { get; set; }
        public DateTime DateCompleted { get; set; }

        public int ArtStarted { get; set; }
        public int CptStarted { get; set; }

        public DateTime ArtStartedOn { get; set; }
        public DateTime CptStartedOn { get; set; }

        public DateTime CreatedOn { get; set; }
        public Users CreatedBy { get; set; }

        public PatientProgram() {
            Id = 0;
            Default = true;
            TbmuNumber = "";
            RegisterNumber = "";
            LaboratoryNumber = "";

            Patient = new Patient();
            Facility = new Facility();

            Program = new Programs();
            ReferredBy = new Concept();
            DotsBy = new Concept();
            Confirmation = new Concept();
            Outcome = new Concept();
            Type = new Concept();
            Category = new Concept();

            DateEnrolled = DateTime.Now;

            ArtStarted = 0;
            CptStarted = 0;

            ArtStartedOn = DateTime.Now;
            CptStartedOn = DateTime.Now;

            CreatedOn = DateTime.Now;
            CreatedBy = new Users();
        }

        public PatientProgram(Int64 idx) : this() {
            Id = idx;
        }

        public PatientProgram(Patient patient) : this() {
            Patient = patient;
        }

        public PatientProgram Create(HttpContext Context) {
            MdrtbCoreService core = new MdrtbCoreService(Context);
            TbmuNumber = core.GetNextTbmuNumber(Facility, DateEnrolled);

            return core.CreatePatientProgram(this);
        }

        public PatientProgram UpdateIntake(HttpContext Context) {
            MdrtbCoreService core = new MdrtbCoreService(Context);

            return core.UpdateIntake(this);
        }
    }
}
