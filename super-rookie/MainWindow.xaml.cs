using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using super_rookie.ViewModels;
using super_rookie.Services;
using super_rookie.Pages;

namespace super_rookie
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Services.NavigationService _navigationService;
        private SimulationStateService _stateService;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize services
            _stateService = new SimulationStateService();
            _navigationService = new Services.NavigationService(ContentArea);
            
            // Initialize pages
            InitializePages();
            
            // Set data context
            var mainVM = new MainViewModel(new SimulationService(), _stateService, _navigationService);
            DataContext = mainVM;
            
            // Subscribe to tab changes
            TabControl.SelectionChanged += (s, e) => mainVM.SelectedTabIndex = TabControl.SelectedIndex;
            
            // Navigate to configuration page by default
            _navigationService.NavigateTo("ConfigurationOverview");
        }

        private void InitializePages()
        {
            // Register main pages
            _navigationService.RegisterPage("ConfigurationOverview", () => new ConfigurationOverviewPage());
            _navigationService.RegisterPage("UnitConfiguration", () => new UnitConfigurationPage());
            _navigationService.RegisterPage("TankConfiguration", () => new TankConfigurationPage());
            _navigationService.RegisterPage("Simulation", () => new SimulationPage());
            _navigationService.RegisterPage("Results", () => new ResultsPage());
        }
    }
}
