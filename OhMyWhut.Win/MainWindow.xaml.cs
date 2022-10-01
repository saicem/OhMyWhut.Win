using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Win.Pages;
using OhMyWhut.Win.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public Navigator Navigator { get; }

        public AppPreference Preference { get => App.Preference; }

        public MainWindow()
        {
            this.Closed += MainWindow_Closed;

            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);
            Navigator = new Navigator(null, RootFrame);
            Navigator.NavigateTo(typeof(HomePage));
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {

        }

        private void NavigationTap(object sender, TappedRoutedEventArgs e)
        {
            Navigator.NavigateTo((sender as Button).Name);
        }

        private void PersonProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Navigator.NavigateTo(typeof(LoginPage));
        }

        private void PersonProfile_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
            Navigator.NavigateTo(typeof(LoginPage));
        }
    }
}