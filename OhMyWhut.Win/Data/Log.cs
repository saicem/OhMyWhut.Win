using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Win.Data
{
    public enum LogType
    {
        FetchCourses,
        FetchElectricFee,
        FetchBooks,
        Login,
    }

    [Table(nameof(Log))]
    public class Log
    {
        public Log(LogType type, string data)
        {
            Type = type;
            Data = data;
        }

        public long Id { get; set; }

        public LogType Type { get; set; }

        public string Data { get; set; }

        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow; 
    }
}
