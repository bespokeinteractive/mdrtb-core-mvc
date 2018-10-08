using System;

namespace EtbSomalia.Models
{
    public class PatientProgram
    {
        public Int64 Id { get; set; }
        public String TbmuNumber { get; set; }
        public Patient Patient { get; set; }
        public Facility Facility { get; set; }
        public Concept Program { get; set; }
        public Concept Outcome { get; set; }
        public Concept Type { get; set; }
        public Concept Category { get; set; }

        public DateTime DateEnrolled { get; set; }
        public DateTime DateCompleted { get; set; }


        public PatientProgram()
        {
            DateEnrolled = DateTime.Now;
        }

        public PatientProgram Save(){


            return this;
        }
    }
}
