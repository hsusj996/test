using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class DigitalInputVM : ObservableObject
    {
        private readonly DigitalInput _model;

        public int Id => _model.Id;
        public string Name => _model.Name;

        [ObservableProperty]
        private bool _status;

        public DigitalInputVM()
        {
            _model = new DigitalInput();
            _status = false;
        }

        public DigitalInputVM(DigitalInput model)
        {
            _model = model ?? new DigitalInput();
            _status = _model.Status;
        }

        public DigitalInputVM(int id, string name, bool status = false)
        {
            _model = new DigitalInput(id, name, status);
            _status = status;
        }

        public DigitalInput GetModel()
        {
            _model.Status = _status;
            return _model;
        }
    }
}
