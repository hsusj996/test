using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class AnalogInputVM : ObservableObject
    {
        private readonly AnalogInput _model;

        public AnalogInputVM(AnalogInput model)
        {
            _model = model;
            _status = model.Status;
        }

        public int Id => _model.Id;
        public string Name => _model.Name;

        private int _status;

        public int Status
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
    }
}