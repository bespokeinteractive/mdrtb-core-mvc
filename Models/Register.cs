using System;
namespace EtbSomalia.Models
{
    public class Register
    {
        public string DateCompleted { get; set; }

        public PatientProgram Program { get; set; }
        public RegisterExam Start { get; set; }
        public RegisterExam Two { get; set; }
        public RegisterExam Five { get; set; }
        public RegisterExam Last { get; set; }

        public Register() {
            DateCompleted = "—";

            Program = new PatientProgram();
            Start = new RegisterExam();
            Two = new RegisterExam();
            Five = new RegisterExam();
            Last = new RegisterExam();
        }
    }

    public class RegisterExam {
        public string Date { get; set; }
        public string SputumSmear { get; set; }
        public string GeneXpert { get; set; }
        public string HivExam { get; set; }
        public string XrayExam { get; set; }

        public RegisterExam() {
            Date = "—";
            SputumSmear = "—";
            HivExam = "—";
            XrayExam = "—";
        }
    }
}
