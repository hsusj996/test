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
    /// LevelSensorGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LevelSensorGrid : UserControl
    {
        public LevelSensorGrid()
        {
            InitializeComponent();
            this.DataContextChanged += LevelSensorGrid_DataContextChanged;
        }

        private void LevelSensorGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                    // SelectedModule이 LevelSensorVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is LevelSensorVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddLevelSensor_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 LevelSensor 모델 생성
                var newLevelSensor = new LevelSensor
                {
                    Name = $"LevelSensor_{mixingUnitVM.LevelSensors.Count + 1}",
                    TriggerAmount = 50.0,
                    IsTriggered = false
                };
                
                // 새 LevelSensorVM 생성 및 추가
                var newLevelSensorVM = new LevelSensorVM(newLevelSensor);
                mixingUnitVM.LevelSensors.Add(newLevelSensorVM);
            }
        }

        private void DeleteLevelSensor_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var levelSensorVM = button?.Tag as LevelSensorVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (levelSensorVM != null && mixingUnitVM != null)
            {
                // 선택된 레벨 센서가 삭제될 레벨 센서와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == levelSensorVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 레벨 센서 삭제
                mixingUnitVM.LevelSensors.Remove(levelSensorVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedLevelSensor = DataGrid.SelectedItem as LevelSensorVM;
                mixingUnitVM.SelectedModule = selectedLevelSensor;
            }
        }
    }
}
