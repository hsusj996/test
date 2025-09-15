using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class ValveVM : ObservableObject
    {
        private readonly Valve _model;

        public string Name => _model.Name;

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
    }
}