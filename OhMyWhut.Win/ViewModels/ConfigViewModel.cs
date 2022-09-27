using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.ViewModels
{
    public class ConfigViewModel
    {
        public ObservableCollection<Log> LogCollection { get; } 
            = new ObservableCollection<Log>();

        public ConfigViewModel() => Task.Run(Initialize);

        private void Initialize()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                foreach (var item in db.Logs)
                {
                    LogCollection.Add(item);
                }
            }
        }
    }
}
