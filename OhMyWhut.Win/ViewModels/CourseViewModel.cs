using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

namespace OhMyWhut.Win.ViewModels
{
    public class CourseViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MyCourse> CourseList { get; }
            = new ObservableCollection<MyCourse>();

        public event PropertyChangedEventHandler PropertyChanged;

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
