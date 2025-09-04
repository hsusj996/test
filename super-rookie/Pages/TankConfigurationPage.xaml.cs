using System.Windows.Controls;
using super_rookie.ViewModels;

namespace super_rookie.Pages
{
    /// <summary>
    /// TankConfigurationPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TankConfigurationPage : UserControl
    {
        public TankConfigurationPage()
        {
            InitializeComponent();
        }

        public TankConfigurationPage(TankViewModel tank) : this()
        {
            DataContext = tank;
        }

        public void SetTank(TankViewModel tank)
        {
            DataContext = tank;
        }
    }
}
