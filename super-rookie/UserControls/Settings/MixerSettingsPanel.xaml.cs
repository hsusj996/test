using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls.Settings
{
    /// <summary>
    /// MixerSettingsPanel.xaml???�???�호 ?�용 ?�리
    /// </summary>
    public partial class MixerSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(MixerSettingsPanel),
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public MixerSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += MixerSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MixerSettingsPanel panel)
            {
                panel.UpdateComboBoxes();
            }
        }

        private void MixerSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
