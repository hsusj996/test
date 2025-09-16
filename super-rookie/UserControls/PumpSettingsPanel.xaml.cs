using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls
{
    /// <summary>
    /// PumpSettingsPanel.xaml에 대한 상호 작용 논리
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
                ControlOutputComboBox.ItemsSource = MixingUnitVM.DigitalOutputs;
                StatusInputComboBox.ItemsSource = MixingUnitVM.DigitalInputs;
            }
        }
    }
}
