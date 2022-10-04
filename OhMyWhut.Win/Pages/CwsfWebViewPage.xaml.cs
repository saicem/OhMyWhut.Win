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
            await CwsfWebView.ExecuteScriptAsync(
                """
                data = {};
                data.factoryCode = $('#factorycode').val();
                data.roomId = $('#roomid').find('option:selected').val();
                data.dormitory = $('#roomid').find('option:selected').text();
                fetch("http://cwsf.whut.edu.cn/queryRoomElec", {
                  "headers": {
                    "accept": "application/json, text/javascript, */*; q=0.01",
                    "accept-language": "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6",
                    "content-type": "application/x-www-form-urlencoded; charset=UTF-8",
                    "proxy-connection": "keep-alive",
                    "x-requested-with": "XMLHttpRequest",
                    "Referrer-Policy": "strict-origin-when-cross-origin"
                  },
                  "body": `roomid=${data.roomId}&factorycode=${data.factoryCode}`,
                  "method": "POST"
                }).then(res => res.json()).then(json => data.meterId = json.meterId);
                """);
            // TODO 优雅的解决这个异步获取
            await Task.Delay(1000);
            var data = await CwsfWebView.ExecuteScriptAsync("data");
            var doc = JsonDocument.Parse(data);
            using (var scope = App.Current.Services.CreateScope())
            {
                var preference = App.Preference;
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                preference.FactoryCode = doc.RootElement.GetProperty("factoryCode").GetString();
                preference.RoomId = doc.RootElement.GetProperty("roomId").GetString();
                preference.Dormitory = doc.RootElement.GetProperty("dormitory").GetString();
                preference.MeterId = doc.RootElement.GetProperty("meterId").GetString();
                // todo 显示弹窗 让用户确认 + 尝试一次请求来验证获取的信息是否正确
                await App.ViewModel.ElectricFeeViewModel.UpdateElectricFeeAsync();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = GetElectricDataAsync();
        }
    }
}
