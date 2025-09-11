using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.Models.Status;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class ValveVM : ObservableObject
    {
        private readonly Valve _model;

        public string Name => _model.Name;

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private double _flowRate;

        [ObservableProperty]
        private ValveType _direction;

        [ObservableProperty]
        private DigitalOutputVM? _commandDo;

        public ValveVM()
        {
            _model = new Valve();
            _isOpen = false;
            _flowRate = 0;
            _direction = ValveType.Inlet;
            _commandDo = null;
        }

        public ValveVM(Valve model)
        {
            _model = model ?? new Valve();
            _isOpen = _model.IsOpen;
            _flowRate = _model.FlowRate;
            _direction = _model.Direction;
            _commandDo = _model.CommandDo != null ? new DigitalOutputVM(_model.CommandDo) : null;
        }

        public ValveVM(string name, ValveType direction, double flowRate)
        {
            _model = new Valve { Name = name };
            _isOpen = false;
            _flowRate = flowRate;
            _direction = direction;
            _commandDo = null;
        }

        public Valve GetModel()
        {
            _model.IsOpen = _isOpen;
            _model.FlowRate = _flowRate;
            _model.Direction = _direction;
            _model.CommandDo = _commandDo?.GetModel();
            return _model;
        }
    }
}
