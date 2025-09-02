using System;

namespace super_rookie.Models
{
    public enum ValveType
    {
        Inlet,
        Outlet
    }

    public class Valve
    {
        public string Name { get; set; }

        public bool IsOpen { get; private set; }

        // Units: volume per second (e.g., L/s). Positive value.
        public double FlowRate { get; set; }

        public ValveType Direction { get; set; }

        // Optional: connected DO to command valve open/close
        public DigitalOutput CommandDo { get; set; }

        public Valve(string name, ValveType direction, double flowRate = 0)
        {
            Name = name;
            IsOpen = false;
            Direction = direction;
            FlowRate = flowRate < 0 ? 0 : flowRate;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
        }

        // Apply control from digital output (if connected)
        public void Update()
        {
            if (CommandDo == null) return;
            if (CommandDo.State)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }
}


