using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class AnalogInputVM : ObservableObject
    {
        private readonly AnalogInput _model;

        public int Id => _model.Id;
        public string Name => _model.Name;

        [ObservableProperty]
        private int _status;

        public AnalogInputVM()
        {
            _model = new AnalogInput();
            _status = 0;
        }

        public AnalogInputVM(AnalogInput model)
        {
            _model = model ?? new AnalogInput();
            _status = _model.Status;
        }

        public AnalogInputVM(int id, string name, int status = 0)
        {
            _model = new AnalogInput(id, name, status);
            _status = status;
        }

        public AnalogInput GetModel()
        {
            _model.Status = _status;
            return _model;
        }
    }
}
