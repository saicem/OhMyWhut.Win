using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Win.Pages;
using OhMyWhut.Win.Services;
using Windows.Foundation.Metadata;

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
            _appStatus = App.Current.Services.GetService<AppStatus>();
            _dataFetcher = App.Current.Services.GetService<DataFetcher>();

            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            CheckLoginStatus();

            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
            NavigationViewControl.SelectedItem = HomeNavItem;
        }

        public void CheckLoginStatus()
        {
            if (!_appStatus.IsLogin)
            {
                ShowLoginDialog();
            }
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

        public string AppTitleText
        {
            get
            {
#if !UNIVERSAL && DEBUG
                return "OhMyWhut Dev";
#elif !UNIVERSAL
                return "OhMyWhut";
#elif DEBUG
                return "OhMyWhut (UWP)";
#else
                return "OhMyWhut (UWP)";
#endif
            }
        }

        private void OnNavViewItemSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                rootFrame.NavigateToType(typeof(ConfigPage), null, navOptions);
            }
            else
            {
                var selectedTag = (args.SelectedItem as NavigationViewItem).Tag.ToString();
                string pageName = "OhMyWhut.Win.Pages." + selectedTag;
                rootFrame.NavigateToType(Type.GetType(pageName), null, navOptions);
            }
        }

        private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            Thickness currMargin = AppTitleBar.Margin;
            if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness() { Left = (sender.CompactPaneLength * 2), Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };

            }
            else
            {
                AppTitleBar.Margin = new Thickness() { Left = sender.CompactPaneLength, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
            }

            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_PaneOpening(NavigationView sender, object args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void UpdateAppTitleMargin(NavigationView sender)
        {
            const int smallLeftIndent = 4, largeLeftIndent = 24;

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                AppTitle.TranslationTransition = new Vector3Transition();

                if ((sender.DisplayMode == NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
                }
                else
                {
                    AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
                }
            }
            else
            {
                Thickness currMargin = AppTitle.Margin;

                if ((sender.DisplayMode == NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Margin = new Thickness() { Left = smallLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
                }
                else
                {
                    AppTitle.Margin = new Thickness() { Left = largeLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
                }
            }
        }
    }
}