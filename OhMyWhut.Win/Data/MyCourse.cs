using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(MyCourse))]
    public class MyCourse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Teacher { get; set; }

        public string Position { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public int StartWeek { get; set; }

        public int EndWeek { get; set; }

        public int StartSec { get; set; }

        public int EndSec { get; set; }

        [NotMapped]
        public int BigSec => StartSec switch
        {
            1 => 1,
            3 => 2,
            6 => 3,
            9 => 4,
            11 => 5,
            _ => throw new Exception("课程节数异常"),
        };

        private static int[] _startSections = { 1, 3, 6, 9, 11 };

        private static int[] _endSections = { 2, 4, 5, 7, 8, 10, 12, 13 };

        public string[] Verify()
        {
            var errors = new List<string>();
            if (StartWeek > EndWeek)
            {
                errors.Add("开始周应该小于结束周");
            }
            if (StartSec > EndSec)
            {
                errors.Add("开始节应该小于结束节");
            }
            if (StartWeek < 1 || StartWeek > 20 || EndWeek < 1 || EndWeek > 20)
            {
                errors.Add("周次应在为 1-20");
            }
            if (!_startSections.Contains(StartSec) || !_endSections.Contains(EndSec))
            {
                errors.Add("节次有误");
            }
            return errors.ToArray();
        }
    }
}
