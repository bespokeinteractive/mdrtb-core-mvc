using System;
namespace EtbSomalia.Models
{
    public class ProgramsExamination
    {
        public Int64 Id { get; set; }
        public Programs Program { get; set; }
        public Visit Exam { get; set; }
        public string Notes { get; set; }

        public ProgramsExamination() {
            Id = 0;
            Notes = "";
            Program = new Programs();
            Exam = new Visit();
        }
    }
}
