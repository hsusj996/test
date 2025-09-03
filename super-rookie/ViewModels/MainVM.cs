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

        public ObservableCollection<ValveViewModel> Valves { get; } = new ObservableCollection<ValveViewModel>();
        public ObservableCollection<LevelSensorViewModel> Sensors { get; } = new ObservableCollection<LevelSensorViewModel>();

        private Tank _tank;
        public double TankCapacity
        {
            get; set;
        } = 100;

        public double TankInitialAmount { get; set; } = 0;

        public double StepSeconds
        {
            get => _simulationService.StepSeconds;
            set { _simulationService.StepSeconds = value; OnPropertyChanged(); }
        }

        private double _currentAmount;
        public double CurrentAmount
        {
            get => _currentAmount;
            private set => SetProperty(ref _currentAmount, value);
        }

        public ICommand AddInletValveCommand { get; }
        public ICommand AddOutletValveCommand { get; }
        public ICommand AddSensorCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel() : this(new SimulationService())
        {
        }

        public MainViewModel(ISimulationService simulationService)
        {
            _simulationService = simulationService;

            AddInletValveCommand = new RelayCommand(_ => AddValve(ValveType.Inlet));
            AddOutletValveCommand = new RelayCommand(_ => AddValve(ValveType.Outlet));
            AddSensorCommand = new RelayCommand(_ => AddSensor());
            StartCommand = new RelayCommand(_ => Start());
            StopCommand = new RelayCommand(_ => Stop());

            // defaults
            AddValve(ValveType.Inlet);
            AddValve(ValveType.Outlet);
            AddSensor();
        }

        private void AddValve(ValveType type)
        {
            var model = new Valve($"V_{type}_{Valves.Count(v => v.Direction == type)}", type, type == ValveType.Inlet ? 1.0 : 0.5)
            {
                CommandDo = new DigitalOutput($"DO_{type}_{Valves.Count}")
            };
            var vm = new ValveViewModel(model);
            Valves.Add(vm);
        }

        private void AddSensor()
        {
            var model = new LevelSensor($"LS_{Sensors.Count}", 80)
            {
                StatusDi = new DigitalInput($"DI_LS_{Sensors.Count}")
            };
            var vm = new LevelSensorViewModel(model);
            Sensors.Add(vm);
        }

        private void Start()
        {
            _tank = new Tank("T1", TankCapacity, TankInitialAmount);
            foreach (var v in Valves) _tank.AttachValve(v.Model);
            foreach (var s in Sensors) _tank.AttachSensor(s.Model);
            _simulationService.Configure(_tank, Valves.Select(v => v.Model).ToList(), Sensors.Select(s => s.Model).ToList());
            _simulationService.Start();

            // UI polling timer for CurrentAmount display
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (s, e) =>
            {
                if (_tank != null) CurrentAmount = _tank.Amount;
                foreach (var v in Valves) v.NotifyState();
                foreach (var s2 in Sensors) s2.NotifyState();
            };
            timer.Start();
        }

        private void Stop()
        {
            _simulationService.Stop();
        }
    }
}


