using System.Collections.ObjectModel;
using System.Linq;
using super_rookie.Models;
using super_rookie.ViewModels.Base;

namespace super_rookie.ViewModels
{
    public class TankViewModel : ObservableObject
    {
        public Tank Model { get; }

        public string Name => Model.Name;

        private double _currentAmount;
        public double CurrentAmount
        {
            get => _currentAmount;
            set => SetProperty(ref _currentAmount, value);
        }

        public double Capacity
        {
            get => Model.Capacity;
        }

        public ObservableCollection<ValveViewModel> Valves { get; } = new ObservableCollection<ValveViewModel>();
        public ObservableCollection<LevelSensorViewModel> Sensors { get; } = new ObservableCollection<LevelSensorViewModel>();

        public TankViewModel(Tank model)
        {
            Model = model;
        }

        public void AttachValve(Valve valve)
        {
            Model.AttachValve(valve);
            Valves.Add(new ValveViewModel(valve));
        }

        public void AttachSensor(LevelSensor sensor)
        {
            Model.AttachSensor(sensor);
            Sensors.Add(new LevelSensorViewModel(sensor));
        }

        public void NotifyState()
        {
            CurrentAmount = Model.Amount;
            foreach (var v in Valves) v.NotifyState();
            foreach (var s in Sensors) s.NotifyState();
        }
    }
}


