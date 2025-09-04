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

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        public ValveType Direction
        {
            get => Model.Direction;
            set
            {
                Model.Direction = value;
                OnPropertyChanged();
            }
        }

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

        public DigitalOutput CommandDo
        {
            get => Model.CommandDo;
            set
            {
                if (Model.CommandDo == value) return;
                Model.CommandDo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DoState));
            }
        }

        public void NotifyState()
        {
            OnPropertyChanged(nameof(IsOpen));
            OnPropertyChanged(nameof(DoState));
        }
    }
}


