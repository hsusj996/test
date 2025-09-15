using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class PumpVM : ObservableObject
    {
        private readonly Pump _model;

        public string Name => _model.Name;

        private DigitalOutputVM? _controlOutput;
        private DigitalInputVM? _statusInput;

        public DigitalOutputVM? ControlOutput
        {
            get => _controlOutput;
            set
            {
                if (SetProperty(ref _controlOutput, value))
                {
                    _model.ControlOutput = value?.GetModel();
                }
            }
        }

        public DigitalInputVM? StatusInput
        {
            get => _statusInput;
            set
            {
                if (SetProperty(ref _statusInput, value))
                {
                    _model.StatusInput = value?.GetModel();
                }
            }
        }
    }
}