using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using OhMyWhut.Engine;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Extentions;
using OhMyWhut.Win.Services;
using Windows.System.Power.Diagnostics;

namespace OhMyWhut.Win.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> BookList { get; }
            = new ObservableCollection<Book>();

        //private DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        public event PropertyChangedEventHandler PropertyChanged;

        public BookViewModel() => Task.Run(Initialize);

        private async void Initialize()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<Logger>();
                if (await logger.GetLatestRecordTimeSpanAsync(LogType.FetchBooks)
                >= App.Preference.QuerySpanBooks)
                {
                    await UpdateBooksAsync();
                }
                else
                {
                    LoadBooksFromDb();
                }
            }
        }

        public void LoadBooksFromDb()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                BookList.Reload(db.Books.AsNoTracking());
            }
        }

        public async Task UpdateBooksAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var gluttony = scope.ServiceProvider.GetService<Gluttony>();
                var db = scope.ServiceProvider.GetService<AppDbContext>();

                var books = await gluttony.GetBooksAsync(App.Preference.UserName, App.Preference.Password);
                var bookBag = from book in books
                              select new Book
                              {
                                  Name = book.Name,
                                  BorrowDate = book.BorrowDate,
                                  ExpireDate = book.ExpireDate,
                              };
                await db.Database.ExecuteSqlRawAsync($"DELETE FROM {nameof(Book)}");
                await db.Books.AddRangeAsync(bookBag);
                await db.Logs.AddAsync(new Log(LogType.FetchBooks, "success"));
                await db.SaveChangesAsync();
                BookList.Reload(bookBag);
            }
        }

        public async Task RefreshBooksAsync()
        {
            using (var scope = App.Current.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<AppDbContext>();
                BookList.Reload(db.Books.AsNoTracking());
            }
        }
    }
}
