using System;
using System.Collections.Generic;
using EtbSomalia.DataModel;

namespace EtbSomalia.ViewModel
{
    public class PatientSummaryViewModel
    {
        public List<DataSummaryModel> Summary { get; set; }
        public string Title { get; set; }

        public PatientSummaryViewModel() {
            Title = "";
            Summary = new List<DataSummaryModel>();
        }
    }
}
