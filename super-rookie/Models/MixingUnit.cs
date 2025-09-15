using System.Collections.Generic;
using super_rookie.Models.Module;
using super_rookie.Models.Status;

namespace super_rookie.Models
{
    public class MixingUnit
    {
        public string ChemId { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }

        public MixingUnit(string chemId, string ipAddress, string name)
        {
            ChemId = chemId;
            IpAddress = ipAddress;
            Name = name;
        }

        // Status related data lists
        public List<Function> Functions { get; } = new List<Function>();
        public List<DigitalInput> DigitalInputs { get; } = new List<DigitalInput>();
        public List<DigitalOutput> DigitalOutputs { get; } = new List<DigitalOutput>();
        public List<AnalogInput> AnalogInputs { get; } = new List<AnalogInput>();

        // Module related data lists
        public List<Tank> Tanks { get; } = new List<Tank>();
        public List<Valve> Valves { get; } = new List<Valve>();
        public List<LevelSensor> LevelSensors { get; } = new List<LevelSensor>();
        public List<Heater> Heaters { get; } = new List<Heater>();
        public List<Mixer> Mixers { get; } = new List<Mixer>();
        public List<Pump> Pumps { get; } = new List<Pump>();
    }
}
