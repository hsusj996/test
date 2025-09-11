using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Messages;

namespace super_rookie.ViewModels.Module
{
    public partial class TankVM : ObservableObject
    {
        private readonly Tank _model;

        public string Name => _model.Name;

        [ObservableProperty]
        private double _capacity;

        [ObservableProperty]
        private double _amount;

        partial void OnCapacityChanged(double value)
        {
            WeakReferenceMessenger.Default.Send(new TankLevelChangedMessage(this, value, _amount));
        }

        partial void OnAmountChanged(double value)
        {
            WeakReferenceMessenger.Default.Send(new TankLevelChangedMessage(this, _capacity, value));
        }

        [ObservableProperty]
        private ObservableCollection<ValveVM> _valves;

        public TankVM()
        {
            _model = new Tank();
            _capacity = 0;
            _amount = 0;
            _valves = new ObservableCollection<ValveVM>();
        }

        public TankVM(Tank model)
        {
            _model = model ?? new Tank();
            _capacity = _model.Capacity;
            _amount = _model.Amount;
            _valves = new ObservableCollection<ValveVM>(_model.Valves.Select(v => new ValveVM(v)));
        }

        public TankVM(string name, double capacity, double amount)
        {
            _model = new Tank { Name = name };
            _capacity = capacity;
            _amount = amount;
            _valves = new ObservableCollection<ValveVM>();
        }

        public Tank GetModel()
        {
            _model.Capacity = _capacity;
            _model.Amount = _amount;
            
            _model.Valves.Clear();
            _model.Valves.AddRange(_valves.Select(v => v.GetModel()));
            
            return _model;
        }
    }
}
