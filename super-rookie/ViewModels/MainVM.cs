using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using super_rookie.Models;
using super_rookie.Services;
using super_rookie.ViewModels.Base;

namespace super_rookie.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ISimulationService _simulationService;
        private readonly SimulationStateService _stateService;
        private readonly NavigationService _navigationService;
        private int _selectedTabIndex = 0;

        public ObservableCollection<UnitViewModel> Units { get; } = new ObservableCollection<UnitViewModel>();

        public double DefaultTankCapacity { get; set; } = 100;
        public double DefaultTankInitialAmount { get; set; } = 0;

        public double StepSeconds
        {
            get => _simulationService.StepSeconds;
            set { _simulationService.StepSeconds = value; OnPropertyChanged(); }
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                NavigateToTab(value);
            }
        }

        public ICommand AddUnitCommand { get; }
        public ICommand RemoveUnitCommand { get; }
        public ICommand AddTankToUnitCommand { get; }
        public ICommand RemoveTankFromUnitCommand { get; }
        public ICommand AddInletValveToTankCommand { get; }
        public ICommand AddOutletValveToTankCommand { get; }
        public ICommand AddSensorToTankCommand { get; }
        public ICommand ToggleValveCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand StartSimulationCommand { get; }
        public ICommand NavigateToUnitCommand { get; }
        public ICommand NavigateToTankCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand LoadSettingsCommand { get; }
        public ICommand ExportSettingsCommand { get; }
        public ICommand ImportSettingsCommand { get; }
        public ICommand CreateFromTemplateCommand { get; }
        public ICommand IOMappingCommand { get; }
        public ICommand CopyTankCommand { get; }
        public ICommand MoveTankCommand { get; }
        public ICommand RemoveValveCommand { get; }
        public ICommand ConfigureValveCommand { get; }
        public ICommand RemoveSensorCommand { get; }
        public ICommand ConfigureSensorCommand { get; }

        // Available IO lists (hardcoded 20 each for testing)
        public ObservableCollection<DigitalOutput> AvailableDos { get; } = new ObservableCollection<DigitalOutput>();
        public ObservableCollection<DigitalInput> AvailableDis { get; } = new ObservableCollection<DigitalInput>();

        public MainViewModel() : this(new SimulationService(), null, null)
        {
        }

        public MainViewModel(ISimulationService simulationService, SimulationStateService stateService, NavigationService navigationService)
        {
            _simulationService = simulationService;
            _stateService = stateService;
            _navigationService = navigationService;

            AddUnitCommand = new RelayCommand(_ => AddUnit());
            RemoveUnitCommand = new RelayCommand(u => RemoveUnit(u as UnitViewModel));
            AddTankToUnitCommand = new RelayCommand(u => AddTank(u as UnitViewModel));
            RemoveTankFromUnitCommand = new RelayCommand(t => RemoveTank(t as TankViewModel));
            AddInletValveToTankCommand = new RelayCommand(t => AddValveToTank(t as TankViewModel, ValveType.Inlet));
            AddOutletValveToTankCommand = new RelayCommand(t => AddValveToTank(t as TankViewModel, ValveType.Outlet));
            AddSensorToTankCommand = new RelayCommand(t => AddSensorToTank(t as TankViewModel));
            ToggleValveCommand = new RelayCommand(v => ToggleValve(v as ValveViewModel));
            StartCommand = new RelayCommand(_ => Start());
            StopCommand = new RelayCommand(_ => Stop());
            StartSimulationCommand = new RelayCommand(_ => StartSimulation());
            NavigateToUnitCommand = new RelayCommand(u => NavigateToUnit(u as UnitViewModel));
            NavigateToTankCommand = new RelayCommand(t => NavigateToTank(t as TankViewModel));
            NavigateBackCommand = new RelayCommand(_ => NavigateBack());
            PauseCommand = new RelayCommand(_ => Pause());
            ResetCommand = new RelayCommand(_ => Reset());
            SaveSettingsCommand = new RelayCommand(_ => SaveSettings());
            LoadSettingsCommand = new RelayCommand(_ => LoadSettings());
            ExportSettingsCommand = new RelayCommand(_ => ExportSettings());
            ImportSettingsCommand = new RelayCommand(_ => ImportSettings());
            CreateFromTemplateCommand = new RelayCommand(_ => CreateFromTemplate());
            IOMappingCommand = new RelayCommand(_ => OpenIOMapping());
            CopyTankCommand = new RelayCommand(t => CopyTank(t as TankViewModel));
            MoveTankCommand = new RelayCommand(t => MoveTank(t as TankViewModel));
            RemoveValveCommand = new RelayCommand(v => RemoveValve(v as ValveViewModel));
            ConfigureValveCommand = new RelayCommand(v => ConfigureValve(v as ValveViewModel));
            RemoveSensorCommand = new RelayCommand(s => RemoveSensor(s as LevelSensorViewModel));
            ConfigureSensorCommand = new RelayCommand(s => ConfigureSensor(s as LevelSensorViewModel));

            // Initialize available IOs
            for (int i = 1; i <= 20; i++)
            {
                AvailableDos.Add(new DigitalOutput($"DO_{i:D2}"));
                AvailableDis.Add(new DigitalInput($"DI_{i:D2}"));
            }

            // Add default unit
            AddUnit();
        }

        private void NavigateToTab(int tabIndex)
        {
            if (_navigationService == null) return;

            switch (tabIndex)
            {
                case 0: // Configuration
                    _navigationService.NavigateTo("ConfigurationOverview");
                    break;
                case 1: // Simulation
                    _navigationService.NavigateTo("Simulation");
                    break;
                case 2: // Results
                    _navigationService.NavigateTo("Results");
                    break;
            }
        }

        private void NavigateToUnit(UnitViewModel unit)
        {
            if (_navigationService == null || unit == null) return;
            _navigationService.NavigateTo("UnitConfiguration", unit);
        }

        private void NavigateToTank(TankViewModel tank)
        {
            if (_navigationService == null || tank == null) return;
            _navigationService.NavigateTo("TankConfiguration", tank);
        }

        private void NavigateBack()
        {
            if (_navigationService == null) return;
            _navigationService.NavigateBack();
        }

        private void StartSimulation()
        {
            if (_stateService == null) return;
            _stateService.StartSimulation();
            SelectedTabIndex = 1; // Navigate to Simulation tab
        }

        private void AddUnit()
        {
            if (Units.Count >= 15) return;
            var unit = new Unit($"U{Units.Count + 1}");
            var uvm = new UnitViewModel(unit);
            Units.Add(uvm);
            // Seed with one tank by default
            AddTank(uvm);
        }

        private void RemoveUnit(UnitViewModel uvm)
        {
            if (uvm == null) return;
            Units.Remove(uvm);
        }

        private void AddTank(UnitViewModel uvm)
        {
            if (uvm == null) 
            {
                System.Windows.MessageBox.Show("유닛이 선택되지 않았습니다.");
                return;
            }
            
            var name = $"T{uvm.Tanks.Count + 1}";
            var tank = new Tank(name, DefaultTankCapacity, DefaultTankInitialAmount);
            var tvm = uvm.AddTank(tank);
            AddValveToTank(tvm, ValveType.Inlet);
            AddValveToTank(tvm, ValveType.Outlet);
            AddSensorToTank(tvm);
            
            System.Windows.MessageBox.Show($"탱크 '{name}'이 추가되었습니다.\n유닛: {uvm.Name}\n총 탱크 수: {uvm.Tanks.Count}");
        }

        private void RemoveTank(TankViewModel tvm)
        {
            if (tvm == null) 
            {
                System.Windows.MessageBox.Show("제거할 탱크를 선택해주세요.");
                return;
            }
            
            // Find the unit that contains this tank and remove it
            foreach (var unit in Units)
            {
                if (unit.Tanks.Contains(tvm))
                {
                    unit.RemoveTank(tvm);
                    System.Windows.MessageBox.Show($"탱크 '{tvm.Name}'이 제거되었습니다.");
                    return;
                }
            }
        }

        private void AddValveToTank(TankViewModel tvm, ValveType type)
        {
            if (tvm == null) return;
            var model = new Valve($"V_{type}_{tvm.Valves.Count(v => v.Direction == type)}", type, type == ValveType.Inlet ? 1.0 : 0.5)
            {
                CommandDo = new DigitalOutput($"DO_{tvm.Name}_{type}_{tvm.Valves.Count}")
            };
            tvm.AttachValve(model);
        }

        private void AddSensorToTank(TankViewModel tvm)
        {
            if (tvm == null) return;
            var model = new LevelSensor($"LS_{tvm.Sensors.Count}", 80)
            {
                StatusDi = new DigitalInput($"DI_{tvm.Name}_LS_{tvm.Sensors.Count}")
            };
            tvm.AttachSensor(model);
        }

        private void ToggleValve(ValveViewModel vvm)
        {
            if (vvm == null) return;
            vvm.DoState = !vvm.DoState;
        }

        private void Start()
        {
            var units = Units.Select(u => u.Model).ToList();
            _simulationService.Configure(units);
            _simulationService.Start();

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (s, e) =>
            {
                foreach (var u in Units)
                {
                    foreach (var t in u.Tanks) t.NotifyState();
                }
            };
            timer.Start();
        }

        private void Stop()
        {
            _simulationService.Stop();
        }

        private void Pause()
        {
            // TODO: Implement pause functionality
            System.Windows.MessageBox.Show("일시정지 기능은 아직 구현되지 않았습니다.");
        }

        private void Reset()
        {
            // TODO: Implement reset functionality
            System.Windows.MessageBox.Show("리셋 기능은 아직 구현되지 않았습니다.");
        }

        private void SaveSettings()
        {
            // TODO: Implement save settings functionality
            System.Windows.MessageBox.Show("설정 저장 기능은 아직 구현되지 않았습니다.");
        }

        private void LoadSettings()
        {
            // TODO: Implement load settings functionality
            System.Windows.MessageBox.Show("설정 불러오기 기능은 아직 구현되지 않았습니다.");
        }

        private void ExportSettings()
        {
            // TODO: Implement export settings functionality
            System.Windows.MessageBox.Show("설정 내보내기 기능은 아직 구현되지 않았습니다.");
        }

        private void ImportSettings()
        {
            // TODO: Implement import settings functionality
            System.Windows.MessageBox.Show("설정 가져오기 기능은 아직 구현되지 않았습니다.");
        }

        private void CreateFromTemplate()
        {
            // TODO: Implement create from template functionality
            System.Windows.MessageBox.Show("템플릿에서 생성 기능은 아직 구현되지 않았습니다.");
        }

        private void OpenIOMapping()
        {
            // TODO: Implement I/O mapping functionality
            System.Windows.MessageBox.Show("I/O 매핑 설정 기능은 아직 구현되지 않았습니다.");
        }

        private void CopyTank(TankViewModel tank)
        {
            if (tank == null) return;
            
            // Find the unit that contains this tank
            UnitViewModel sourceUnit = null;
            foreach (var unit in Units)
            {
                if (unit.Tanks.Contains(tank))
                {
                    sourceUnit = unit;
                    break;
                }
            }
            
            if (sourceUnit == null) return;
            
            // Create a copy of the tank
            var newTank = new Tank($"{tank.Name}_Copy", tank.Capacity, tank.Amount);
            var newTvm = sourceUnit.AddTank(newTank);
            
            // Copy valves
            foreach (var valve in tank.Valves)
            {
                var newValve = new Valve($"V_{valve.Direction}_{newTvm.Valves.Count(v => v.Direction == valve.Direction)}", 
                    valve.Direction, valve.FlowRate)
                {
                    CommandDo = new DigitalOutput($"DO_{newTank.Name}_{valve.Direction}_{newTvm.Valves.Count}")
                };
                newTvm.AttachValve(newValve);
            }
            
            // Copy sensors
            foreach (var sensor in tank.Sensors)
            {
                var newSensor = new LevelSensor($"LS_{newTvm.Sensors.Count}", sensor.TriggerAmount)
                {
                    StatusDi = new DigitalInput($"DI_{newTank.Name}_LS_{newTvm.Sensors.Count}")
                };
                newTvm.AttachSensor(newSensor);
            }
            
            System.Windows.MessageBox.Show($"탱크 '{tank.Name}'이 복사되었습니다.");
        }

        private void MoveTank(TankViewModel tank)
        {
            if (tank == null) return;
            
            // TODO: Implement tank movement between units
            System.Windows.MessageBox.Show("탱크 이동 기능은 아직 구현되지 않았습니다.");
        }

        private void RemoveValve(ValveViewModel valve)
        {
            if (valve == null) return;
            
            // Find the tank that contains this valve
            foreach (var unit in Units)
            {
                foreach (var tank in unit.Tanks)
                {
                    if (tank.Valves.Contains(valve))
                    {
                        tank.Valves.Remove(valve);
                        tank.Model.Valves.Remove(valve.Model);
                        System.Windows.MessageBox.Show($"밸브 '{valve.Name}'이 제거되었습니다.");
                        return;
                    }
                }
            }
        }

        private void ConfigureValve(ValveViewModel valve)
        {
            if (valve == null) return;
            
            // TODO: Implement valve configuration dialog
            System.Windows.MessageBox.Show($"밸브 '{valve.Name}' 설정 기능은 아직 구현되지 않았습니다.");
        }

        private void RemoveSensor(LevelSensorViewModel sensor)
        {
            if (sensor == null) return;
            
            // Find the tank that contains this sensor
            foreach (var unit in Units)
            {
                foreach (var tank in unit.Tanks)
                {
                    if (tank.Sensors.Contains(sensor))
                    {
                        tank.Sensors.Remove(sensor);
                        tank.Model.LevelSensors.Remove(sensor.Model);
                        System.Windows.MessageBox.Show($"센서 '{sensor.Name}'이 제거되었습니다.");
                        return;
                    }
                }
            }
        }

        private void ConfigureSensor(LevelSensorViewModel sensor)
        {
            if (sensor == null) return;
            
            // TODO: Implement sensor configuration dialog
            System.Windows.MessageBox.Show($"센서 '{sensor.Name}' 설정 기능은 아직 구현되지 않았습니다.");
        }
    }
}