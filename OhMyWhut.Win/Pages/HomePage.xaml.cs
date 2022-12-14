using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public bool IsShowSetRoomButton { get => !Preference.IsSetMeterInfo; }

        public AppPreference Preference { get => App.Preference; }

        public MainViewModel ViewModel { get => App.ViewModel; }

        private void BindRoomButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            App.MainWindow.Navigator.NavigateTo(typeof(CwsfWebViewPage));
        }
    }
}
