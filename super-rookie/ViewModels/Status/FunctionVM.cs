using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Status;

namespace super_rookie.ViewModels.Status
{
    public partial class FunctionVM : ObservableObject
    {
        private readonly Function _model;

        public int Id => _model.Id;
        public string Name => _model.Name;

        [ObservableProperty]
        private bool _status;

        public FunctionVM()
        {
            _model = new Function();
            _status = false;
        }

        public FunctionVM(Function model)
        {
            _model = model ?? new Function();
            _status = _model.Status;
        }

        public FunctionVM(int id, string name, bool status = false)
        {
            _model = new Function(id, name, status);
            _status = status;
        }

        public Function GetModel()
        {
            _model.Status = _status;
            return _model;
        }
    }
}
