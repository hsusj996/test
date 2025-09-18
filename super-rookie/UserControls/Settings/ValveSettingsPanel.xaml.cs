using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls.Settings
{
    /// <summary>
    /// ValveSettingsPanel.xaml???�???�호 ?�용 ?�리
    /// </summary>
    public partial class ValveSettingsPanel : UserControl
    {
        public static readonly DependencyProperty MixingUnitVMProperty =
            DependencyProperty.Register(nameof(MixingUnitVM), typeof(MixingUnitVM), typeof(ValveSettingsPanel),
                new PropertyMetadata(null, OnMixingUnitVMChanged));

        public MixingUnitVM MixingUnitVM
        {
            get => (MixingUnitVM)GetValue(MixingUnitVMProperty);
            set => SetValue(MixingUnitVMProperty, value);
        }

        public ValveSettingsPanel()
        {
            InitializeComponent();
            this.DataContextChanged += ValveSettingsPanel_DataContextChanged;
        }

        private static void OnMixingUnitVMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ValveSettingsPanel panel)
            {
                panel.UpdateDigitalOutputsList();
            }
        }

        private void ValveSettingsPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateDigitalOutputsList();
        }

        private void UpdateDigitalOutputsList()
        {
            // ComboBox??ItemsSource�?DigitalOutputs�??�정
            var commandDoComboBox = this.FindName("CommandDoComboBox") as ComboBox;
            if (commandDoComboBox != null && MixingUnitVM != null)
            {
                commandDoComboBox.ItemsSource = MixingUnitVM.DigitalOutputs;
            }
        }
    }
}
