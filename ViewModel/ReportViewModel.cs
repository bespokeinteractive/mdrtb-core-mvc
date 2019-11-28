using System;
using EtbSomalia.Models;

namespace EtbSomalia.ViewModel
{
    public class ReportViewModel
    {
        public Dashboard Dashboard { get; set; }
        public ReportViewModel()
        {
            Dashboard = new Dashboard();
        }
    }
}
