using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CwsfWebViewPage : Page
    {
        public CwsfWebViewPage()
        {
            InitializeComponent();
        }

        private async Task GetElectricDataAsync()
        {
            var data = await CwsfWebView.ExecuteScriptAsync(
                """
                data = {};
                data.factoryCode = $('#factorycode').val();
                data.meterId = $('#roomid').find('option:selected').val();
                data.dormitory = $('#roomid').find('option:selected').text();
                data
                """);
            var doc = JsonDocument.Parse(data);
            using (var scope = App.Current.Services.CreateScope())
            {
                var preference = scope.ServiceProvider.GetService<AppPreference>();
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                preference.FactoryCode = doc.RootElement.GetProperty("factoryCode").GetString();
                preference.MeterId = doc.RootElement.GetProperty("meterId").GetString();
                preference.Dormitory = doc.RootElement.GetProperty("dormitory").GetString();
                // todo 显示弹窗 让用户确认 + 尝试一次请求来验证获取的信息是否正确
                await preference.SaveAsync(db);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = GetElectricDataAsync();
        }
    }
}
