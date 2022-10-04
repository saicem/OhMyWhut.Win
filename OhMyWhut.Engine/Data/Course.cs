using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWhut.Engine.Data
{
    public class Course
    {
        /// <summary>
        /// 选课课号
        /// </summary>
        public string SelectCode { get; set; } = null!;

        /// <summary>
        /// 课程代码
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// 学院
        /// </summary>
        public string College { get; set; } = null!;

        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string[] Teachers { get; set; } = null!;

        /// <summary>
        /// 周几
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// 开始周 1-20
        /// </summary>
        public int StartWeek { get; set; }

        /// <summary>
        /// 结束周 1-20
        /// </summary>
        public int EndWeek { get; set; }

        /// <summary>
        /// 开始节 1-13
        /// </summary>
        public int StartSection { get; set; }

        /// <summary>
        /// 结束节 1-13
        /// </summary>
        public int EndSection { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>
        public string Position { get; set; } = null!;
    }
}
