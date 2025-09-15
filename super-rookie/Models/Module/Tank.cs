using System.Collections.Generic;

namespace super_rookie.Models.Module
{
    public class Tank
    {
        public string Name { get; set; }
        public double Capacity { get; set; }
        public double Amount { get; set; }
        public List<Valve> Valves { get; } = new List<Valve>();
    }
}
