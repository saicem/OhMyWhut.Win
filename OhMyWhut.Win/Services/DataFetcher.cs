using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using OhMyWhut.Engine;
using OhMyWhut.Engine.Exceptions;
using OhMyWhut.Win.Data;
using Windows.UI.Notifications;

namespace OhMyWhut.Win.Services
{
    public class DataFetcher
    {
        private readonly Gluttony _gluttony;
        private readonly AppPreference _preference;
        private readonly AppDbContext _db;
        private readonly Logger _logger;

        public DataFetcher(Gluttony gluttony, AppPreference preference, AppDbContext db, Logger logger)
        {
            _db = db;
            _logger = logger;
            _gluttony = gluttony;
            _preference = preference;
        }

        public async Task<DataFetcher> LoginAsync()
        {
            try
            {
                await _gluttony.LoginAsync();
                ToastNotificationManager.CreateToastNotifier()
                    .Show(new ToastNotification(new ToastContentBuilder().AddText("登录成功").GetToastContent().GetXml()));
                _ = _logger.AddLogAsync(LogType.Login, "success");
            }
            catch (RequestFailedException ex)
            {
                ToastNotificationManager.CreateToastNotifier()
                    .Show(new ToastNotification(new ToastContentBuilder().AddText(ex.Message).GetToastContent().GetXml()));
                _ = _logger.AddLogAsync(LogType.Login, "fail");
            }
            return this;
        }

        public async Task UpdateDataAsync()
        {
            await UpdateCoursesAsync();
            await UpdateBooksAsync();
            await UpdateElectricFeeAsync();
        }

        public async Task<ICollection<MyCourse>> GetCoursesAsync()
        {
            await UpdateCoursesAsync();
            return await _db.MyCourses.AsNoTracking().ToArrayAsync();
        }

        private async Task UpdateCoursesAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchCourses) >= _preference.QuerySpanCourses)
            {
                return;
            }
            var courses = await _gluttony.GetMyCoursesAsync().ConfigureAwait(false);
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
            await _db.Database.ExecuteSqlRawAsync($"DELETE FROM {nameof(MyCourse)}");
            await _db.MyCourses.AddRangeAsync(myCourseBag);
            _ = _db.SaveChangesAsync();
        }

        public async Task<ICollection<Book>> GetBooksAsync()
        {
            await UpdateBooksAsync();
            return await _db.Books.AsNoTracking().ToListAsync();
        }

        private async Task UpdateBooksAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchBooks) >= _preference.QuerySpanBooks)
            {
                return;
            }
            var books = await _gluttony.GetBooksAsync();
            var bookBag = from book in books
                          select new Book
                          {
                              Name = book.Name,
                              BorrowDate = book.BorrowDate,
                              ExpireDate = book.ExpireDate,
                          };
            await _db.Database.ExecuteSqlRawAsync($"DELETE FROM {nameof(Book)}");
            await _db.Books.AddRangeAsync(bookBag);
            _ = _db.SaveChangesAsync();
        }

        public async Task<List<ElectricFee>> GetElectricFeeAsync()
        {
            await UpdateElectricFeeAsync();
            return await _db.ElectricFees.AsNoTracking().ToListAsync();
        }

        private async Task UpdateElectricFeeAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchElectricFee) >= _preference.QuerySpanElectricFee)
            {
                return;
            }
            var fee = await _gluttony.GetElectricFeeAsync();
            _db.ElectricFees.Add(new ElectricFee
            {
                RemainName = fee.RemainName,
                RemainPower = fee.RemainPower,
                MeterOverdue = fee.MeterOverdue,
                TotalValue = fee.TotalValue,
                Unit = fee.Unit
            });
            _ = _db.SaveChangesAsync();
        }

        //private async Task<string> FetchUserNameAsync()
        //{
        //    if (!isLogin)
        //    {
        //        await LoginAsync();
        //    }
        //    var res = await _gluttony.LoginToJwc();
        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.Load(res);
        //    var name = htmlDoc.DocumentNode.SelectSingleNode("html/body/div/div[1]/div[1]/div[1]/div/div[2]/div[1]/p/b").InnerText;
        //    return name;
        //}
    }
}
