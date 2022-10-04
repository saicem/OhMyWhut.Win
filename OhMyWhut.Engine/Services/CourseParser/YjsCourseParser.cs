namespace OhMyWhut.Engine.Services.CourseParser
{
    using HtmlAgilityPack;
    using OhMyWhut.Engine.Data;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class YjsCourseParser : ICourseParser
    {
        private readonly string dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="YjsCourseParser"/> class.
        /// </summary>
        /// <param name="dataSource">解析文本</param>
        public YjsCourseParser(string dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <inheritdoc/>
        public bool IsContentValid()
        {
            return Regex.IsMatch(dataSource, "<tbody class=\"WtbodyZlistS\".+?>(.+?)</tbody>");
        }

        /// <inheritdoc/>
        public IEnumerable<Course> Parse()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(dataSource);
            var tds = htmlDoc.DocumentNode.SelectSingleNode("//*[@class='WtbodyZlistS']").SelectNodes("tr/td");
            for (int i = 0; i < tds.Count; i++)
            {
                // 周几
                var dowOrder = (i % 9) - 1;

                // 5个td标签 每个下有9个td标签 前两个标签 分别为 小节数 和时间
                if (dowOrder <= 0)
                {
                    continue;
                }

                DayOfWeek dow = (DayOfWeek)(dowOrder % 7);

                var courseText = tds[i].InnerText.Trim().Replace("\r", string.Empty);
                if (courseText.Length == 0)
                {
                    continue;
                }

                // 自然辩证法概论
                // 洪涛 自学马安机2101班
                // 节次: 1,2节
                // 周次:4 - 12
                // 地点: 西东教 - 合五
                // 开课院系: 马克思主义学院
                // 电话:87658425
                // 模拟法庭、仲裁、调解训练
                // 陈茂国 1班
                // 节次:单周9,10节 
                // 周次:7-15
                // 地点:模拟法庭
                // 开课院系:法学与人文社会学院
                // 电话:
                var multipleCourseText = courseText.Split("\n\n");
                foreach (var courseTextItem in multipleCourseText)
                {
                    var courseTextSplit = courseTextItem.Split('\n');

                    // [考试]第一外国语（英）
                    // 节次: 6,7,8节
                    // 周次:15
                    // 地点: 南新1 - 310
                    // 开课院系: 外国语学院
                    // 5行的是考试
                    if (courseTextSplit.Length == 5 && courseTextSplit[0].StartsWith("[考试]"))
                    {
                        var examName = courseTextSplit[0];
                        var examSecMatches = Regex.Match(courseTextSplit[1], @"节次:(.*?)([\d,]+?)节");
                        var examWeek = int.Parse(courseTextSplit[2].Split(':')[1]);
                        var examRoom = courseTextSplit[3];
                        GetSectionRange(examSecMatches.Groups[2].Value, out var start, out var end);
                        yield return new Course
                        {
                            Name = examName,
                            Position = examRoom,
                            StartWeek = examWeek,
                            EndWeek = examWeek,
                            StartSection = start,
                            EndSection = end,
                            DayOfWeek = dow
                        };
                        continue;
                    }

                    // 7行的是课表
                    if (courseTextSplit.Length != 7)
                    {
                        throw new Exception("不能处理的研究生课表样式");
                    }

                    var courseName = courseTextSplit[0];
                    var teacher = courseTextSplit[1].Split(' ')[0];
                    var sectionMatchs = Regex.Match(courseTextSplit[2], @"节次:(.*?)([\d,]+?)节");

                    // 已知的奇怪词汇 单周 双周 第
                    var strangeWords = sectionMatchs.Groups[1].Value;
                    GetCourseSections(sectionMatchs, strangeWords, out int sectionStart, out int sectionEnd);

                    // 需要特别处理 情况较多
                    var weekText = courseTextSplit[3];
                    var room = courseTextSplit[4][4..];

                    // 周次里有括号 1-8(某老师)
                    if (Regex.IsMatch(weekText, "\\)"))
                    {
                        var weekTextSplit = Regex.Matches(weekText, "(\\d+)-(\\d+)\\((.+?)\\)");
                        foreach (Match weekRe in weekTextSplit)
                        {
                            var weekStart = int.Parse(weekRe.Groups[1].Value);
                            var weekEnd = int.Parse(weekRe.Groups[2].Value);
                            var weekTeacher = weekRe.Groups[3].Value;
                            var courses = GetCourses(
                                dow,
                                courseName,
                                weekTeacher,
                                strangeWords,
                                sectionStart,
                                sectionEnd,
                                room,
                                weekStart,
                                weekEnd);
                            foreach (var course in courses)
                            {
                                yield return course;
                            }
                        }
                    }
                    else
                    {
                        // 周次:4 - 12 筛选周次
                        var weekList = weekText[3..].Split(",");
                        foreach (var week in weekList)
                        {
                            // 这里可能只有一个数 取决于课表条目 比如 周次:5 周次:5-8
                            var weeks = week.Split("-");
                            var weekStart = int.Parse(weeks[0]);
                            var weekEnd = int.Parse(weeks[^1]);
                            var courses = GetCourses(
                                dow,
                                courseName,
                                teacher,
                                strangeWords,
                                sectionStart,
                                sectionEnd,
                                room,
                                weekStart,
                                weekEnd);
                            foreach (var course in courses)
                            {
                                yield return course;
                            }
                        }
                    }
                }
            }
        }

        private static void GetCourseSections(in Match sectionMatchs, in string strangeWords, out int sectionStart, out int sectionEnd)
        {
            // 一节课的情况
            if (strangeWords == "第")
            {
                sectionStart = sectionEnd = int.Parse(sectionMatchs.Groups[2].Value);
            }

            // 多节课的情况
            else
            {
                GetSectionRange(sectionMatchs.Groups[2].Value, out sectionStart, out sectionEnd);
            }
        }

        private static void GetSectionRange(in string sectionText, out int sectionStart, out int sectionEnd)
        {
            var sectionTexts = sectionText.Split(',');
            sectionStart = int.Parse(sectionTexts[0]);
            sectionEnd = int.Parse(sectionTexts[^1]);
        }

        private static IEnumerable<Course> GetCourses(
            DayOfWeek dow,
            string courseName,
            string teacher,
            string strangeWords,
            int sectionStart,
            int sectionEnd,
            string room,
            int weekStart,
            int weekEnd)
        {
            if (strangeWords == string.Empty || strangeWords == "第")
            {
                yield return new Course
                {
                    Name = courseName,
                    Position = room,
                    StartWeek = weekStart,
                    EndWeek = weekEnd,
                    StartSection = sectionStart,
                    EndSection = sectionEnd,
                    DayOfWeek = dow,
                    Teachers = new string[] { teacher },
                };
            }
            else if (strangeWords == "单周")
            {
                for (int j = weekStart; j <= weekEnd; j++)
                {
                    if ((j & 1) == 1)
                    {
                        yield return new Course
                        {
                            Name = courseName,
                            Position = room,
                            StartWeek = j,
                            EndWeek = j,
                            StartSection = sectionStart,
                            EndSection = sectionEnd,
                            DayOfWeek = dow,
                            Teachers = new string[] { teacher },
                        };
                    }
                }
            }
            else if (strangeWords == "双周")
            {
                for (int j = weekStart; j <= weekEnd; j++)
                {
                    if ((j & 1) == 0)
                    {
                        yield return new Course
                        {
                            Name = courseName,
                            Position = room,
                            StartWeek = j,
                            EndWeek = j,
                            StartSection = sectionStart,
                            EndSection = sectionEnd,
                            DayOfWeek = dow,
                            Teachers = new string[] { teacher },
                        };
                    }
                }
            }
            else
            {
                throw new Exception($"strange words {strangeWords}");
            }
        }
    }
}
