using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string username;
        private string password;
        private bool isLogin = false;

        public DataFetcher(AppDbContext db)
        {
            _db = db;
        }

        public DataFetcher UpdateUserInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
            return this;
        }

        public async Task LoginAsync()
        {
            try
            {
                await gluttony.LoginAsync(username, password);
                ToastNotificationManager.CreateToastNotifier()
                    .Show(new ToastNotification(new ToastContentBuilder().AddText("登录成功").GetToastContent().GetXml()));
                _db.AddLog("login", "success");
            }
            catch (RequestFailedException ex)
            {
                ToastNotificationManager.CreateToastNotifier()
                    .Show(new ToastNotification(new ToastContentBuilder().AddText(ex.Message).GetToastContent().GetXml()));
                _db.AddLog("login", "faild");
            }
        }

        public async Task<IEnumerable<Engine.Data.Book>> GetBooksAsync()
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            return await gluttony.GetBooksAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Engine.Data.Course>> GetCoursesAsync()
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            return await gluttony.GetMyCoursesAsync().ConfigureAwait(false);
        }

        public async Task<Engine.Data.ElectricFee> GetElectricFeeAsync(string meterId, string factoryCode)
        {
            if (!isLogin)
            {
                await LoginAsync();
            }
            return await gluttony.GetElectricFeeAsync(meterId, factoryCode).ConfigureAwait(false);
        }
    }
}
