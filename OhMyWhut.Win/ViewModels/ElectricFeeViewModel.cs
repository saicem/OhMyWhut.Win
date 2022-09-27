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

namespace OhMyWhut.Win.ViewModels
{
    public class ElectricFeeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ElectricFee> ElectricFeeList { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ElectricFeeViewModel() => Task.Run(GetElectricFeeAsync);

        private async Task GetElectricFeeAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                var fee = await fetcher.GetElectricFeeAsync();
                ElectricFeeList = new ObservableCollection<ElectricFee>(fee);
            }
        }
    }
}
