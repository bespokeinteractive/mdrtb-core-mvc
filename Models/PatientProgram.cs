
using System;
using EtbSomalia.Extensions;
using EtbSomalia.Services;

namespace EtbSomalia.Models
{
    public class PatientProgram
    {
        public Int64 Id { get; set; }
        public String TbmuNumber { get; set; }
        public Patient Patient { get; set; }
        public Facility Facility { get; set; }
        public Concept Program { get; set; }
        public Concept Confirmation { get; set; }
        public Concept Outcome { get; set; }
        public Concept Type { get; set; }
        public Concept Category { get; set; }

        public DateTime DateEnrolled { get; set; }
        public DateTime DateCompleted { get; set; }


        //Services
        MdrtbCoreService dashboard = new MdrtbCoreService();

        public PatientProgram() {
            Id = 0;
            TbmuNumber = "";

            Patient = new Patient();
            Facility = new Facility();

            Program = new Concept();
            Confirmation = new Concept();
            Outcome = new Concept();
            Type = new Concept();
            Category = new Concept();


            DateEnrolled = DateTime.Now;
        }

        public PatientProgram(Int64 idx) : this() {
            Id = idx;
        }

        public PatientProgram(Patient patient) : this() {
            Patient = patient;
        }

        public PatientProgram Save(){
            TbmuNumber = dashboard.GetNextTbmuNumber(Facility, DateEnrolled);

            SqlServerConnection conn = new SqlServerConnection();
            Id = conn.SqlServerUpdate("INSERT INTO PatientProgram(pp_tbmu, pp_patient, pp_facility, pp_progam, pp_category, pp_type, pp_confirmation, pp_enrolled_on) output INSERTED.pp_idnt VALUES ('" + TbmuNumber  + "', " + Patient.Id + ", " + Facility.Id + ", " + Program.Id + ", " + Category.Id + ", " + Type.Id + ", " + Confirmation.Id + ", '" + DateEnrolled.Date + "')");

            return this;
        }
    }
}
