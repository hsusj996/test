using System;
using super_rookie.Models.Status;

namespace super_rookie.Models.Module
{
    public enum ValveType
    {
        Inlet,
        Outlet
    }

    public class Valve
    {
        public string Name { get; set; }
        public bool IsOpen { get; set; }
        public double FlowRate { get; set; }
        public ValveType Direction { get; set; }
        public DigitalOutput CommandDo { get; set; }

        public Valve()
        {
            Name = string.Empty;
            IsOpen = false;
            FlowRate = 0;
            Direction = ValveType.Inlet;
            CommandDo = null;
        }
    }
}
