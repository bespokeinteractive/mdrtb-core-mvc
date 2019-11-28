using System;
using System.Collections.Generic;

namespace EtbSomalia.Models
{
    public class Dashboard
    {
        public decimal NewPatients { get; set; }
        public decimal AllPatients { get; set; }
        public decimal CaseNew { get; set; }
        public decimal CaseCurrent { get; set; }
        public decimal CasePrevious { get; set; }
        public decimal CasePercent { get; set; }
        public string CaseStatus { get; set; }

        public decimal NewCatg { get; set; }
        public decimal Relapse { get; set; }
        public decimal Failure { get; set; }
        public decimal Transfer { get; set; }

        public decimal Budget { get; set; }
        public decimal Usage { get; set; }
        public decimal Expenditure { get; set; }

        public decimal DrugsQuantity { get; set; }
        public decimal DrugsCosting { get; set; }

        public long Facility { get; set; }
        public long Female { get; set; }
        public long Male { get; set; }

        public long Children { get; set; }
        public long Youths { get; set; }
        public long Adults { get; set; }
        public long Seniors { get; set; }

        public List<string> MonthNames { get; set; }
        public List<int> MonthValue { get; set; }

        public Dashboard() {
            NewPatients = 0;
            AllPatients = 0;
            CaseNew = 0;
            CaseCurrent = 0;
            CasePercent = 0;
            CaseStatus = "Growth";

            Budget = 0;
            Usage = 0;
            Expenditure = 0;

            NewCatg = 0;
            Relapse = 0;
            Failure = 0;
            Transfer = 0;
            Facility = 0;

            DrugsQuantity = 0;
            DrugsCosting = 0;

            Male = 0;
            Female = 0;

            Children = 0;
            Youths = 0;
            Adults = 0;
            Seniors = 0;

            MonthNames = new List<string>();
            MonthValue = new List<int>();
        }
    }
}
