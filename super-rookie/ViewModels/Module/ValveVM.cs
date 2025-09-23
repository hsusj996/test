using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class ValveVM : ObservableObject
    {
        private readonly Valve _model;

        public ValveVM(Valve model)
        {
            _model = model;
            _name = model.Name;
            _isOpen = model.IsOpen;
            _flowRate = model.FlowRate;
            _direction = model.Direction;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    _model.Name = value;
                }
            }
        }

        private bool _isOpen;
        private double _flowRate;
        private ValveType _direction;
        private DigitalOutputVM? _commandDo;

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (SetProperty(ref _isOpen, value))
                {
                    _model.IsOpen = value;
                }
            }
        }

        public double FlowRate
        {
            get => _flowRate;
            set
            {
                if (SetProperty(ref _flowRate, value))
                {
                    _model.FlowRate = value;
                }
            }
        }

        public ValveType Direction
        {
            get => _direction;
            set
            {
                if (SetProperty(ref _direction, value))
                {
                    _model.Direction = value;
                }
            }
        }

        public DigitalOutputVM? CommandDo
        {
            get => _commandDo;
            set
            {
                if (SetProperty(ref _commandDo, value))
                {
                    _model.CommandDo = value?.GetModel();
                }
            }
        }

        public Valve GetModel() => _model;

        /// <summary>
        /// 밸브 시뮬레이션 업데이트
        /// </summary>
        public void Update()
        {
            // TODO: 밸브 시뮬레이션 로직 구현
            // - 밸브 개폐 상태 시뮬레이션
            // - 유량 변화 시뮬레이션
            // - 디지털 출력 상태 반영
        }
    }
}