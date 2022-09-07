using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Win.Pages;
using System;

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

        public MainWindow()
        {
            this.InitializeComponent();
            this.navOptions = new FrameNavigationOptions();
            this.navOptions.IsNavigationStackEnabled = false;
            this.navView.SelectedItem = this.HomeNavItem;
        }

        private void CheckLoginStatus()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.root.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "登录";
            dialog.CloseButtonText = "取消";
            dialog.Content = new LoginPage();
            var result = dialog.ShowAsync();
        }

        private void OnNavViewItemSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                this.contentFrame.NavigateToType(typeof(ConfigPage), null, this.navOptions);
            }
            else
            {
                var selectedTag = (args.SelectedItem as NavigationViewItem).Tag.ToString();
                if (selectedTag == "AccountPage")
                {
                    
                }
                string pageName = "OhMyWhut.Win.Pages." + selectedTag;
                this.contentFrame.NavigateToType(Type.GetType(pageName), null, this.navOptions);
            }
        }
    }
}
