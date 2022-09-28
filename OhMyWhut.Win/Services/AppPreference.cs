using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.ViewModels;

namespace OhMyWhut.Win.Services
{
    public class AppPreference : BindableBase
    {
        public bool IsSetUserInfo { get => UserName != string.Empty && Password != string.Empty; }

        public bool IsSetMeterInfo { get => MeterId != string.Empty && FactoryCode != string.Empty; }

        private string _username = string.Empty;

        public string UserName
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public string Password { get; set; } = string.Empty;

        public string RealName { get; set; } = string.Empty;

        private string _roomId = string.Empty;

        public string RoomId
        {
            get => _roomId;
            set => Set(ref _roomId, value);
        }

        private string _meterId = string.Empty;

        public string MeterId
        {
            get => _meterId;
            set => Set(ref _meterId, value);
        }

        private string _factoryCode = string.Empty;

        public string FactoryCode
        {
            get => _factoryCode;
            set => Set(ref _factoryCode, value);
        }

        private string _dormitory = string.Empty;

        public string Dormitory
        {
            get => _dormitory;
            set => Set(ref _dormitory, value);
        }

        private TimeSpan _querySpanCourses = TimeSpan.FromDays(7);

        public TimeSpan QuerySpanCourses
        {
            get => _querySpanCourses;
            set => Set(ref _querySpanCourses, value);
        }

        private TimeSpan _querySpanElectricFee = TimeSpan.FromHours(6);

        public TimeSpan QuerySpanElectricFee
        {
            get => _querySpanCourses;
            set => Set(ref _querySpanElectricFee, value);
        }

        private TimeSpan _querySpanBooks = TimeSpan.FromDays(1);

        public TimeSpan QuerySpanBooks
        {
            get => _querySpanBooks;
            set => Set(ref _querySpanElectricFee, value);
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
