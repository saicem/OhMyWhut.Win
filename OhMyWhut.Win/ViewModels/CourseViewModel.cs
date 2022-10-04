using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Engine;
using OhMyWhut.Engine.Data;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Extentions;
using OhMyWhut.Win.Services;
using Windows.Foundation.Collections;

namespace OhMyWhut.Win.ViewModels
{
    public class CourseViewModel : BindableBase
    {
        public ObservableCollection<MyCourse> CourseList { get; }
            = new ObservableCollection<MyCourse>();

        public CourseViewModel() => Task.Run(Initialize);

        private async void Initialize()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<Logger>();
                if (await logger.GetLatestRecordTimeSpanAsync(LogType.FetchCourses)
                    >= App.Preference.QuerySpanCourses)
                {
                    await UpdateCoursesAsync();
                }
                else
                {
                    LoadCoursesFromDb();
                }
            }
        }

        public void LoadCoursesFromDb()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                CourseList.Reload(db.MyCourses.AsNoTracking());
            }
        }

        public async Task UpdateCoursesAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var gluttony = scope.ServiceProvider.GetService<Gluttony>();
                var db = scope.ServiceProvider.GetService<AppDbContext>();

                var courses = await gluttony.GetMyCoursesAsync(App.Preference.UserName, App.Preference.Password).ConfigureAwait(false);
                var myCourseBag = new ConcurrentBag<MyCourse>();
                Parallel.ForEach(courses, course =>
                {
                    myCourseBag.Add(new MyCourse
                    {
                        Name = course.Name,
                        Teacher = string.Join(' ', course.Teachers),
                        Position = course.Position,
                        DayOfWeek = course.DayOfWeek,
                        StartWeek = course.StartWeek,
                        EndWeek = course.EndWeek,
                        StartSec = course.StartSection,
                        EndSec = course.EndSection
                    });
                });
                await db.Database.ExecuteSqlRawAsync($"DELETE FROM {nameof(MyCourse)}");
                await db.MyCourses.AddRangeAsync(myCourseBag);
                await db.Logs.AddAsync(new Log(LogType.FetchCourses, "success"));
                await db.SaveChangesAsync();
                CourseList.Reload(myCourseBag);
            }
        }

        public async Task AddCourseAsync(MyCourse course)
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
            }
            CourseList.Add(course);
        }
    }
}
