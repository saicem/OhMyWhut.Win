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
            navOptions = new FrameNavigationOptions();
            navOptions.IsNavigationStackEnabled = false;

            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);

            _ = InitializePreferenceAsync();
        }

        public async Task InitializePreferenceAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var appPreference = AppPreference.GetInstance();
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                await appPreference.LoadFromDatabaseAsync(db);
                if (!appPreference.IsSetUserInfo)
                {
                    RootFrame.NavigateToType(typeof(LoginPage),null,navOptions);
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
            if (!AppPreference.GetInstance().IsSetUserInfo)
            {
                NavigateTo(typeof(LoginPage));
                return;
            }
            var name = (sender as Button).Name;
            if (name == "HomeButton")
            {
                NavigateTo(typeof(HomePage));
            }
            else if (name == "CourseButton")
            {
                NavigateTo(typeof(CoursePage));
            }
            else if (name == "BookButton")
            {
                NavigateTo(typeof(BookPage));
            }
            else if (name == "ElectricFeeButton")
            {
                var preference = AppPreference.GetInstance();
                if (preference.IsSetMeterInfo)
                {
                    NavigateTo(typeof(ElectricFeePage));
                }
                else
                {
                    NavigateTo(typeof(CwsfWebViewPage));
                }
            }
            else
            {
                throw new Exception("未处理的导航选项");
            }
        }

        private Type curNavigatedType = null;

        private void NavigateTo(Type type)
        {
            if (type == curNavigatedType)
            {
                return;
            }
            curNavigatedType = type;
            RootFrame.NavigateToType(type, null, navOptions);
        }
        private void PersonProfile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RootFrame.NavigateToType(typeof(LoginPage), null, navOptions);
        }
    }
}