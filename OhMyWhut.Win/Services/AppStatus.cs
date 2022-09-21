using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    internal class AppStatus
    {
        private readonly AppDbContext db;

        public AppStatus(AppDbContext db)
        {
            this.db = db;
        }

        public void AddOrModify(string key, string value)
        {
            var obj = db.Preferences.Find(key);
            if (obj is null)
            {
                db.Preferences.Add(new Preference
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
            get => db.Preferences.Find(nameof(IsLogin)) is not null;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => db.Preferences.Find(nameof(UserName)).Value;
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
            get => db.Preferences.Find(nameof(Password)).Value;
            set
            {
                AddOrModify(nameof(Password), value);
            }
        }

        public void Save()
        {
            _ = db.SaveChangesAsync();
        }
    }
}
