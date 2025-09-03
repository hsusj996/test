using super_rookie.Models;
using super_rookie.ViewModels.Base;

namespace super_rookie.ViewModels
{
    public class LevelSensorViewModel : ObservableObject
    {
        public LevelSensor Model { get; }
        public string Name => Model.Name;

        public double TriggerAmount
        {
            get => Model.TriggerAmount;
            set { Model.TriggerAmount = value; OnPropertyChanged(); }
        }

        public bool IsTriggered => Model.IsTriggered;

        public bool DiState => Model.StatusDi != null && Model.StatusDi.State;

        public LevelSensorViewModel(LevelSensor model)
        {
            Model = model;
        }

        public void NotifyState()
        {
            OnPropertyChanged(nameof(IsTriggered));
            OnPropertyChanged(nameof(DiState));
        }

        public DigitalInput SelectedDi
        {
            get => Model.StatusDi;
            set
            {
                if (Model.StatusDi == value) return;
                Model.StatusDi = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DiState));
            }
        }
    }
}


