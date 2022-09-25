using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using OhMyWhut.Win.Pages;
using Windows.Foundation;

namespace OhMyWhut.Win.Controls
{
    internal class Dialogs
    {
        internal static void ShowLoginDialog(XamlRoot root, TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> action)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = root;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.PrimaryButtonText = "登录";
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new LoginPage();
            dialog.PrimaryButtonClick += action;
            _ = dialog.ShowAsync();
        }
    }
}
