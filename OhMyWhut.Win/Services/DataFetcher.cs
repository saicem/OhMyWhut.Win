using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Engine;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    public class DataFetcher
    {
        private readonly Gluttony _gluttony;
        private readonly AppDbContext _db;
        private readonly Logger _logger;

        public DataFetcher(Gluttony gluttony, AppDbContext db, Logger logger)
        {
            _db = db;
            _logger = logger;
            _gluttony = gluttony;
        }

        public void CheckUserInfo()
        {
            if (_gluttony.HasSetUserInfo)
            {
                return;
            }
            var preference = App.Preference;
            if (preference.IsSetUserInfo)
            {
                _gluttony.SetUserInfo(preference.UserName, preference.Password);
            }
            else
            {
                throw new Exception("未设置用户信息");
            }
        }

        public void CheckMeterInfo()
        {
            if (_gluttony.HasSetElectricMeter)
            {
                return;
            }
            var preference = App.Preference;
            if (preference.IsSetMeterInfo)
            {
                _gluttony.SetMeter(preference.MeterId, preference.FactoryCode);
            }
            else
            {
                throw new Exception("未设置电表信息");
            }
        }

        //public async Task<DataFetcher> LoginAsync()
        //{
        //    try
        //    {
        //        await _gluttony.LoginAsync();
        //        ToastNotificationManager.CreateToastNotifier()
        //            .Show(new ToastNotification(new ToastContentBuilder().AddText("登录成功").GetToastContent().GetXml()));
        //        _ = _logger.AddLogAsync(LogType.Login, "success");
        //    }
        //    catch (RequestFailedException ex)
        //    {
        //        ToastNotificationManager.CreateToastNotifier()
        //            .Show(new ToastNotification(new ToastContentBuilder().AddText(ex.Message).GetToastContent().GetXml()));
        //        _ = _logger.AddLogAsync(LogType.Login, "fail");
        //    }
        //    return this;
        //}

        public async Task<ICollection<MyCourse>> GetCoursesAsync()
        {
            await UpdateCoursesAsync();
            return await _db.MyCourses.AsNoTracking().ToArrayAsync();
        }

        public async Task UpdateCoursesAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchCourses)
                <= App.Preference.QuerySpanCourses)
            {
                return;
            }
            CheckUserInfo();
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
            await _db.Logs.AddAsync(new Log(LogType.FetchCourses, "success"));
            await _db.SaveChangesAsync();
        }

        public async Task<ICollection<Book>> GetBooksAsync()
        {
            await UpdateBooksAsync();
            return await _db.Books.AsNoTracking().ToListAsync();
        }

        public async Task UpdateBooksAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchBooks)
                <= App.Preference.QuerySpanBooks)
            {
                return;
            }
            CheckUserInfo();
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
            await _db.Logs.AddAsync(new Log(LogType.FetchBooks, "success"));
            await _db.SaveChangesAsync();
        }

        public async Task<List<ElectricFee>> GetElectricFeeAsync()
        {
            await UpdateElectricFeeAsync();
            return await _db.ElectricFees.AsNoTracking().ToListAsync();
        }

        public async Task UpdateElectricFeeAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchElectricFee)
                <= App.Preference.QuerySpanElectricFee)
            {
                return;
            }
            CheckUserInfo();
            CheckMeterInfo();
            var fee = await _gluttony.GetElectricFeeAsync();
            _db.ElectricFees.Add(new ElectricFee
            {
                RemainName = fee.RemainName,
                RemainPower = fee.RemainPower,
                MeterOverdue = fee.MeterOverdue,
                TotalValue = fee.TotalValue,
                Unit = fee.Unit
            });
            await _db.Logs.AddAsync(new Log(LogType.FetchCourses, "success"));
            await _db.SaveChangesAsync();
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
