using System;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
