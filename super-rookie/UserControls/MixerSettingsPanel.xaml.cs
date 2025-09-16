using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls
{
    /// <summary>
    /// MixerSettingsPanel.xaml에 대한 상호 작용 논리
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
                ControlOutputComboBox.ItemsSource = MixingUnitVM.DigitalOutputs;
                StatusInputComboBox.ItemsSource = MixingUnitVM.DigitalInputs;
            }
        }
    }
}
