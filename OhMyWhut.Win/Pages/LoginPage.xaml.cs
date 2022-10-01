using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;
using OhMyWhut.Win.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public (string, string) GetBoxInfo()
        {
            return (usernameBox.Text, passwordBox.Password);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Preference.UserName, App.Preference.Password) = GetBoxInfo();
            using (var scope = App.Current.Services.CreateScope())
            {
                var dataFetcher = scope.ServiceProvider.GetService<DataFetcher>();
                var ok = await dataFetcher.UpdateCoursesAsync();
                if (ok)
                {
                    App.MainWindow.Navigator.NavigateTo(typeof(HomePage));
                }
                else
                {
                    FailedTip.Visibility = Visibility.Visible;
                }
            }
        }

        public MainViewModel ViewModel { get => App.ViewModel; }

        public AppPreference AppPreference { get => App.Preference; }
    }
}
