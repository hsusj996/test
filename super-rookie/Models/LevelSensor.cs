using System;

namespace super_rookie.Models
{
    public class LevelSensor
    {
        public string Name { get; set; }

        // Trigger point within the tank (same unit as Amount)
        public double TriggerAmount { get; set; }

        public bool IsTriggered { get; private set; }

        // Optional: connected DI to report sensor state
        public DigitalInput StatusDi { get; set; }

        public LevelSensor(string name, double triggerAmount)
        {
            Name = name;
            TriggerAmount = triggerAmount;
            IsTriggered = false;
        }

        public void Update(double currentAmount)
        {
            IsTriggered = currentAmount >= TriggerAmount;
            if (StatusDi != null)
            {
                StatusDi.State = IsTriggered;
            }
        }
    }
}


