using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using OhMyWhut.Win.Pages;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

namespace OhMyWhut.Win.Controls
{
    internal class Dialogs
    {
        internal static void ShowLoginDialog(XamlRoot root)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = root;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "登录";
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new LoginPage();
            dialog.PrimaryButtonClick += (s, args) =>
            {
                using (var scope = App.Current.Services.CreateScope())
                {
                    var appPreference = scope.ServiceProvider.GetService<AppPreference>();
                    (appPreference.UserName, appPreference.Password) = (s.Content as LoginPage).GetBoxInfo();
                    _ = appPreference.SaveAsync(scope.ServiceProvider.GetService<AppDbContext>());
                }
            };
            _ = dialog.ShowAsync();
        }
    }
}
