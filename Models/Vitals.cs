using System;
namespace EtbSomalia.Models
{
    public class Vitals
    {
        public string Weight { get; set; }
        public string Height { get; set; }
        public string WeightOn { get; set; }
        public string HeightOn { get; set; }

        public Vitals() {
            Weight = "N/A";
            Height = "N/A";

            WeightOn = "N/A";
            HeightOn = "N/A";
        }
    }
}
