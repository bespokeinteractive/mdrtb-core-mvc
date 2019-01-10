using System;
namespace EtbSomalia.ViewModel
{
    public class PatientSearchViewModel
    {
        public string Query { get; set; }

        public PatientSearchViewModel() {
            Query = "";
        }
    }
}
