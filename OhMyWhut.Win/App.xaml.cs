using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using OhMyWhut.Engine;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;
using OhMyWhut.Win.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static readonly string DataFolder = Path.Join(Directory.GetParent(Environment.ProcessPath).FullName, "OhMyWhut");

        public static MainViewModel ViewModel { get; } = new MainViewModel();

        public static AppPreference Preference { get; } = new AppPreference();

        public static MainWindow MainWindow { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
            UpdateDatabase();
            InitializeComponent();
        }

        private void UpdateDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AppDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    Console.WriteLine("尚有未迁移的数据库，已迁移");
                }
            }
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Gluttony>();
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlite());
            services.AddScoped<Logger>();
            services.AddScoped<DataFetcher>();

            return services.BuildServiceProvider();
        }
    }
}
