using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;

namespace super_rookie.ViewModels.Module
{
    public partial class TankVM : ObservableObject
    {
        private readonly Tank _model;

        public TankVM(Tank model)
        {
            _model = model;
            _capacity = model.Capacity;
            _amount = model.Amount;
            Valves = new ObservableCollection<ValveVM>();
        }

        public string Name => _model.Name;

        private double _capacity;
        private double _amount;
        public ObservableCollection<ValveVM> Valves { get; set; }

        public double Capacity
        {
            get => _capacity;
            set
            {
                if (SetProperty(ref _capacity, value))
                {
                    _model.Capacity = value;
                }
            }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                if (SetProperty(ref _amount, value))
                {
                    _model.Amount = value;
                }
            }
        }

        public Tank GetModel() => _model;
    }
}