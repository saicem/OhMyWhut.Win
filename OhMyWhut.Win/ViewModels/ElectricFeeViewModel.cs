using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;
using Windows.System.Power.Diagnostics;

namespace OhMyWhut.Win.ViewModels
{
    public class ElectricFeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ElectricFee> ElectricFeeList { get; }
            = new ObservableCollection<ElectricFee>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ElectricFeeViewModel() => Task.Run(GetElectricFeeAsync);

        private async Task GetElectricFeeAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                var fees = await fetcher.GetElectricFeeAsync();
                ElectricFeeList.Clear();
                foreach (var fee in fees)
                {
                    ElectricFeeList.Add(fee);
                }
            }
        }
    }
}
