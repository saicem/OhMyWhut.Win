using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OhMyWhut.Win.Data
{
    [Table(nameof(DetailCourse))]
    public class DetailCourse
    {
        public int Id { get; set; }

        public string SelectCode { get; set; }

        public string Code { get; set; }

        public string College { get; set; }

        public string Name { get; set; }

        public string Teachers { get; set; }

        public string Position { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public int StartWeek { get; set; }

        public int EndWeek { get; set; }

        public int StartSec { get; set; }

        public int EndSec { get; set; }
    }
}
