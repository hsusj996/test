using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using super_rookie.Pages;
using super_rookie.ViewModels;

namespace super_rookie.Services
{
    public class NavigationService
    {
        private readonly Dictionary<string, Func<UserControl>> _pageFactories;
        private readonly Stack<string> _navigationStack;
        private UserControl _currentPage;
        private Panel _contentPanel;

        public event EventHandler<string> PageChanged;
        public event EventHandler NavigationStackChanged;

        public NavigationService(Panel contentPanel)
        {
            _pageFactories = new Dictionary<string, Func<UserControl>>();
            _navigationStack = new Stack<string>();
            _contentPanel = contentPanel;
        }

        public void RegisterPage(string pageName, Func<UserControl> pageFactory)
        {
            _pageFactories[pageName] = pageFactory;
        }

        public void NavigateTo(string pageName, object parameter = null)
        {
            if (!_pageFactories.ContainsKey(pageName))
            {
                throw new ArgumentException($"Page '{pageName}' not found");
            }

            if (_currentPage != null)
            {
                _navigationStack.Push(_currentPage.GetType().Name);
            }

            _currentPage = _pageFactories[pageName]();
            _contentPanel.Children.Clear();
            _contentPanel.Children.Add(_currentPage);

            // Set data context if parameter is provided
            if (parameter != null)
            {
                _currentPage.DataContext = parameter;
                
                // Special handling for TankConfigurationPage
                if (_currentPage is Pages.TankConfigurationPage tankPage && parameter is ViewModels.TankViewModel tank)
                {
                    tankPage.SetTank(tank);
                }
            }

            PageChanged?.Invoke(this, pageName);
            NavigationStackChanged?.Invoke(this, EventArgs.Empty);
        }

        public void NavigateBack()
        {
            if (_navigationStack.Count > 0)
            {
                var previousPageName = _navigationStack.Pop();
                NavigateTo(previousPageName);
            }
        }

        public bool CanNavigateBack => _navigationStack.Count > 0;

        public string CurrentPage => _currentPage?.GetType().Name;

        public void ClearHistory()
        {
            _navigationStack.Clear();
        }
    }
}
