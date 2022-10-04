namespace OhMyWhut.Engine.Services.CourseParser
{
    using OhMyWhut.Engine.Data;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class BksCourseParser : ICourseParser
    {
        private readonly string dataSource;

        // 大节和小节的换算关系
        private static readonly Dictionary<int, int> bigToSmallSectionDic = new()
        {
            { 1, 1 },
            { 2, 3 },
            { 3, 6 },
            { 4, 9 },
            { 5, 11 }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BksCourseParser"/> class.
        /// </summary>
        /// <param name="dataSource"></param>
        public BksCourseParser(string dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <inheritdoc/>
        public bool IsContentValid()
        {
            return Regex.IsMatch(dataSource, "id=\"weekTable\"");
        }

        /// <inheritdoc/>
        public IEnumerable<Course> Parse()
        {
            var content = Regex.Replace(dataSource, @"[\r\t\n]", string.Empty);
            var divMatch = Regex.Match(content, "<td id=\"t(\\d)(\\d)\".*?>(.*?)</td>");
            while (divMatch.Success)
            {
                // 0, 2, 4, 6, 8 => 1, 2, 3, 4, 5
                var section = (int.Parse(divMatch.Groups[1].Value) >> 1) + 1;
                DayOfWeek dow = (DayOfWeek)(int.Parse(divMatch.Groups[2].Value) % 7);
                var courseMatch = Regex.Match(divMatch.Groups[3].Value, @"(.*?)\(第(\d+)-(\d+)周((\d+)-(\d+)节)?,(.*?),(.*?)\)&nbsp;&nbsp;");
                while (courseMatch.Success)
                {
                    // 没有写节次的课程默认为两节
                    var sectionStart = string.IsNullOrEmpty(courseMatch.Groups[5].Value) ? bigToSmallSectionDic[section] : int.Parse(courseMatch.Groups[5].Value);
                    var sectionEnd = string.IsNullOrEmpty(courseMatch.Groups[6].Value) ? bigToSmallSectionDic[section] + 1 : int.Parse(courseMatch.Groups[6].Value);
                    yield return new Course
                    {
                        Name = courseMatch.Groups[1].Value.Trim(),
                        StartWeek = int.Parse(courseMatch.Groups[2].Value),
                        EndWeek = int.Parse(courseMatch.Groups[3].Value),
                        Teachers = new string[] { courseMatch.Groups[7].Value.Trim() },
                        Position = courseMatch.Groups[8].Value,
                        StartSection = sectionStart,
                        EndSection = sectionEnd,
                        DayOfWeek = dow
                    };
                    courseMatch = courseMatch.NextMatch();
                }

                divMatch = divMatch.NextMatch();
            }
        }
    }
}
