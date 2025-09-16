using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models;
using super_rookie.ViewModels.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels
{
    public partial class MixingUnitVM : ObservableObject
    {
        private readonly MixingUnit _model;

        public MixingUnitVM(MixingUnit model)
        {
            _model = model;
            InitializeCollections();
        }

        public string ChemId => _model.ChemId;
        public string IpAddress => _model.IpAddress;
        public string Name => _model.Name;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        // Status ViewModels
        public ObservableCollection<FunctionVM> Functions { get; set; }
        public ObservableCollection<DigitalInputVM> DigitalInputs { get; set; }
        public ObservableCollection<DigitalOutputVM> DigitalOutputs { get; set; }
        public ObservableCollection<AnalogInputVM> AnalogInputs { get; set; }

        // Module ViewModels
        public ObservableCollection<TankVM> Tanks { get; set; }
        public ObservableCollection<ValveVM> Valves { get; set; }
        public ObservableCollection<LevelSensorVM> LevelSensors { get; set; }
        public ObservableCollection<HeaterVM> Heaters { get; set; }
        public ObservableCollection<MixerVM> Mixers { get; set; }
        public ObservableCollection<PumpVM> Pumps { get; set; }

        // Selected Modules (각 타입별로 독립적)
        private TankVM _selectedTank;
        public TankVM SelectedTank
        {
            get => _selectedTank;
            set
            {
                if (_selectedTank != value)
                {
                    _selectedTank = value;
                    OnPropertyChanged(nameof(SelectedTank));
                    UpdateSelectedModule();
                }
            }
        }

        private ValveVM _selectedValve;
        public ValveVM SelectedValve
        {
            get => _selectedValve;
            set
            {
                if (_selectedValve != value)
                {
                    _selectedValve = value;
                    OnPropertyChanged(nameof(SelectedValve));
                    UpdateSelectedModule();
                }
            }
        }

        private HeaterVM _selectedHeater;
        public HeaterVM SelectedHeater
        {
            get => _selectedHeater;
            set
            {
                if (_selectedHeater != value)
                {
                    _selectedHeater = value;
                    OnPropertyChanged(nameof(SelectedHeater));
                    UpdateSelectedModule();
                }
            }
        }

        private MixerVM _selectedMixer;
        public MixerVM SelectedMixer
        {
            get => _selectedMixer;
            set
            {
                if (_selectedMixer != value)
                {
                    _selectedMixer = value;
                    OnPropertyChanged(nameof(SelectedMixer));
                    UpdateSelectedModule();
                }
            }
        }

        private PumpVM _selectedPump;
        public PumpVM SelectedPump
        {
            get => _selectedPump;
            set
            {
                if (_selectedPump != value)
                {
                    _selectedPump = value;
                    OnPropertyChanged(nameof(SelectedPump));
                    UpdateSelectedModule();
                }
            }
        }

        private LevelSensorVM _selectedLevelSensor;
        public LevelSensorVM SelectedLevelSensor
        {
            get => _selectedLevelSensor;
            set
            {
                if (_selectedLevelSensor != value)
                {
                    _selectedLevelSensor = value;
                    OnPropertyChanged(nameof(SelectedLevelSensor));
                    UpdateSelectedModule();
                }
            }
        }

        // 통합된 SelectedModule (설정 패널용)
        private object _selectedModule;
        public object SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (_selectedModule != value)
                {
                    _selectedModule = value;
                    OnPropertyChanged(nameof(SelectedModule));
                }
            }
        }

        private void UpdateSelectedModule()
        {
            // 가장 최근에 선택된 모듈을 SelectedModule로 설정
            var newSelectedModule = _selectedTank ?? (object)_selectedValve ?? (object)_selectedHeater ?? 
                                  (object)_selectedMixer ?? (object)_selectedPump ?? (object)_selectedLevelSensor;
            
            if (SelectedModule != newSelectedModule)
            {
                SelectedModule = newSelectedModule;
            }
        }

        private void InitializeCollections()
        {
            // Status ViewModels 초기화
            Functions = new ObservableCollection<FunctionVM>();
            DigitalInputs = new ObservableCollection<DigitalInputVM>();
            DigitalOutputs = new ObservableCollection<DigitalOutputVM>();
            AnalogInputs = new ObservableCollection<AnalogInputVM>();

            // Module ViewModels 초기화 (빈 리스트)
            Tanks = new ObservableCollection<TankVM>();
            Valves = new ObservableCollection<ValveVM>();
            LevelSensors = new ObservableCollection<LevelSensorVM>();
            Heaters = new ObservableCollection<HeaterVM>();
            Mixers = new ObservableCollection<MixerVM>();
            Pumps = new ObservableCollection<PumpVM>();

            // 모델 데이터를 ViewModel로 변환
            LoadStatusData();
            LoadModuleData();
        }

        private void LoadStatusData()
        {
            foreach (var function in _model.Functions)
            {
                Functions.Add(new FunctionVM(function));
            }

            foreach (var digitalInput in _model.DigitalInputs)
            {
                DigitalInputs.Add(new DigitalInputVM(digitalInput));
            }

            foreach (var digitalOutput in _model.DigitalOutputs)
            {
                DigitalOutputs.Add(new DigitalOutputVM(digitalOutput));
            }

            foreach (var analogInput in _model.AnalogInputs)
            {
                AnalogInputs.Add(new AnalogInputVM(analogInput));
            }
        }

        private void LoadModuleData()
        {
            foreach (var tank in _model.Tanks)
            {
                Tanks.Add(new TankVM(tank));
            }

            foreach (var valve in _model.Valves)
            {
                Valves.Add(new ValveVM(valve));
            }

            foreach (var levelSensor in _model.LevelSensors)
            {
                LevelSensors.Add(new LevelSensorVM(levelSensor));
            }

            foreach (var heater in _model.Heaters)
            {
                Heaters.Add(new HeaterVM(heater));
            }

            foreach (var mixer in _model.Mixers)
            {
                Mixers.Add(new MixerVM(mixer));
            }

            foreach (var pump in _model.Pumps)
            {
                Pumps.Add(new PumpVM(pump));
            }
        }
    }
}