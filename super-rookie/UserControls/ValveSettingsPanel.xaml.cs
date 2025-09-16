using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.UserControls
{
    /// <summary>
    /// ValveSettingsPanel.xaml에 대한 상호 작용 논리
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
            // ComboBox의 ItemsSource를 DigitalOutputs로 설정
            var commandDoComboBox = this.FindName("CommandDoComboBox") as ComboBox;
            if (commandDoComboBox != null && MixingUnitVM != null)
            {
                commandDoComboBox.ItemsSource = MixingUnitVM.DigitalOutputs;
            }
        }
    }
}
