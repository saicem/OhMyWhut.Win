using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    internal class AppPreference
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public TimeSpan QuerySpanCourses { get; set; } = TimeSpan.FromDays(7);

        public TimeSpan QuerySpanElectricFee { get; set; } = TimeSpan.FromHours(6);

        public TimeSpan QuerySpanBooks { get; set; } = TimeSpan.FromDays(1);

        public async Task LoadFromDatabaseAsync(AppDbContext db)
        {
            await db.Preferences.AsNoTracking().ForEachAsync(p =>
            {
                var propertyInfo = GetType().GetProperty(p.Key);
                propertyInfo.SetValue(this, Convert.ChangeType(p.Value, propertyInfo.PropertyType.GetType()));
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
