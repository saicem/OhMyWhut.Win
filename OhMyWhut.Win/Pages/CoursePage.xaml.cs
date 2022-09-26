using System;
using System.Collections.Generic;
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
using OhMyWhut.Win.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoursePage : Page
    {
        public CoursePage()
        {
            InitializeComponent();
            Loaded += CoursePage_Loaded;
        }

        private void CoursePage_Loaded(object sender, RoutedEventArgs e)
        {
            var courses = ViewModel.CourseViewModel.CourseList;
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

        public MainViewModel ViewModel => App.ViewModel;
    }
}
