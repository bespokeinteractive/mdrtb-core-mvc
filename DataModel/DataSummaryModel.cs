using System;
namespace EtbSomalia.DataModel
{
    public class DataSummaryModel
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public int Males { get; set; }
        public int Females { get; set; }
        public int Tb { get; set; }
        public int Mdr { get; set; }
        public int Pulmonary { get; set; }
        public int ExtraPulmonary { get; set; }
        public int BacterialConfirmed { get; set; }
        public int ClinicalDiagnosed { get; set; }
        public int Outcomes { get; set; }
        public int Complete { get; set; }

        public DataSummaryModel() {
            Name = "";
            Year = 0;
            Males = 0;
            Females = 0;
            Tb = 0;
            Mdr = 0;
            Pulmonary = 0;
            ExtraPulmonary = 0;
            BacterialConfirmed = 0;
            ClinicalDiagnosed = 0;
            Outcomes = 0;
            Complete = 0;
        }
    }
}
