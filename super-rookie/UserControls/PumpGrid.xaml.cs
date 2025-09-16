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
    /// PumpGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PumpGrid : UserControl
    {
        public PumpGrid()
        {
            InitializeComponent();
            this.DataContextChanged += PumpGrid_DataContextChanged;
        }

        private void PumpGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                    // SelectedModule이 PumpVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is PumpVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddPump_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 Pump 모델 생성
                var newPump = new Pump
                {
                    Name = $"Pump_{mixingUnitVM.Pumps.Count + 1}"
                };
                
                // 새 PumpVM 생성 및 추가
                var newPumpVM = new PumpVM(newPump);
                mixingUnitVM.Pumps.Add(newPumpVM);
            }
        }

        private void DeletePump_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pumpVM = button?.Tag as PumpVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (pumpVM != null && mixingUnitVM != null)
            {
                // 선택된 펌프가 삭제될 펌프와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == pumpVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 펌프 삭제
                mixingUnitVM.Pumps.Remove(pumpVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedPump = DataGrid.SelectedItem as PumpVM;
                mixingUnitVM.SelectedModule = selectedPump;
            }
        }
    }
}
