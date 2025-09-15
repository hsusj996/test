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
    }
}