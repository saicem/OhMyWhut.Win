using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OhMyWhut.Engine.Data;
using OhMyWhut.Engine.Extentions;

namespace OhMyWhut.Engine.Fetchers
{
    internal class AllCourseFetcher : IFetcher
    {
        private readonly HttpClient httpClient;

        public AllCourseFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// 非常慢
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            await httpClient.GetAsync("http://202.114.50.130/DailyMgt");
            var res = await httpClient.PostAsync("http://202.114.50.130/DailyMgt/jsList.do",
                new StringContent("xnxq=2022-2023-1&xydm=&xm=%25&kcmc=", Encoding.UTF8, "application/x-www-form-urlencoded"));

            var stream = await res.Content.ReadAsStreamAsync();
            ConcurrentBag<Course> bag = ParseCourses(stream);
            stream.Close();
            stream.Dispose();
            return bag.AsEnumerable();
        }

        private static ConcurrentBag<Course> ParseCourses(Stream stream)
        {
            var bag = new ConcurrentBag<Course>();
            var doc = new HtmlDocument();
            doc.Load(stream);
            var trs = doc.DocumentNode.SelectNodes("//tbody/tr");
            Parallel.ForEach(trs, tr =>
            {
                var tds = tr.ChildNodes.Where(x => x.Name == "td").ToArray();
                var courseSelectCode = tds[0].InnerText;
                var college = tds[1].InnerText;
                var courseName = tds[2].InnerText;
                var courseCode = tds[3].InnerText;
                var courseTeachers = tds[4].ChildNodes.Select(a => a.InnerText);
                var courseWeekSpan = tds[5].InnerText;
                var courseSpans = tds[6].InnerHtml.Split("<br>", StringSplitOptions.RemoveEmptyEntries);
                var rooms = tds[7].InnerHtml.Split("<br>", StringSplitOptions.RemoveEmptyEntries);
                var courseClass = tds[8].InnerText;
                //var presetCapacity = tds[9].InnerText;
                //var actualCapacity = tds[10].InnerText;
                //var qqGroup = tds[14].InnerText;
                for (int i = 0; i < courseSpans.Length; i++)
                {
                    // TODO 为了防止匹配失败 追加 log
                    var match = Regex.Match(courseSpans[i],
                        "^周(?<dow>.)第(?<startSec>\\d+)-(?<endSec>\\d+)节\\{第(?<startWeek>\\d+)-(?<endWeek>\\d+)周\\}$");
                    if (!match.Success)
                    {
                        // TODO 加到为解析 log 里
                        continue;
                    }
                    bag.Add(new Course
                    {
                        SelectCode = courseSelectCode,
                        Code = courseCode,
                        Name = courseName,
                        College = college,
                        DayOfWeek = match.Groups["dow"].Value[0].ToDayOfWeek(),
                        StartWeek = Convert.ToInt32(match.Groups["startWeek"].Value),
                        EndWeek = Convert.ToInt32(match.Groups["endWeek"].Value),
                        StartSection = Convert.ToInt32(match.Groups["startSec"].Value),
                        EndSection = Convert.ToInt32(match.Groups["endSec"].Value),
                        Teachers = courseTeachers.ToArray(),
                        Position = rooms[i]
                    });
                }
            });
            return bag;
        }

        public Task<FetchResult> FetchAsync()
        {
            throw new NotImplementedException();
        }
    }
}
