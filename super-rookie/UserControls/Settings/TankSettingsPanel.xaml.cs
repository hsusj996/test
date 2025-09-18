using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;
using super_rookie.ViewModels.Module;

namespace super_rookie.UserControls.Settings
{
    /// <summary>
    /// TankSettingsPanel.xaml???�???�호 ?�용 ?�리
    /// </summary>
    public partial class TankSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(TankSettingsPanel), 
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public TankSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += TankSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TankSettingsPanel panel)
            {
                panel.UpdateAvailableValves();
            }
        }

        private void TankSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateAvailableValves();
        }

        private void UpdateAvailableValves()
        {
            if (MixingUnitVM != null && DataContext is TankVM tankVM)
            {
                // ?��? ?�결??밸브?�을 ?�외???�용 가?�한 밸브 목록
                var availableValves = MixingUnitVM.Valves.Where(v => !tankVM.Valves.Contains(v)).ToList();
                this.AvailableValvesListBox.ItemsSource = availableValves;
            }
        }

        private void AddValve_Click(object sender, RoutedEventArgs e)
        {
            var selectedValve = this.AvailableValvesListBox.SelectedItem as ValveVM;
            var tankVM = DataContext as TankVM;

            if (selectedValve != null && tankVM != null)
            {
                // ?��? ?�결??밸브?��? ?�인
                if (!tankVM.Valves.Contains(selectedValve))
                {
                    tankVM.Valves.Add(selectedValve);
                    UpdateAvailableValves(); // ?�용 가?�한 밸브 목록 ?�데?�트
                }
            }
        }

        private void RemoveValve_Click(object sender, RoutedEventArgs e)
        {
            var selectedValve = this.ConnectedValvesListBox.SelectedItem as ValveVM;
            var tankVM = DataContext as TankVM;

            if (selectedValve != null && tankVM != null)
            {
                tankVM.Valves.Remove(selectedValve);
                UpdateAvailableValves(); // ?�용 가?�한 밸브 목록 ?�데?�트
            }
        }

    }
}
