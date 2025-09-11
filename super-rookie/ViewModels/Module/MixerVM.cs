using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.Models.Status;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class MixerVM : ObservableObject
    {
        private readonly Mixer _model;

        public string Name => _model.Name;

        [ObservableProperty]
        private DigitalOutputVM? _controlOutput;

        [ObservableProperty]
        private DigitalInputVM? _statusInput;

        public MixerVM()
        {
            _model = new Mixer();
            _controlOutput = null;
            _statusInput = null;
        }

        public MixerVM(Mixer model)
        {
            _model = model ?? new Mixer();
            _controlOutput = _model.ControlOutput != null ? new DigitalOutputVM(_model.ControlOutput) : null;
            _statusInput = _model.StatusInput != null ? new DigitalInputVM(_model.StatusInput) : null;
        }

        public MixerVM(string name)
        {
            _model = new Mixer(name);
            _controlOutput = null;
            _statusInput = null;
        }

        public MixerVM(string name, DigitalOutput controlOutput, DigitalInput statusInput)
        {
            _model = new Mixer(name, controlOutput, statusInput);
            _controlOutput = controlOutput != null ? new DigitalOutputVM(controlOutput) : null;
            _statusInput = statusInput != null ? new DigitalInputVM(statusInput) : null;
        }

        public Mixer GetModel()
        {
            _model.ControlOutput = _controlOutput?.GetModel();
            _model.StatusInput = _statusInput?.GetModel();
            return _model;
        }
    }
}
