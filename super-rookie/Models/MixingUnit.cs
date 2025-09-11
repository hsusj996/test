using System;
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

        public MixingUnit()
        {
            ChemId = string.Empty;
            IpAddress = string.Empty;
            Name = string.Empty;
        }

        public MixingUnit(string chemId, string ipAddress, string name)
        {
            ChemId = chemId ?? string.Empty;
            IpAddress = ipAddress ?? string.Empty;
            Name = name ?? string.Empty;
        }

        // Methods to manage status objects
        public void AddFunction(Function function)
        {
            if (function != null)
            {
                Functions.Add(function);
            }
        }

        public void AddDigitalInput(DigitalInput digitalInput)
        {
            if (digitalInput != null)
            {
                DigitalInputs.Add(digitalInput);
            }
        }

        public void AddDigitalOutput(DigitalOutput digitalOutput)
        {
            if (digitalOutput != null)
            {
                DigitalOutputs.Add(digitalOutput);
            }
        }

        public void AddAnalogInput(AnalogInput analogInput)
        {
            if (analogInput != null)
            {
                AnalogInputs.Add(analogInput);
            }
        }

        // Remove methods
        public void RemoveFunction(Function function)
        {
            if (function != null)
            {
                Functions.Remove(function);
            }
        }

        public void RemoveDigitalInput(DigitalInput digitalInput)
        {
            if (digitalInput != null)
            {
                DigitalInputs.Remove(digitalInput);
            }
        }

        public void RemoveDigitalOutput(DigitalOutput digitalOutput)
        {
            if (digitalOutput != null)
            {
                DigitalOutputs.Remove(digitalOutput);
            }
        }

        public void RemoveAnalogInput(AnalogInput analogInput)
        {
            if (analogInput != null)
            {
                AnalogInputs.Remove(analogInput);
            }
        }

        // Methods to manage module objects
        public void AddTank(Tank tank)
        {
            if (tank != null)
            {
                Tanks.Add(tank);
            }
        }

        public void AddValve(Valve valve)
        {
            if (valve != null)
            {
                Valves.Add(valve);
            }
        }

        public void AddLevelSensor(LevelSensor levelSensor)
        {
            if (levelSensor != null)
            {
                LevelSensors.Add(levelSensor);
            }
        }

        public void AddHeater(Heater heater)
        {
            if (heater != null)
            {
                Heaters.Add(heater);
            }
        }

        public void AddMixer(Mixer mixer)
        {
            if (mixer != null)
            {
                Mixers.Add(mixer);
            }
        }

        public void AddPump(Pump pump)
        {
            if (pump != null)
            {
                Pumps.Add(pump);
            }
        }

        // Remove module methods
        public void RemoveTank(Tank tank)
        {
            if (tank != null)
            {
                Tanks.Remove(tank);
            }
        }

        public void RemoveValve(Valve valve)
        {
            if (valve != null)
            {
                Valves.Remove(valve);
            }
        }

        public void RemoveLevelSensor(LevelSensor levelSensor)
        {
            if (levelSensor != null)
            {
                LevelSensors.Remove(levelSensor);
            }
        }

        public void RemoveHeater(Heater heater)
        {
            if (heater != null)
            {
                Heaters.Remove(heater);
            }
        }

        public void RemoveMixer(Mixer mixer)
        {
            if (mixer != null)
            {
                Mixers.Remove(mixer);
            }
        }

        public void RemovePump(Pump pump)
        {
            if (pump != null)
            {
                Pumps.Remove(pump);
            }
        }

        // Clear all status objects
        public void ClearAllStatus()
        {
            Functions.Clear();
            DigitalInputs.Clear();
            DigitalOutputs.Clear();
            AnalogInputs.Clear();
        }

        // Clear all module objects
        public void ClearAllModules()
        {
            Tanks.Clear();
            Valves.Clear();
            LevelSensors.Clear();
            Heaters.Clear();
            Mixers.Clear();
            Pumps.Clear();
        }

        // Clear all objects
        public void ClearAll()
        {
            ClearAllStatus();
            ClearAllModules();
        }
    }
}
