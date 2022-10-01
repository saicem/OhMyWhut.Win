using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Win.Pages;

namespace OhMyWhut.Win.Services
{
    public class Navigator
    {
        private static readonly FrameNavigationOptions navOptions = new FrameNavigationOptions
        {
            IsNavigationStackEnabled = false
        };

        private Type curNavigatedType = null;

        private readonly Frame frame;

        public Navigator(Type curNavigatedType, Frame frame)
        {
            this.curNavigatedType = curNavigatedType;
            this.frame = frame;
        }

        public void NavigateTo(string name)
        {
            if (!App.Preference.IsSetUserInfo)
            {
                NavigateTo(typeof(LoginPage));
                return;
            }
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
                if (App.Preference.IsSetMeterInfo)
                {
                    NavigateTo(typeof(ElectricFeePage));
                }
                else
                {
                    NavigateTo(typeof(CwsfWebViewPage));
                }
            }
            else if (name == "ConfigButton")
            {
                NavigateTo(typeof(ConfigPage));
            }
            else
            {
                throw new Exception("未处理的导航选项");
            }
        }


        public void NavigateTo(Type type)
        {
            if (type == curNavigatedType)
            {
                return;
            }
            curNavigatedType = type;
            frame.NavigateToType(type, null, navOptions);
        }
    }
}
