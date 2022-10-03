using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OhMyWhut.Win.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OhMyWhut.Win.Controls
{
    public sealed partial class CourseBox : UserControl
    {
        public CourseBox(MyCourse course)
        {
            this.InitializeComponent();
            Course = course;
        }

        public MyCourse Course { get; }

        private async void EditMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "修改课程";
            dialog.PrimaryButtonText = "确定";
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new EditCourseDialogContent(Course);
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            var result = await dialog.ShowAsync();
        }

        private async void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var course = (sender.Content as EditCourseDialogContent).GetEditedCourse();
            var errors = course.Verify();
            if (errors.Length > 0)
            {
                // todo 通知错误
                return;
            }
            await App.ViewModel.CourseViewModel.AddCourse(course);
        }

        private async void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                db.MyCourses.Remove(Course);
                await db.SaveChangesAsync();
                await App.ViewModel.CourseViewModel.FreshCoursesAsync();
            }
        }
    }
}
