using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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
        private readonly Gluttony gluttony = new Gluttony();
        private readonly AppDbContext _db;
        private readonly Logger _logger;
        private string username;
        private string password;
        private string meterId;
        private string factoryCode;
        private bool isLogin = false;
        private bool isSetElectricMeter = false;

        public DataFetcher(AppDbContext db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void SetUserInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
            isLogin = true;
        }

        public void SetMeter(string meterId, string factoryCode)
        {
            this.meterId = meterId;
            this.factoryCode = factoryCode;
            isSetElectricMeter = true;
        }

        public async Task<DataFetcher> LoginAsync()
        {
            try
            {
                await gluttony.LoginAsync(username, password);
                ToastNotificationManager.CreateToastNotifier()
                    .Show(new ToastNotification(new ToastContentBuilder().AddText("登录成功").GetToastContent().GetXml()));
                _ = _logger.AddLogAsync(LogType.Login, "success");
                isLogin = true;
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

        public async Task<List<MyCourse>> GetCoursesAsync()
        {
            await UpdateCoursesAsync();
            return await _db.MyCourses.AsNoTracking().ToListAsync();
        }

        private async Task UpdateCoursesAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchCourses) < TimeSpan.FromDays(7))
            {
                _ = FetchCoursesAsync();
            }
        }

        private async Task<IEnumerable<Engine.Data.Course>> FetchCoursesAsync()
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            return await gluttony.GetMyCoursesAsync().ConfigureAwait(false);
        }

        public async Task<IList<Book>> GetBooksAsync()
        {
            await UpdateBooksAsync();
            return await _db.Books.AsNoTracking().ToListAsync();
        }

        private async Task UpdateBooksAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchBooks) < TimeSpan.FromDays(1))
            {
                _ = FetchBooksAsync();
            }
        }

        private async Task<IEnumerable<Engine.Data.Book>> FetchBooksAsync()
        {
            // TODO 应该增加登录失败的处理逻辑，这部分逻辑由 gluttony 解决
            if (!isLogin || !isSetElectricMeter)
            {
                throw new NotImplementedException();
            }
            return await gluttony.GetBooksAsync().ConfigureAwait(false);
        }

        public async Task<List<ElectricFee>> GetElectricFeeAsync()
        {
            await UpdateElectricFeeAsync();
            return await _db.ElectricFees.AsNoTracking().ToListAsync();
        }

        private async Task UpdateElectricFeeAsync()
        {
            if (await _logger.GetLatestRecordTimeSpanAsync(LogType.FetchElectricFee) < TimeSpan.FromHours(2))
            {
                _ = FetchElectricFeeAsync();
            }
        }
        
        private async Task<Engine.Data.ElectricFee> FetchElectricFeeAsync()
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            return await gluttony.GetElectricFeeAsync(meterId, factoryCode).ConfigureAwait(false);
        }

        private async Task<string> FetchUserNameAsync()
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            var res = await gluttony.LoginToJwc();
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(res);
            var name = htmlDoc.DocumentNode.SelectSingleNode("html/body/div/div[1]/div[1]/div[1]/div/div[2]/div[1]/p/b").InnerText;
            return name;
        }
    }
}
