using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    internal class AppStatus
    {
        private readonly AppDbContext _db;
        private readonly Logger _logger;

        public AppStatus(AppDbContext db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void AddOrModify(string key, string value)
        {
            var obj = _db.Preferences.Find(key);
            if (obj is null)
            {
                _db.Preferences.Add(new Preference
                {
                    Key = key,
                    Value = value
                });
            }
            else
            {
                obj.Value = value;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get => _db.Preferences.Find(nameof(UserName)) is not null;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => _db.Preferences.Find(nameof(UserName)).Value;
            set
            {
                AddOrModify(nameof(UserName), value);
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _db.Preferences.Find(nameof(Password)).Value;
            set
            {
                AddOrModify(nameof(Password), value);
            }
        }

        public string RealName
        {
            get => _db.Preferences.Find(nameof(RealName))?.Value  ?? "未登录";
            set
            {
                AddOrModify(nameof(RealName), value);
            }
        }

        public void Save()
        {
            _ = _db.SaveChangesAsync();
        }
    }
}
