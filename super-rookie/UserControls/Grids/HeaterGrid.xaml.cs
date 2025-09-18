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
    /// HeaterGrid.xaml�� ���� ��ȣ �ۿ� ����
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
                    // SelectedModule�� HeaterVM�� �ƴϸ� ���� ����
                    if (!(mixingUnitVM.SelectedModule is HeaterVM))
                    {
                        _isUpdatingSelection = true;
                        this.DataGrid.SelectedItem = null;
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
                // �� Heater �� ����
                var newHeater = new Heater
                {
                    Name = $"Heater_{mixingUnitVM.Heaters.Count + 1}"
                };
                
                // �� HeaterVM ���� �� �߰�
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
                // ���õ� ���Ͱ� ������ ���Ϳ� ���ٸ� ���� ����
                if (mixingUnitVM.SelectedModule == heaterVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // ���� ����
                mixingUnitVM.Heaters.Remove(heaterVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ���� ������Ʈ ���̸� ����
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedHeater = this.DataGrid.SelectedItem as HeaterVM;
                mixingUnitVM.SelectedModule = selectedHeater;
            }
        }
    }
}
