using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        private readonly FrameNavigationOptions navOptions;
        private readonly AppStatus _appStatus;
        private readonly DataFetcher _dataFetcher;

        public MainWindow()
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            _appStatus = App.Current.Services.GetService<AppStatus>();
            _dataFetcher = App.Current.Services.GetService<DataFetcher>();
            InitializeComponent();

            Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread().TryEnqueue(
            Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
            new Microsoft.UI.Dispatching.DispatcherQueueHandler(() =>
            {
                if (!_appStatus.IsLogin)
                {
                    ShowLoginDialog();
                }
                else
                {
                    _ = _dataFetcher.UpdateUserInfo(_appStatus.UserName, _appStatus.Password).LoginAsync();
                }
            }));

            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
            NavView.SelectedItem = HomeNavItem;
        }

        private void ShowLoginDialog()
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
                _ = _dataFetcher.UpdateUserInfo(_appStatus.UserName, _appStatus.Password).LoginAsync();
            };
            _ = dialog.ShowAsync();
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

        private void UserView_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginDialog();
        }
    }
}
