using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls.Settings
{
    /// <summary>
    /// LevelSensorSettingsPanel.xaml???�???�호 ?�용 ?�리
    /// </summary>
    public partial class LevelSensorSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(LevelSensorSettingsPanel),
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public LevelSensorSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += LevelSensorSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LevelSensorSettingsPanel panel)
            {
                panel.UpdateComboBoxes();
            }
        }

        private void LevelSensorSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            if (MixingUnitVM != null)
            {
                this.StatusDiComboBox.ItemsSource = MixingUnitVM.DigitalInputs;
                this.TankComboBox.ItemsSource = MixingUnitVM.Tanks;
            }
        }
    }
}
