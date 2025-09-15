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
    }
}
