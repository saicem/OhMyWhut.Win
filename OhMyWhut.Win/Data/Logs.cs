﻿using System;
using System.Collections.Generic;
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

    public class Log
    {
        public long Id { get; set; }

        public LogType Type { get; set; }

        public string Data { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
