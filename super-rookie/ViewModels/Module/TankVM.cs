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
            _name = model.Name;
            _capacity = model.Capacity;
            _amount = model.Amount;
            Valves = new ObservableCollection<ValveVM>();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    _model.Name = value;
                }
            }
        }

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

        /// <summary>
        /// 탱크 시뮬레이션 업데이트
        /// </summary>
        public void Update()
        {
            // 시뮬레이션 간격: 100ms = 0.1초
            const double simulationIntervalSeconds = 0.1;
            
            // 연결된 밸브들을 순회하면서 유량 계산
            foreach (var valve in Valves)
            {
                if (valve.IsOpen)
                {
                    // FlowRate는 초당 유량이므로 시뮬레이션 간격에 맞춰 계산
                    double flowAmount = valve.FlowRate * simulationIntervalSeconds;
                    
                    // 밸브 타입에 따라 Amount 증감
                    if (valve.Direction == Models.Module.ValveType.Inlet)
                    {
                        // Inlet 밸브: 유체 유입 (Amount 증가)
                        Amount = System.Math.Min(Amount + flowAmount, Capacity);
                    }
                    else if (valve.Direction == Models.Module.ValveType.Outlet)
                    {
                        // Outlet 밸브: 유체 유출 (Amount 감소)
                        Amount = System.Math.Max(Amount - flowAmount, 0);
                    }
                }
            }
        }
    }
}