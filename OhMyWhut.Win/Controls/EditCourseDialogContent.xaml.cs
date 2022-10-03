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
using OhMyWhut.Win.Data;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Controls
{
    public sealed partial class EditCourseDialogContent : UserControl
    {
        private int id = 0;

        public EditCourseDialogContent()
        {
            InitializeComponent();
        }

        public EditCourseDialogContent(MyCourse course)
        {
            InitializeComponent();
            id = course.Id;
            CourseNameTextBox.Text = course.Name;
            TeacherTextBox.Text = course.Teacher;
            PositionTextBox.Text = course.Position;
            StartWeekNumberBox.Value = course.StartWeek;
            EndWeekNumberBox.Value = course.EndWeek;
            StartSectionNumberBox.Value = course.StartSec;
            EndSectionNumberBox.Value = course.EndSec;
            DayOfWeekComboBox.SelectedValue = course.DayOfWeek switch
            {
                DayOfWeek.Sunday => 7,
                _ => (int)(course.DayOfWeek)
            };
        }

        public MyCourse GetEditedCourse()
        {
            return new MyCourse
            {
                Id = id,
                Name = CourseNameTextBox.Text,
                Teacher = TeacherTextBox.Text,
                Position = PositionTextBox.Text,
                StartWeek = (int)StartWeekNumberBox.Value,
                EndWeek = (int)EndWeekNumberBox.Value,
                StartSec = (int)StartSectionNumberBox.Value,
                EndSec = (int)EndSectionNumberBox.Value,
                DayOfWeek = (DayOfWeek)((int)DayOfWeekComboBox.SelectedValue % 7)
            };
        }
    }
}
