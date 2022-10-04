using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Extentions
{
    internal static class DayOfWeekExtention
    {
        public static DayOfWeek ToDayOfWeek(this char value)
        {
            if (value == '一')
            {
                return DayOfWeek.Monday;
            }
            else if (value == '二')
            {
                return DayOfWeek.Tuesday;
            }
            else if (value == '三')
            {
                return DayOfWeek.Wednesday;
            }
            else if (value == '四')
            {
                return DayOfWeek.Thursday;
            }
            else if (value == '五')
            {
                return DayOfWeek.Friday;
            }
            else if (value == '六')
            {
                return DayOfWeek.Saturday;
            }
            else if (value == '日')
            {
                return DayOfWeek.Sunday;
            }
            throw new Exception($"Invalid char {value}");
        }
    }
}
