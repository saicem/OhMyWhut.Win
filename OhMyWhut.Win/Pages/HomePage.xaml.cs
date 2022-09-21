using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _db;
        private readonly AppStatus _appStatus;

        public HomePage()
        {
            _logger = App.Current.Services.GetService<ILogger>();
            _db = App.Current.Services.GetService<AppDbContext>();
            _appStatus = App.Current.Services.GetService<AppStatus>();

            InitializeComponent();

            if (!_appStatus.IsLogin)
            {

            }
        }
    }
}
