using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class DigitalInputVM : ObservableObject
    {
        private readonly DigitalInput _model;

        public DigitalInputVM(DigitalInput model)
        {
            _model = model;
            _status = model.Status;
        }

        public int Id => _model.Id;
        public string Name => _model.Name;

        private bool _status;

        public bool Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
                {
                    _model.Status = value;
                }
            }
        }

        public DigitalInput GetModel() => _model;
    }
}