﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

namespace OhMyWhut.Win.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> BookList { get; private set; }

        //private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public event PropertyChangedEventHandler PropertyChanged;

        public BookViewModel() => Task.Run(GetBooksAsync);

        private async Task GetBooksAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var fetcher = scope.ServiceProvider.GetService<DataFetcher>();
                var books = await fetcher.GetBooksAsync();
                BookList = new ObservableCollection<Book>(books);
            }
        }
    }
}