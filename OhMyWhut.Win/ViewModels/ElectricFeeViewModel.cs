using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Engine;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Extentions;
using OhMyWhut.Win.Services;
using Windows.System.Power.Diagnostics;

namespace OhMyWhut.Win.ViewModels
{
    public class ElectricFeeViewModel
    {
        public ObservableCollection<ElectricFee> ElectricFeeList { get; }
            = new ObservableCollection<ElectricFee>();

        public ElectricFeeViewModel() => Task.Run(Initialize);

        private async void Initialize()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<Logger>();
                if (await logger.GetLatestRecordTimeSpanAsync(LogType.FetchElectricFee)
                >= App.Preference.QuerySpanElectricFee)
                {
                    await UpdateElectricFeeAsync();
                }
                else
                {
                    LoadElectricFeeFromDb();
                }
            }
        }

        public void LoadElectricFeeFromDb()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                ElectricFeeList.Reload(db.ElectricFees.AsNoTracking());
            }
        }

        public async Task RefreshElectricFeeAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                ElectricFeeList.Reload(db.ElectricFees.AsNoTracking());
            }
        }

        public async Task UpdateElectricFeeAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var gluttony = scope.ServiceProvider.GetService<Gluttony>();
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                var fee = await gluttony.GetElectricFeeAsync(App.Preference.UserName,
                                                          App.Preference.Password,
                                                          App.Preference.MeterId,
                                                          App.Preference.FactoryCode);
                await db.ElectricFees.AddAsync(new ElectricFee
                {
                    RemainName = fee.RemainName,
                    RemainPower = float.Parse(fee.RemainPower),
                    MeterOverdue = float.Parse(fee.MeterOverdue),
                    TotalValue = float.Parse(fee.TotalValue),
                    Unit = fee.Unit
                });
                await db.Logs.AddAsync(new Log(LogType.FetchElectricFee, "success"));
                await db.SaveChangesAsync();
            }
            LoadElectricFeeFromDb();
        }
    }
}
