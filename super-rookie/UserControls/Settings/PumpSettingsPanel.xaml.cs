using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls.Settings
{
    /// <summary>
    /// PumpSettingsPanel.xaml???�???�호 ?�용 ?�리
    /// </summary>
    public partial class PumpSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(PumpSettingsPanel),
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public PumpSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += PumpSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PumpSettingsPanel panel)
            {
                panel.UpdateComboBoxes();
            }
        }

        private void PumpSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            if (MixingUnitVM != null)
            {
                this.ControlOutputComboBox.ItemsSource = MixingUnitVM.DigitalOutputs;
                this.StatusInputComboBox.ItemsSource = MixingUnitVM.DigitalInputs;
            }
        }
    }
}
