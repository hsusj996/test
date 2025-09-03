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

        public ObservableCollection<UnitViewModel> Units { get; } = new ObservableCollection<UnitViewModel>();

        public double DefaultTankCapacity { get; set; } = 100;
        public double DefaultTankInitialAmount { get; set; } = 0;

        public double StepSeconds
        {
            get => _simulationService.StepSeconds;
            set { _simulationService.StepSeconds = value; OnPropertyChanged(); }
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

        // Available IO lists (hardcoded 20 each for testing)
        public ObservableCollection<DigitalOutput> AvailableDos { get; } = new ObservableCollection<DigitalOutput>();
        public ObservableCollection<DigitalInput> AvailableDis { get; } = new ObservableCollection<DigitalInput>();

        public MainViewModel() : this(new SimulationService())
        {
        }

        public MainViewModel(ISimulationService simulationService)
        {
            _simulationService = simulationService;

            AddUnitCommand = new RelayCommand(_ => AddUnit());
            RemoveUnitCommand = new RelayCommand(u => RemoveUnit(u as UnitViewModel));
            AddTankToUnitCommand = new RelayCommand(u => AddTank(u as UnitViewModel));
            RemoveTankFromUnitCommand = new RelayCommand(t =>
            {
                if (t is object[] arr && arr.Length == 2)
                {
                    var unit = arr[0] as UnitViewModel;
                    var tank = arr[1] as TankViewModel;
                    RemoveTank(unit, tank);
                }
            });
            AddInletValveToTankCommand = new RelayCommand(t => AddValveToTank(t as TankViewModel, ValveType.Inlet));
            AddOutletValveToTankCommand = new RelayCommand(t => AddValveToTank(t as TankViewModel, ValveType.Outlet));
            AddSensorToTankCommand = new RelayCommand(t => AddSensorToTank(t as TankViewModel));
            ToggleValveCommand = new RelayCommand(v => ToggleValve(v as ValveViewModel));
            StartCommand = new RelayCommand(_ => Start());
            StopCommand = new RelayCommand(_ => Stop());

            // defaults
            SeedIo();
            AddUnit();
        }

        private void SeedIo()
        {
            for (int i = 1; i <= 20; i++)
            {
                AvailableDos.Add(new DigitalOutput($"DO_{i:00}"));
                AvailableDis.Add(new DigitalInput($"DI_{i:00}"));
            }
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
            if (uvm == null) return;
            var name = $"T{uvm.Tanks.Count + 1}";
            var tank = new Tank(name, DefaultTankCapacity, DefaultTankInitialAmount);
            var tvm = uvm.AddTank(tank);
            AddValveToTank(tvm, ValveType.Inlet);
            AddValveToTank(tvm, ValveType.Outlet);
            AddSensorToTank(tvm);
        }

        private void RemoveTank(UnitViewModel uvm, TankViewModel tvm)
        {
            if (uvm == null || tvm == null) return;
            uvm.RemoveTank(tvm);
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
    }
}


