using System;
using System.Collections.Generic;

namespace super_rookie.Models
{
    public class Tank
    {
        public string Name { get; set; }

        // Maximum capacity of the tank (same unit as Amount/Volume)
        public double Capacity { get; private set; }

        // Current liquid amount in the tank
        public double Amount { get; private set; }

        // Connected valves
        public List<Valve> Valves { get; } = new List<Valve>();

        // Optional: sensors inside tank
        public List<LevelSensor> LevelSensors { get; } = new List<LevelSensor>();

        public Tank(string name, double capacity, double initialAmount = 0)
        {
            if (capacity <= 0) throw new ArgumentException("Capacity must be positive");
            if (initialAmount < 0) throw new ArgumentException("Initial amount cannot be negative");
            if (initialAmount > capacity) initialAmount = capacity;

            Name = name;
            Capacity = capacity;
            Amount = initialAmount;
        }

        public void AttachValve(Valve valve)
        {
            if (valve == null) return;
            Valves.Add(valve);
        }

        public void AttachSensor(LevelSensor sensor)
        {
            if (sensor == null) return;
            LevelSensors.Add(sensor);
        }

        // Time-step simulation: compute amount change over time (seconds)
        // Do NOT call other objects' Update here; an external orchestrator should coordinate updates.
        public void Update(double seconds)
        {
            if (seconds <= 0) return;

            double netFlow = 0;
            foreach (var valve in Valves)
            {
                if (!valve.IsOpen || valve.FlowRate <= 0) continue;
                if (valve.Direction == ValveType.Inlet)
                {
                    netFlow += valve.FlowRate;
                }
                else if (valve.Direction == ValveType.Outlet)
                {
                    netFlow -= valve.FlowRate;
                }
            }

            double delta = netFlow * seconds;
            Amount += delta;
            if (Amount > Capacity) Amount = Capacity;
            if (Amount < 0) Amount = 0;
        }
    }
}


