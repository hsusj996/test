using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.Models.Status;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class HeaterVM : ObservableObject
    {
        private readonly Heater _model;

        public string Name => _model.Name;

        [ObservableProperty]
        private DigitalOutputVM? _controlOutput;

        [ObservableProperty]
        private DigitalInputVM? _statusInput;

        public HeaterVM()
        {
            _model = new Heater();
            _controlOutput = null;
            _statusInput = null;
        }

        public HeaterVM(Heater model)
        {
            _model = model ?? new Heater();
            _controlOutput = _model.ControlOutput != null ? new DigitalOutputVM(_model.ControlOutput) : null;
            _statusInput = _model.StatusInput != null ? new DigitalInputVM(_model.StatusInput) : null;
        }

        public HeaterVM(string name)
        {
            _model = new Heater(name);
            _controlOutput = null;
            _statusInput = null;
        }

        public HeaterVM(string name, DigitalOutput controlOutput, DigitalInput statusInput)
        {
            _model = new Heater(name, controlOutput, statusInput);
            _controlOutput = controlOutput != null ? new DigitalOutputVM(controlOutput) : null;
            _statusInput = statusInput != null ? new DigitalInputVM(statusInput) : null;
        }

        public Heater GetModel()
        {
            _model.ControlOutput = _controlOutput?.GetModel();
            _model.StatusInput = _statusInput?.GetModel();
            return _model;
        }
    }
}
