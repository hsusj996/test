using System;
using super_rookie.Models.Status;

namespace super_rookie.Models.Module
{
    public class LevelSensor
    {
        public string Name { get; set; }
        public double TriggerAmount { get; set; }
        public bool IsTriggered { get; set; }
        public DigitalInput StatusDi { get; set; }
        public Tank Tank { get; set; }

        public LevelSensor()
        {
            Name = string.Empty;
            TriggerAmount = 0;
            IsTriggered = false;
            StatusDi = null;
            Tank = null;
        }

        public LevelSensor(string name, Tank tank)
        {
            Name = name ?? string.Empty;
            TriggerAmount = 0;
            IsTriggered = false;
            StatusDi = null;
            Tank = tank;
        }

        public LevelSensor(string name, double triggerAmount, Tank tank)
        {
            Name = name ?? string.Empty;
            TriggerAmount = triggerAmount;
            IsTriggered = false;
            StatusDi = null;
            Tank = tank;
        }
    }
}
