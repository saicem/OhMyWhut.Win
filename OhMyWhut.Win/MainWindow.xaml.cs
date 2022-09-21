using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Engine;
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
        private readonly FrameNavigationOptions navOptions;
        private readonly AppStatus _appStatus;

        public MainWindow()
        {
            _appStatus = App.Current.Services.GetService<AppStatus>();
            InitializeComponent();

            Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread().TryEnqueue(
            Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
            new Microsoft.UI.Dispatching.DispatcherQueueHandler(() =>
            {
                if (!_appStatus.IsLogin)
                {
                    ShowLoginDialog();
                }else
                {
                    Init();
                }
            }));

            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
            navView.SelectedItem = HomeNavItem;
        }

        private async void ShowLoginDialog()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = root.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "登录";
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new LoginPage();
            dialog.PrimaryButtonClick += (s, args) =>
            {
                (_appStatus.UserName, _appStatus.Password) = (s.Content as LoginPage).GetBoxInfo();
                _appStatus.Save();
                Init();
            };
            _ = await dialog.ShowAsync();
        }

        private void Init()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var gluttony = scope.ServiceProvider.GetService<Gluttony>();
                try
                {
                    gluttony.LoginAsync(_appStatus.UserName, _appStatus.Password).ContinueWith((task) =>
                    {
                        var books = gluttony.GetBooksAsync().Result;
                        Console.WriteLine(books);
                    });
                }
                catch (Exception)
                {
                    // TODO show notification
                    throw;
                }
            };
        }

        private void OnNavViewItemSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.NavigateToType(typeof(ConfigPage), null, navOptions);
            }
            else
            {
                var selectedTag = (args.SelectedItem as NavigationViewItem).Tag.ToString();
                if (selectedTag == "AccountPage")
                {

                }
                string pageName = "OhMyWhut.Win.Pages." + selectedTag;
                contentFrame.NavigateToType(Type.GetType(pageName), null, navOptions);
            }
        }
    }
}
