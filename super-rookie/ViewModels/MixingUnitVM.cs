using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using super_rookie.Models;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels
{
    public partial class MixingUnitVM : ObservableObject
    {
        private readonly MixingUnit _model;

        public string ChemId => _model.ChemId;
        public string IpAddress => _model.IpAddress;
        public string Name => _model.Name;

        // Status ViewModels
        [ObservableProperty]
        private ObservableCollection<FunctionVM> _functions;

        [ObservableProperty]
        private ObservableCollection<DigitalInputVM> _digitalInputs;

        [ObservableProperty]
        private ObservableCollection<DigitalOutputVM> _digitalOutputs;

        [ObservableProperty]
        private ObservableCollection<AnalogInputVM> _analogInputs;

        // Module ViewModels
        [ObservableProperty]
        private ObservableCollection<TankVM> _tanks;

        [ObservableProperty]
        private ObservableCollection<ValveVM> _valves;

        [ObservableProperty]
        private ObservableCollection<LevelSensorVM> _levelSensors;

        [ObservableProperty]
        private ObservableCollection<HeaterVM> _heaters;

        [ObservableProperty]
        private ObservableCollection<MixerVM> _mixers;

        [ObservableProperty]
        private ObservableCollection<PumpVM> _pumps;

        // 선택된 모듈들
        [ObservableProperty]
        private TankVM? _selectedTank;

        [ObservableProperty]
        private ValveVM? _selectedValve;

        [ObservableProperty]
        private ObservableCollection<DigitalOutputVM> _availableDigitalOutputs;

        partial void OnSelectedTankChanged(TankVM? value)
        {
            UpdateAvailableValves();
        }

        partial void OnSelectedValveChanged(ValveVM? value)
        {
            UpdateAvailableDigitalOutputs();
        }

        private void UpdateAvailableValves()
        {
            _availableValves.Clear();
            
            if (SelectedTank == null)
            {
                foreach (var valve in _valves)
                {
                    _availableValves.Add(valve);
                }
            }
            else
            {
                foreach (var valve in _valves)
                {
                    if (!SelectedTank.Valves.Contains(valve))
                    {
                        _availableValves.Add(valve);
                    }
                }
            }
        }

        private void UpdateAvailableDigitalOutputs()
        {
            _availableDigitalOutputs.Clear();
            
            if (_selectedValve != null)
            {
                // 모든 DigitalOutput을 사용 가능한 목록에 추가
                foreach (var output in _digitalOutputs)
                {
                    _availableDigitalOutputs.Add(output);
                }
            }
        }

        // 탱크에 추가 가능한 밸브 목록 (이미 추가된 밸브 제외)
        [ObservableProperty]
        private ObservableCollection<ValveVM> _availableValves;

        public MixingUnitVM()
        {
            _model = new MixingUnit();
            _availableValves = new ObservableCollection<ValveVM>();
            _availableDigitalOutputs = new ObservableCollection<DigitalOutputVM>();
            InitializeCollections();
        }

        public MixingUnitVM(MixingUnit model)
        {
            _model = model ?? new MixingUnit();
            _availableValves = new ObservableCollection<ValveVM>();
            _availableDigitalOutputs = new ObservableCollection<DigitalOutputVM>();
            InitializeCollections();
            LoadFromModel();
        }

        public MixingUnitVM(string chemId, string ipAddress, string name)
        {
            _model = new MixingUnit(chemId, ipAddress, name);
            _availableValves = new ObservableCollection<ValveVM>();
            _availableDigitalOutputs = new ObservableCollection<DigitalOutputVM>();
            InitializeCollections();
        }

        private void InitializeCollections()
        {
            _functions = new ObservableCollection<FunctionVM>();
            _digitalInputs = new ObservableCollection<DigitalInputVM>();
            _digitalOutputs = new ObservableCollection<DigitalOutputVM>();
            _analogInputs = new ObservableCollection<AnalogInputVM>();
            _tanks = new ObservableCollection<TankVM>();
            _valves = new ObservableCollection<ValveVM>();
            _levelSensors = new ObservableCollection<LevelSensorVM>();
            _heaters = new ObservableCollection<HeaterVM>();
            _mixers = new ObservableCollection<MixerVM>();
            _pumps = new ObservableCollection<PumpVM>();
        }

        private void LoadFromModel()
        {
            // Load Status objects
            foreach (var function in _model.Functions)
            {
                _functions.Add(new FunctionVM(function));
            }

            foreach (var digitalInput in _model.DigitalInputs)
            {
                _digitalInputs.Add(new DigitalInputVM(digitalInput));
            }

            foreach (var digitalOutput in _model.DigitalOutputs)
            {
                _digitalOutputs.Add(new DigitalOutputVM(digitalOutput));
            }

            foreach (var analogInput in _model.AnalogInputs)
            {
                _analogInputs.Add(new AnalogInputVM(analogInput));
            }

            // Load Module objects
            foreach (var tank in _model.Tanks)
            {
                _tanks.Add(new TankVM(tank));
            }

            foreach (var valve in _model.Valves)
            {
                _valves.Add(new ValveVM(valve));
            }

            foreach (var levelSensor in _model.LevelSensors)
            {
                _levelSensors.Add(new LevelSensorVM(levelSensor));
            }

            foreach (var heater in _model.Heaters)
            {
                _heaters.Add(new HeaterVM(heater));
            }

            foreach (var mixer in _model.Mixers)
            {
                _mixers.Add(new MixerVM(mixer));
            }

            foreach (var pump in _model.Pumps)
            {
                _pumps.Add(new PumpVM(pump));
            }
        }

        // Status management methods
        public void AddFunction(FunctionVM function)
        {
            if (function != null)
            {
                _functions.Add(function);
            }
        }

        public void AddDigitalInput(DigitalInputVM digitalInput)
        {
            if (digitalInput != null)
            {
                _digitalInputs.Add(digitalInput);
            }
        }

        public void AddDigitalOutput(DigitalOutputVM digitalOutput)
        {
            if (digitalOutput != null)
            {
                _digitalOutputs.Add(digitalOutput);
            }
        }

        public void AddAnalogInput(AnalogInputVM analogInput)
        {
            if (analogInput != null)
            {
                _analogInputs.Add(analogInput);
            }
        }

        // Module management methods
        public void AddTank(TankVM tank)
        {
            if (tank != null)
            {
                _tanks.Add(tank);
            }
        }

        public void AddValve(ValveVM valve)
        {
            if (valve != null)
            {
                _valves.Add(valve);
            }
        }

        public void AddLevelSensor(LevelSensorVM levelSensor)
        {
            if (levelSensor != null)
            {
                _levelSensors.Add(levelSensor);
            }
        }

        public void AddHeater(HeaterVM heater)
        {
            if (heater != null)
            {
                _heaters.Add(heater);
            }
        }

        public void AddMixer(MixerVM mixer)
        {
            if (mixer != null)
            {
                _mixers.Add(mixer);
            }
        }

        public void AddPump(PumpVM pump)
        {
            if (pump != null)
            {
                _pumps.Add(pump);
            }
        }

        // Remove methods
        public void RemoveFunction(FunctionVM function)
        {
            if (function != null)
            {
                _functions.Remove(function);
            }
        }

        public void RemoveDigitalInput(DigitalInputVM digitalInput)
        {
            if (digitalInput != null)
            {
                _digitalInputs.Remove(digitalInput);
            }
        }

        public void RemoveDigitalOutput(DigitalOutputVM digitalOutput)
        {
            if (digitalOutput != null)
            {
                _digitalOutputs.Remove(digitalOutput);
            }
        }

        public void RemoveAnalogInput(AnalogInputVM analogInput)
        {
            if (analogInput != null)
            {
                _analogInputs.Remove(analogInput);
            }
        }

        public void RemoveTank(TankVM tank)
        {
            if (tank != null)
            {
                _tanks.Remove(tank);
            }
        }

        public void RemoveValve(ValveVM valve)
        {
            if (valve != null)
            {
                _valves.Remove(valve);
            }
        }

        public void RemoveLevelSensor(LevelSensorVM levelSensor)
        {
            if (levelSensor != null)
            {
                _levelSensors.Remove(levelSensor);
            }
        }

        public void RemoveHeater(HeaterVM heater)
        {
            if (heater != null)
            {
                _heaters.Remove(heater);
            }
        }

        public void RemoveMixer(MixerVM mixer)
        {
            if (mixer != null)
            {
                _mixers.Remove(mixer);
            }
        }

        public void RemovePump(PumpVM pump)
        {
            if (pump != null)
            {
                _pumps.Remove(pump);
            }
        }

        // Clear methods
        public void ClearAllStatus()
        {
            _functions.Clear();
            _digitalInputs.Clear();
            _digitalOutputs.Clear();
            _analogInputs.Clear();
        }

        public void ClearAllModules()
        {
            _tanks.Clear();
            _valves.Clear();
            _levelSensors.Clear();
            _heaters.Clear();
            _mixers.Clear();
            _pumps.Clear();
        }

        public void ClearAll()
        {
            ClearAllStatus();
            ClearAllModules();
        }

        // Get model with updated data
        public MixingUnit GetModel()
        {
            // Update status objects
            _model.Functions.Clear();
            _model.Functions.AddRange(_functions.Select(f => f.GetModel()));

            _model.DigitalInputs.Clear();
            _model.DigitalInputs.AddRange(_digitalInputs.Select(di => di.GetModel()));

            _model.DigitalOutputs.Clear();
            _model.DigitalOutputs.AddRange(_digitalOutputs.Select(do_ => do_.GetModel()));

            _model.AnalogInputs.Clear();
            _model.AnalogInputs.AddRange(_analogInputs.Select(ai => ai.GetModel()));

            // Update module objects
            _model.Tanks.Clear();
            _model.Tanks.AddRange(_tanks.Select(t => t.GetModel()));

            _model.Valves.Clear();
            _model.Valves.AddRange(_valves.Select(v => v.GetModel()));

            _model.LevelSensors.Clear();
            _model.LevelSensors.AddRange(_levelSensors.Select(ls => ls.GetModel()));

            _model.Heaters.Clear();
            _model.Heaters.AddRange(_heaters.Select(h => h.GetModel()));

            _model.Mixers.Clear();
            _model.Mixers.AddRange(_mixers.Select(m => m.GetModel()));

            _model.Pumps.Clear();
            _model.Pumps.AddRange(_pumps.Select(p => p.GetModel()));

            return _model;
        }

        // Commands
        [RelayCommand]
        private void AddTank()
        {
            var tankCount = _tanks.Count + 1;
            var tank = new TankVM($"Tank {tankCount:D2}", 1000, 0);
            _tanks.Add(tank);
        }

        [RelayCommand]
        private void AddValve()
        {
            var valveCount = _valves.Count + 1;
            var valve = new ValveVM($"Valve {valveCount:D2}", ValveType.Inlet, 10);
            _valves.Add(valve);
            UpdateAvailableValves();
        }

        [RelayCommand]
        private void AddLevelSensor()
        {
            var sensorCount = _levelSensors.Count + 1;
            // LevelSensor는 Tank가 필요하므로 첫 번째 Tank를 사용하거나 null
            var firstTank = _tanks.FirstOrDefault();
            var levelSensor = new LevelSensorVM($"Level Sensor {sensorCount:D2}", firstTank);
            _levelSensors.Add(levelSensor);
        }

        [RelayCommand]
        private void AddHeater()
        {
            var heaterCount = _heaters.Count + 1;
            var heater = new HeaterVM($"Heater {heaterCount:D2}");
            _heaters.Add(heater);
        }

        [RelayCommand]
        private void AddPump()
        {
            var pumpCount = _pumps.Count + 1;
            var pump = new PumpVM($"Pump {pumpCount:D2}");
            _pumps.Add(pump);
        }

        [RelayCommand]
        private void AddMixer()
        {
            var mixerCount = _mixers.Count + 1;
            var mixer = new MixerVM($"Mixer {mixerCount:D2}");
            _mixers.Add(mixer);
        }

        // 탱크 밸브 관리 Commands
        [RelayCommand]
        private void AddValveToTank(ValveVM valve)
        {
            if (SelectedTank != null && valve != null)
            {
                SelectedTank.Valves.Add(valve);
                UpdateAvailableValves();
            }
        }

        [RelayCommand]
        private void RemoveValveFromTank(ValveVM valve)
        {
            if (SelectedTank != null && valve != null)
            {
                SelectedTank.Valves.Remove(valve);
                UpdateAvailableValves();
            }
        }

        [RelayCommand]
        private void SelectDigitalOutput(DigitalOutputVM digitalOutput)
        {
            if (SelectedValve != null && digitalOutput != null)
            {
                SelectedValve.CommandDo = digitalOutput;
            }
        }

        // 설정 저장 Command
        [RelayCommand]
        private void SaveSettings()
        {
            try
            {
                // ViewModel의 변경사항을 Model에 동기화
                var updatedModel = GetModel();
                
                // Store에 업데이트된 모델 저장
                var store = Core.MixingUnitStore.Instance;
                var existingIndex = store.MixingUnits.FindIndex(m => m.ChemId == updatedModel.ChemId);
                if (existingIndex >= 0)
                {
                    store.MixingUnits[existingIndex] = updatedModel;
                }
                
                // 성공 메시지 박스 표시
                MessageBox.Show("설정이 저장되었습니다.", "저장 완료", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                // 에러 메시지 박스 표시
                MessageBox.Show($"저장 중 오류가 발생했습니다:\n{ex.Message}", "저장 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
