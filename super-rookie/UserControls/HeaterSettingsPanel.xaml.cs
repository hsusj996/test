using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls
{
    /// <summary>
    /// HeaterSettingsPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HeaterSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(HeaterSettingsPanel),
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public HeaterSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += HeaterSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HeaterSettingsPanel panel)
            {
                panel.UpdateComboBoxes();
            }
        }

        private void HeaterSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
