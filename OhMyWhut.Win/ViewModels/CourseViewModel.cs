using System.Collections.ObjectModel;
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

        public CourseViewModel() => Task.Run(GetCoursesAsync);

        private async Task GetCoursesAsync()
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
    }
}
