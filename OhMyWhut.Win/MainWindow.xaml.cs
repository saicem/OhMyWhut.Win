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
            PrepareData();
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);

            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;
        }

        private void PrepareData()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var appPreference = scope.ServiceProvider.GetService<AppPreference>();
                var gluttony = scope.ServiceProvider.GetService<Gluttony>();
                appPreference.LoadFromDatabaseAsync(scope.ServiceProvider.GetService<AppDbContext>()).Wait();
                // TODO 更优雅的判断方式
                if (appPreference.UserName == string.Empty)
                {
                    _ = Dialogs.ShowLoginDialogAsync(RootGrid.XamlRoot);
                }
                else
                {
                    gluttony.SetUserInfo(appPreference.UserName, appPreference.Password);
                    _ = Task.Run(() =>
                    {
                        using (var scope = App.Current.Services.CreateScope())
                        {
                            var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                            fetcher.UpdateCoursesAsync().Wait();
                            fetcher.UpdateBooksAsync().Wait();
                        }
                    });
                }
                if (appPreference.MeterId != string.Empty)
                {
                    _ = Task.Run(() =>
                    {
                        using (var scope = App.Current.Services.CreateScope())
                        {
                            var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                            fetcher.UpdateElectricFeeAsync().Wait();
                        }
                    });
                }
            }
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
                RootFrame.NavigateToType(typeof(ElectricPage), null, navOptions);
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