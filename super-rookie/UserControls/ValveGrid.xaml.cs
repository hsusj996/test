using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using super_rookie.ViewModels;
using super_rookie.ViewModels.Module;
using super_rookie.Models.Module;

namespace super_rookie.UserControls
{
    /// <summary>
    /// ValveGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ValveGrid : UserControl
    {
        public ValveGrid()
        {
            InitializeComponent();
            this.DataContextChanged += ValveGrid_DataContextChanged;
        }

        private void ValveGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MixingUnitVM oldMixingUnit)
            {
                oldMixingUnit.PropertyChanged -= MixingUnitVM_PropertyChanged;
            }
            
            if (e.NewValue is MixingUnitVM newMixingUnit)
            {
                newMixingUnit.PropertyChanged += MixingUnitVM_PropertyChanged;
            }
        }

        private bool _isUpdatingSelection = false;

        private void MixingUnitVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MixingUnitVM.SelectedModule))
            {
                var mixingUnitVM = sender as MixingUnitVM;
                if (mixingUnitVM != null)
                {
                    // SelectedModule이 ValveVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is ValveVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddValve_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 Valve 모델 생성
                var newValve = new Valve
                {
                    Name = $"Valve_{mixingUnitVM.Valves.Count + 1}",
                    IsOpen = false,
                    FlowRate = 10.0,
                    Direction = ValveType.Inlet
                };
                
                // 새 ValveVM 생성 및 추가
                var newValveVM = new ValveVM(newValve);
                mixingUnitVM.Valves.Add(newValveVM);
            }
        }

        private void DeleteValve_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var valveVM = button?.Tag as ValveVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (valveVM != null && mixingUnitVM != null)
            {
                // 선택된 밸브가 삭제될 밸브와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == valveVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 밸브 삭제
                mixingUnitVM.Valves.Remove(valveVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedValve = DataGrid.SelectedItem as ValveVM;
                mixingUnitVM.SelectedModule = selectedValve;
            }
        }
    }
}
