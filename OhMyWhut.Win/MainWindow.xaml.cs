using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Engine;
using OhMyWhut.Win.Controls;
using OhMyWhut.Win.Data;
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

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);

            using (var scope = App.Current.Services.CreateScope())
            {
                var appPreference = scope.ServiceProvider.GetService<AppPreference>();
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                appPreference.LoadFromDatabaseAsync(db).Wait();
                if (!appPreference.IsSetUserInfo)
                {
                    _ = Dialogs.ShowLoginDialogAsync(RootGrid.XamlRoot);
                }
            }

            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
        }

        public string AppTitleText
        {
            get
            {
#if DEBUG
                return "OhMyWhut Dev";
#else
                return "OhMyWhut";
#endif
            }
        }

        private void NavigationTap(object sender, TappedRoutedEventArgs e)
        {
            var name = (sender as Button).Name;
            if (name == "HomeButton")
            {
                RootFrame.NavigateToType(typeof(HomePage), null, navOptions);
            }
            else if (name == "CourseButton")
            {
                RootFrame.NavigateToType(typeof(CoursePage), null, navOptions);
            }
            else if (name == "BookButton")
            {
                RootFrame.NavigateToType(typeof(BookPage), null, navOptions);
            }
            else if (name == "ElectricFeeButton")
            {
                var preference = App.Current.Services.GetService<AppPreference>();
                if (preference.IsSetMeterInfo)
                {
                    RootFrame.NavigateToType(typeof(ElectricFeePage), null, navOptions);
                }
                else
                {
                    RootFrame.NavigateToType(typeof(CwsfWebViewPage), null, navOptions);
                }
            }
            else
            {
                throw new Exception("未处理的导航选项");
            }
        }

        private void PersonProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _ = Dialogs.ShowLoginDialogAsync(RootGrid.XamlRoot);
        }
    }
}