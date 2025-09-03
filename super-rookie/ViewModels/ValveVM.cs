using super_rookie.Models;
using super_rookie.ViewModels.Base;

namespace super_rookie.ViewModels
{
    public class ValveViewModel : ObservableObject
    {
        public Valve Model { get; }

        public ValveViewModel(Valve model)
        {
            Model = model;
        }

        public string Name => Model.Name;
        public ValveType Direction => Model.Direction;

        public double FlowRate
        {
            get => Model.FlowRate;
            set { Model.FlowRate = value; OnPropertyChanged(); }
        }

        public bool IsOpen => Model.IsOpen;

        public bool DoState
        {
            get => Model.CommandDo != null && Model.CommandDo.State;
            set
            {
                if (Model.CommandDo == null) Model.CommandDo = new DigitalOutput($"DO_{Name}");
                Model.CommandDo.State = value;
                OnPropertyChanged();
            }
        }

        public void NotifyState()
        {
            OnPropertyChanged(nameof(IsOpen));
            OnPropertyChanged(nameof(DoState));
        }
    }
}


