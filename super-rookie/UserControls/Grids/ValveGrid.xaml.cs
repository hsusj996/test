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
    /// ValveGrid.xaml�� ���� ��ȣ �ۿ� ����
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
                    // SelectedModule�� ValveVM�� �ƴϸ� ���� ����
                    if (!(mixingUnitVM.SelectedModule is ValveVM))
                    {
                        _isUpdatingSelection = true;
                        this.DataGrid.SelectedItem = null;
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
                // �� Valve �� ����
                var newValve = new Valve
                {
                    Name = $"Valve_{mixingUnitVM.Valves.Count + 1}",
                    IsOpen = false,
                    FlowRate = 10.0,
                    Direction = ValveType.Inlet
                };
                
                // �� ValveVM ���� �� �߰�
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
                // ���õ� ��갡 ������ ���� ���ٸ� ���� ����
                if (mixingUnitVM.SelectedModule == valveVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // ��� ����
                mixingUnitVM.Valves.Remove(valveVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ���� ������Ʈ ���̸� ����
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedValve = this.DataGrid.SelectedItem as ValveVM;
                mixingUnitVM.SelectedModule = selectedValve;
            }
        }
    }
}
