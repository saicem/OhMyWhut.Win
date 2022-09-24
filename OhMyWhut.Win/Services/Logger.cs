using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OhMyWhut.Win.Data;

namespace OhMyWhut.Win.Services
{
    public class Logger
    {
        private readonly AppDbContext _db;

        public Logger(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddLogAsync(LogType type, string data)
        {
            await _db.Logs.AddAsync(new Log
            {
                Type = type,
                Data = data,
            });
            await _db.SaveChangesAsync();
        }

        public async Task<Log> GetLatestRecordAsync(LogType type)
        {
            return await (from item in _db.Logs
            where item.Type == type
            select item).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TimeSpan> GetLatestRecordTimeSpanAsync(LogType type)
        {
            return DateTime.Now - (await GetLatestRecordAsync(type)).CreatedAt;
        }
    }
}
