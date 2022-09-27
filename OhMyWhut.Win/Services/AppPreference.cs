using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    public class AppPreference
    {
        public bool IsSetUserInfo { get => UserName != string.Empty && Password != string.Empty; }

        public bool IsSetMeterInfo { get => MeterId != string.Empty && FactoryCode != string.Empty; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string RealName { get; set; } = string.Empty;

        public string MeterId { get; set; } = string.Empty;

        public string FactoryCode { get; set; } = string.Empty;

        public string Dormitory { get; set; } = string.Empty;

        public TimeSpan QuerySpanCourses { get; set; } = TimeSpan.FromDays(7);

        public TimeSpan QuerySpanElectricFee { get; set; } = TimeSpan.FromHours(6);

        public TimeSpan QuerySpanBooks { get; set; } = TimeSpan.FromDays(1);

        private static AppPreference instance;

        private static readonly object locker = new object();

        private AppPreference()
        {
        }

        public static AppPreference GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new AppPreference();
                }
            }
            return instance;
        }

        public async Task LoadFromDatabaseAsync(AppDbContext db)
        {
            await db.Preferences.AsNoTracking().ForEachAsync(p =>
            {
                var propertyInfo = GetType().GetProperty(p.Key);
                if (propertyInfo.PropertyType.Name is "TimeSpan")
                {
                    propertyInfo.SetValue(this, TimeSpan.Parse(p.Value));
                }
                else
                {
                    propertyInfo.SetValue(this, p.Value);
                }
            });
        }

        public async Task SaveAsync(AppDbContext db)
        {
            var items = GetType().GetProperties().Where(p => p.CanWrite).ToArray();
            var entities = new Preference[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                entities[i] = new Preference
                {
                    Key = items[i].Name,
                    Value = items[i].GetValue(this).ToString()
                };
            }
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM {nameof(Preference)}");
            await db.Preferences.AddRangeAsync(entities);
            await db.SaveChangesAsync();
        }
    }
}
