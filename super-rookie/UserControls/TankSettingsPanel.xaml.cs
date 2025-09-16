using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;
using super_rookie.ViewModels.Module;

namespace super_rookie.UserControls
{
    /// <summary>
    /// TankSettingsPanel.xaml에 대한 상호 작용 논리
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
                // 이미 연결된 밸브들을 제외한 사용 가능한 밸브 목록
                var availableValves = MixingUnitVM.Valves.Where(v => !tankVM.Valves.Contains(v)).ToList();
                AvailableValvesListBox.ItemsSource = availableValves;
            }
        }

        private void AddValve_Click(object sender, RoutedEventArgs e)
        {
            var selectedValve = AvailableValvesListBox.SelectedItem as ValveVM;
            var tankVM = DataContext as TankVM;

            if (selectedValve != null && tankVM != null)
            {
                // 이미 연결된 밸브인지 확인
                if (!tankVM.Valves.Contains(selectedValve))
                {
                    tankVM.Valves.Add(selectedValve);
                    UpdateAvailableValves(); // 사용 가능한 밸브 목록 업데이트
                }
            }
        }

        private void RemoveValve_Click(object sender, RoutedEventArgs e)
        {
            var selectedValve = ConnectedValvesListBox.SelectedItem as ValveVM;
            var tankVM = DataContext as TankVM;

            if (selectedValve != null && tankVM != null)
            {
                tankVM.Valves.Remove(selectedValve);
                UpdateAvailableValves(); // 사용 가능한 밸브 목록 업데이트
            }
        }

    }
}
