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
using super_rookie.Models.Module;
using super_rookie.ViewModels.Module;

namespace super_rookie.UserControls
{
    /// <summary>
    /// TankGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TankGrid : UserControl
    {
        public TankGrid()
        {
            InitializeComponent();
            this.DataContextChanged += TankGrid_DataContextChanged;
        }

        private void TankGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                    // SelectedModule이 TankVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is TankVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddTank_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 Tank 모델 생성
                var newTank = new Tank
                {
                    Name = $"Tank_{mixingUnitVM.Tanks.Count + 1}",
                    Capacity = 100.0,
                    Amount = 0.0
                };
                
                // 새 TankVM 생성 및 추가
                var newTankVM = new TankVM(newTank);
                mixingUnitVM.Tanks.Add(newTankVM);
            }
        }

        private void DeleteTank_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tankVM = button?.Tag as TankVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (tankVM != null && mixingUnitVM != null)
            {
                // 선택된 탱크가 삭제될 탱크와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == tankVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 탱크 삭제
                mixingUnitVM.Tanks.Remove(tankVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedTank = DataGrid.SelectedItem as TankVM;
                mixingUnitVM.SelectedModule = selectedTank;
            }
        }
    }
}
