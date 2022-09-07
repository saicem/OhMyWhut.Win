using OhMyWhut.Win.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Win.Services
{
    internal class AppStatus
    {
        public AppStatus(AppDbContext db)
        {
            var type = this.GetType();
            Parallel.ForEach(db.Preferences, x =>
            {
                var property = type.GetProperty(x.Key);
                property.SetValue(this, x.Value);
            });
        }

        public bool IsLogin { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
