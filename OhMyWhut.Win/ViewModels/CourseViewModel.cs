using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

namespace OhMyWhut.Win.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        public ObservableCollection<MyCourse> CourseList { get; }
            = new ObservableCollection<MyCourse>();

        public CourseViewModel() => Task.Run(FreshCoursesAsync);

        public async Task FreshCoursesAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                var courses = await fetcher.GetCoursesAsync();
                CourseList.Clear();
                foreach (var course in courses)
                {
                    CourseList.Add(course);
                }
            }
        }

        public async Task AddCourse(MyCourse course)
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                var hasCourse = db.MyCourses.Any(x => x.Id == course.Id);
                if (hasCourse)
                {
                    db.MyCourses.Update(course);
                }
                else
                {
                    db.MyCourses.Add(course);
                }
                await db.SaveChangesAsync();
                _ = App.ViewModel.CourseViewModel.FreshCoursesAsync();
            }
        }
    }
}
