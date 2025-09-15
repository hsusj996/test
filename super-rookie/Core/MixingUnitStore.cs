using System.Collections.Generic;
using super_rookie.Models;

namespace super_rookie.Core
{
    public class MixingUnitStore
    {
        private static MixingUnitStore? _instance;
        private static readonly object _lock = new object();

        public List<MixingUnit> MixingUnits { get; set; }

        private MixingUnitStore()
        {
            MixingUnits = new List<MixingUnit>();
            InitializeTestData();
        }

        public static MixingUnitStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MixingUnitStore();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeTestData()
        {
            // Main Mixing Unit
            var mainUnit = new MixingUnit("CHEM001", "192.168.1.100", "Main Mixing Unit");
            InitializeMainUnitData(mainUnit);
            MixingUnits.Add(mainUnit);

            // Secondary Mixing Unit
            var secondaryUnit = new MixingUnit("CHEM002", "192.168.1.101", "Secondary Mixing Unit");
            InitializeSecondaryUnitData(secondaryUnit);
            MixingUnits.Add(secondaryUnit);

            // Backup Mixing Unit
            var backupUnit = new MixingUnit("CHEM003", "192.168.1.102", "Backup Mixing Unit");
            InitializeBackupUnitData(backupUnit);
            MixingUnits.Add(backupUnit);
        }

        private void InitializeMainUnitData(MixingUnit unit)
        {
            // Main Unit - 정상 운영 상태
            // Functions
            unit.Functions.Add(new Models.Status.Function { Id = 1, Name = "Auto Mode", Status = true });
            unit.Functions.Add(new Models.Status.Function { Id = 2, Name = "Manual Mode", Status = false });
            unit.Functions.Add(new Models.Status.Function { Id = 3, Name = "Emergency Stop", Status = false });

            // Digital Inputs
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 1, Name = "Tank Level High", Status = true });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 2, Name = "Temperature OK", Status = true });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 3, Name = "Pressure OK", Status = true });

            // Digital Outputs
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 1, Name = "Pump Running", Status = true });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 2, Name = "Heater On", Status = true });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 3, Name = "Valve Open", Status = true });

            // Analog Inputs
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 1, Name = "Temperature", Status = 75 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 2, Name = "Pressure", Status = 120 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 3, Name = "Flow Rate", Status = 45 });
        }

        private void InitializeSecondaryUnitData(MixingUnit unit)
        {
            // Secondary Unit - 일부 경고 상태
            // Functions
            unit.Functions.Add(new Models.Status.Function { Id = 1, Name = "Auto Mode", Status = false });
            unit.Functions.Add(new Models.Status.Function { Id = 2, Name = "Manual Mode", Status = true });
            unit.Functions.Add(new Models.Status.Function { Id = 3, Name = "Emergency Stop", Status = false });

            // Digital Inputs
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 1, Name = "Tank Level High", Status = false });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 2, Name = "Temperature OK", Status = false });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 3, Name = "Pressure OK", Status = true });

            // Digital Outputs
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 1, Name = "Pump Running", Status = false });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 2, Name = "Heater On", Status = true });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 3, Name = "Valve Open", Status = false });

            // Analog Inputs
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 1, Name = "Temperature", Status = 85 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 2, Name = "Pressure", Status = 95 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 3, Name = "Flow Rate", Status = 25 });
        }

        private void InitializeBackupUnitData(MixingUnit unit)
        {
            // Backup Unit - 대기 상태
            // Functions
            unit.Functions.Add(new Models.Status.Function { Id = 1, Name = "Auto Mode", Status = false });
            unit.Functions.Add(new Models.Status.Function { Id = 2, Name = "Manual Mode", Status = false });
            unit.Functions.Add(new Models.Status.Function { Id = 3, Name = "Emergency Stop", Status = true });

            // Digital Inputs
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 1, Name = "Tank Level High", Status = false });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 2, Name = "Temperature OK", Status = true });
            unit.DigitalInputs.Add(new Models.Status.DigitalInput { Id = 3, Name = "Pressure OK", Status = false });

            // Digital Outputs
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 1, Name = "Pump Running", Status = false });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 2, Name = "Heater On", Status = false });
            unit.DigitalOutputs.Add(new Models.Status.DigitalOutput { Id = 3, Name = "Valve Open", Status = false });

            // Analog Inputs
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 1, Name = "Temperature", Status = 22 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 2, Name = "Pressure", Status = 0 });
            unit.AnalogInputs.Add(new Models.Status.AnalogInput { Id = 3, Name = "Flow Rate", Status = 0 });
        }
    }
}
