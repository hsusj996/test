using System.Collections.ObjectModel;
using super_rookie.Models;
using super_rookie.ViewModels.Base;

namespace super_rookie.ViewModels
{
    public class UnitViewModel : ObservableObject
    {
        public Unit Model { get; }
        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TankViewModel> Tanks { get; } = new ObservableCollection<TankViewModel>();

        private TankViewModel _selectedTank;
        public TankViewModel SelectedTank
        {
            get => _selectedTank;
            set => SetProperty(ref _selectedTank, value);
        }

        public UnitViewModel(Unit model)
        {
            Model = model;
            foreach (var t in model.Tanks)
            {
                Tanks.Add(new TankViewModel(t));
            }
        }

        public TankViewModel AddTank(Tank tank)
        {
            Model.AddTank(tank);
            var tvm = new TankViewModel(tank);
            Tanks.Add(tvm);
            return tvm;
        }

        public void RemoveTank(TankViewModel tvm)
        {
            if (tvm == null) return;
            Model.RemoveTank(tvm.Model);
            Tanks.Remove(tvm);
        }
    }
}


