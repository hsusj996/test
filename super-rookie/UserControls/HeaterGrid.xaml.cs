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
    /// HeaterGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HeaterGrid : UserControl
    {
        public HeaterGrid()
        {
            InitializeComponent();
            this.DataContextChanged += HeaterGrid_DataContextChanged;
        }

        private void HeaterGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                    // SelectedModule이 HeaterVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is HeaterVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddHeater_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 Heater 모델 생성
                var newHeater = new Heater
                {
                    Name = $"Heater_{mixingUnitVM.Heaters.Count + 1}"
                };
                
                // 새 HeaterVM 생성 및 추가
                var newHeaterVM = new HeaterVM(newHeater);
                mixingUnitVM.Heaters.Add(newHeaterVM);
            }
        }

        private void DeleteHeater_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var heaterVM = button?.Tag as HeaterVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (heaterVM != null && mixingUnitVM != null)
            {
                // 선택된 히터가 삭제될 히터와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == heaterVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 히터 삭제
                mixingUnitVM.Heaters.Remove(heaterVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedHeater = DataGrid.SelectedItem as HeaterVM;
                mixingUnitVM.SelectedModule = selectedHeater;
            }
        }
    }
}
