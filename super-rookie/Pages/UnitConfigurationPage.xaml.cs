using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.Pages
{
    /// <summary>
    /// UnitConfigurationPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UnitConfigurationPage : UserControl
    {
        public UnitConfigurationPage()
        {
            InitializeComponent();
        }

        public UnitConfigurationPage(UnitViewModel unit) : this()
        {
            DataContext = unit;
        }
    }
}
