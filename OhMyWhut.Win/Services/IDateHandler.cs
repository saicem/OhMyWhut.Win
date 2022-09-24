using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Win.Services
{
    internal interface IDateHandler<T> where T : class
    {
        Task<List<T>> GetDataAsync();

        Task UpdateDataAsync();

        Task FetchDataAsync();
    }
}
