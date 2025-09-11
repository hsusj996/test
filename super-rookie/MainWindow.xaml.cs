using System.Windows;
using super_rookie.ViewModels;

namespace super_rookie
{
    public partial class MainWindow : Window
    {
        public MainVM ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainVM();
            DataContext = ViewModel;
        }
    }
}
