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

        public async Task<ICollection<MyCourse>> GetCoursesAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchCourses)
                >= App.Preference.QuerySpanCourses)
            {
                await UpdateCoursesAsync();
            }
            return await _db.MyCourses.AsNoTracking().ToArrayAsync();
        }

        public async Task<bool> UpdateCoursesAsync()
        {
            var courses = await _gluttony.GetMyCoursesAsync(App.Preference.UserName, App.Preference.Password).ConfigureAwait(false);
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
            return true;
        }

        public async Task<ICollection<Book>> GetBooksAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchBooks)
                >= App.Preference.QuerySpanBooks)
            {
                await UpdateBooksAsync();
            }

            return await _db.Books.AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateBooksAsync()
        {
            var books = await _gluttony.GetBooksAsync(App.Preference.UserName, App.Preference.Password);
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
            return true;
        }

        public async Task<List<ElectricFee>> GetElectricFeeAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchElectricFee)
                >= App.Preference.QuerySpanElectricFee)
            {
                await UpdateElectricFeeAsync();
            }
            return await _db.ElectricFees.AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateElectricFeeAsync()
        {
            var fee = await _gluttony.GetElectricFeeAsync(App.Preference.UserName,
                                                          App.Preference.Password,
                                                          App.Preference.MeterId,
                                                          App.Preference.FactoryCode);
            await _db.ElectricFees.AddAsync(new ElectricFee
            {
                RemainName = fee.RemainName,
                RemainPower = float.Parse(fee.RemainPower),
                MeterOverdue = float.Parse(fee.MeterOverdue),
                TotalValue = float.Parse(fee.TotalValue),
                Unit = fee.Unit
            });
            await _db.Logs.AddAsync(new Log(LogType.FetchElectricFee, "success"));
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
