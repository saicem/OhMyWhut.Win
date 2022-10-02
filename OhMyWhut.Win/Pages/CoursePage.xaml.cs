using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OhMyWhut.Win.Controls;
using OhMyWhut.Win.Services;
using OhMyWhut.Win.ViewModels;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoursePage : Page, INotifyPropertyChanged
    {
        private int _selectedWeek;

        public int SelectedWeek
        {
            get => _selectedWeek;
            set
            {
                if (value == _selectedWeek)
                {
                    return;
                }
                _selectedWeek = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWeek)));
            }
        }

        private static string[] weekDic = { "周一", "周二", "周三", "周四", "周五", "周六", "周日" };

        public event PropertyChangedEventHandler PropertyChanged;

        public CoursePage()
        {
            InitializeComponent();
            Loaded += CoursePage_Loaded;
        }

        private void CoursePage_Loaded(object sender, RoutedEventArgs e)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            SelectedWeek = (today.DayNumber - App.Preference.TermStartDay.DayNumber) / 7 + 1;
            SelectedWeek = SelectedWeek < 1 ? 1 : SelectedWeek > 20 ? 20 : SelectedWeek;
            WeekSelectSlider.Value = SelectedWeek;
            UpdateCourses(SelectedWeek);
        }

        public MainViewModel ViewModel => App.ViewModel;

        public AppPreference Preference => App.Preference;

        private void UpdateCourses(int weekOrder)
        {
            CourseGrid.Children.Clear();
            var curWeekStartDate = App.Preference.TermStartDay.AddDays((SelectedWeek - 1) * 7);
            for (int i = 0; i < 7; i++)
            {
                var sp = new StackPanel();
                var dateText = new TextBlock
                {
                    Text = curWeekStartDate.AddDays(i).ToString("MM-dd"),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                var dowText = new TextBlock
                {
                    Text = weekDic[i],
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sp.Children.Add(dateText);
                sp.Children.Add(dowText);
                Grid.SetRow(sp, 0);
                Grid.SetColumn(sp, i);
                CourseGrid.Children.Add(sp);
            }
            var courses = ViewModel.CourseViewModel.CourseList
                .Where(x => x.StartWeek <= weekOrder)
                .Where(x => x.EndWeek >= weekOrder);
            foreach (var course in courses)
            {
                var courseBox = new CourseBox(course);
                Grid.SetRow(courseBox, course.BigSec);
                Grid.SetColumn(courseBox, course.DayOfWeek switch
                {
                    0 => 6,
                    _ => (int)course.DayOfWeek - 1
                });
                CourseGrid.Children.Add(courseBox);
            }
        }

        public void Slider_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            // fix 只在组件上才会触发，无组件时不会触发
            var delta = e.GetCurrentPoint((UIElement)sender).Properties.MouseWheelDelta;
            if (delta < 0 && SelectedWeek < 20)
            {
                SelectedWeek += 1;
                WeekSelectSlider.Value = SelectedWeek;
                UpdateCourses(SelectedWeek);
            }
            else if (delta > 0 && SelectedWeek > 1)
            {
                SelectedWeek -= 1;
                WeekSelectSlider.Value = SelectedWeek;
                UpdateCourses(SelectedWeek);
            }
        }
    }
}
