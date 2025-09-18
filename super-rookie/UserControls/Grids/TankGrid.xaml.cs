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

namespace super_rookie.UserControls.Grids
{
    /// <summary>
    /// TankGrid.xaml�� ���� ��ȣ �ۿ� ����
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
                    // SelectedModule�� TankVM�� �ƴϸ� ���� ����
                    if (!(mixingUnitVM.SelectedModule is TankVM))
                    {
                        _isUpdatingSelection = true;
                        this.DataGrid.SelectedItem = null;
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
                // �� Tank �� ����
                var newTank = new Tank
                {
                    Name = $"Tank_{mixingUnitVM.Tanks.Count + 1}",
                    Capacity = 100.0,
                    Amount = 0.0
                };
                
                // �� TankVM ���� �� �߰�
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
                // ���õ� ��ũ�� ������ ��ũ�� ���ٸ� ���� ����
                if (mixingUnitVM.SelectedModule == tankVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // ��ũ ����
                mixingUnitVM.Tanks.Remove(tankVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ���� ������Ʈ ���̸� ����
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedTank = this.DataGrid.SelectedItem as TankVM;
                mixingUnitVM.SelectedModule = selectedTank;
            }
        }
    }
}
