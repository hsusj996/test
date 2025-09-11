using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class DigitalOutputVM : ObservableObject
    {
        private readonly DigitalOutput _model;

        public int Id => _model.Id;
        public string Name => _model.Name;

        [ObservableProperty]
        private bool _status;

        public DigitalOutputVM()
        {
            _model = new DigitalOutput();
            _status = false;
        }

        public DigitalOutputVM(DigitalOutput model)
        {
            _model = model ?? new DigitalOutput();
            _status = _model.Status;
        }

        public DigitalOutputVM(int id, string name, bool status = false)
        {
            _model = new DigitalOutput(id, name, status);
            _status = status;
        }

        public DigitalOutput GetModel()
        {
            _model.Status = _status;
            return _model;
        }
    }
}
