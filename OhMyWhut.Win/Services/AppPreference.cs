using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    public class AppPreference
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string RealName { get; set; } = string.Empty; 

        public string MeterId { get; set; } = string.Empty;

        public string FactoryCode { get; set; } = string.Empty;

        public string Dormitory { get; set; } = string.Empty;

        public TimeSpan QuerySpanCourses { get; set; } = TimeSpan.FromDays(7);

        public TimeSpan QuerySpanElectricFee { get; set; } = TimeSpan.FromHours(6);

        public TimeSpan QuerySpanBooks { get; set; } = TimeSpan.FromDays(1);

        public async Task LoadFromDatabaseAsync(AppDbContext db)
        {
            await db.Preferences.AsNoTracking().ForEachAsync(p =>
            {
                var propertyInfo = GetType().GetProperty(p.Key);
                propertyInfo.SetValue(this, Convert.ChangeType(p.Value, propertyInfo.PropertyType));
            });
        }

        public async Task SaveAsync(AppDbContext db)
        {
            var items = GetType().GetProperties();
            var entities = new Preference[items.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Preference
                {
                    Key = items[i].Name,
                    Value = items[i].GetValue(this).ToString()
                };
            }
            await db.AddRangeAsync(entities);
            await db.SaveChangesAsync();
        }
    }
}
