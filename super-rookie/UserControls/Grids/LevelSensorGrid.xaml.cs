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

namespace super_rookie.UserControls.Grids
{
    /// <summary>
    /// LevelSensorGrid.xaml�� ���� ��ȣ �ۿ� ����
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
                    // SelectedModule�� LevelSensorVM�� �ƴϸ� ���� ����
                    if (!(mixingUnitVM.SelectedModule is LevelSensorVM))
                    {
                        _isUpdatingSelection = true;
                        this.DataGrid.SelectedItem = null;
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
                // �� LevelSensor �� ����
                var newLevelSensor = new LevelSensor
                {
                    Name = $"LevelSensor_{mixingUnitVM.LevelSensors.Count + 1}",
                    TriggerAmount = 50.0,
                    IsTriggered = false
                };
                
                // �� LevelSensorVM ���� �� �߰�
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
                // ���õ� ���� ������ ������ ���� ������ ���ٸ� ���� ����
                if (mixingUnitVM.SelectedModule == levelSensorVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // ���� ���� ����
                mixingUnitVM.LevelSensors.Remove(levelSensorVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ���� ������Ʈ ���̸� ����
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedLevelSensor = this.DataGrid.SelectedItem as LevelSensorVM;
                mixingUnitVM.SelectedModule = selectedLevelSensor;
            }
        }
    }
}
