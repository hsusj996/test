using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;
using super_rookie.ViewModels.Module;

namespace super_rookie.Pages
{
    public partial class ConfigurationPage : UserControl
    {
        public MixingUnitVM? ViewModel => DataContext as MixingUnitVM;

        public ConfigurationPage()
        {
            InitializeComponent();
        }

        private void TankItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is TankVM tank)
            {
                ViewModel.SelectedTank = tank;
                // 다른 모듈 선택 해제
                ViewModel.SelectedValve = null;
            }
        }

        private void ValveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is ValveVM valve)
            {
                ViewModel.SelectedValve = valve;
                // 다른 모듈 선택 해제
                ViewModel.SelectedTank = null;
            }
        }
    }
}
