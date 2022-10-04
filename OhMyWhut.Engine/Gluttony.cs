using OhMyWhut.Engine.Data;
using OhMyWhut.Engine.Exceptions;
using OhMyWhut.Engine.Fetchers;
using OhMyWhut.Engine.Responses;
using OhMyWhut.Engine.Services.CourseParser;
using System.IO;
using System.Net;
using System.Text.Json;

namespace OhMyWhut.Engine
{
    public class Gluttony
    {
        private readonly CookieContainer _cookieContainer = new CookieContainer();
        private readonly IasFetcher iasFetcher;
        private readonly BookFetcher bookFetcher;
        private readonly ElectricFeeFetcher electricFeeFetcher;
        private readonly JwcFetcher jwcFetcher;
        private readonly UserCourseFetcher userCourseFetcher;

        public Gluttony()
        {
            var httpClient = new HttpClient(new HttpClientHandler()
            {
                CookieContainer = _cookieContainer,
                UseCookies = true,
                AllowAutoRedirect = true,
            });
            AddHttpHeaders(httpClient);

            iasFetcher = new IasFetcher(httpClient);
            jwcFetcher = new JwcFetcher(httpClient);
            userCourseFetcher = new UserCourseFetcher(httpClient);
            bookFetcher = new BookFetcher(httpClient);
            electricFeeFetcher = new ElectricFeeFetcher(httpClient);
        }

        public Gluttony(List<Cookie> cookies) : this()
        {
            cookies.ForEach(cookie => _cookieContainer.Add(cookie));
        }

        public IEnumerable<Cookie> GetCookies()
        {
            return _cookieContainer.GetAllCookies().Cast<Cookie>();
        }

        public async Task<IEnumerable<Course>> GetMyCoursesAsync(string username, string password)
        {
            iasFetcher.SetUserInfo(username, password);
            await iasFetcher.FetchAsync();
            await jwcFetcher.FetchAsync();
            var result = await userCourseFetcher.FetchAsync();
            if (result.Status != FetchStatus.Success)
            {
                throw new RequestFailedException();
            }
            var courses = CourseParserFactory.Factory.Create(result.Data)?.Parse();
            if (courses is null)
            {
                throw new ParseFailedException("解析课表失败");
            }
            return courses.AsEnumerable();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(string username, string password)
        {
            iasFetcher.SetUserInfo(username, password);
            await iasFetcher.FetchAsync();
            var result = await bookFetcher.FetchAsync();
            try
            {
                var json = JsonSerializer.Deserialize<BookResponse>(result.Data);
                return json?.BookList?.Select(item => new Book(item)) ?? new List<Book>();
            }
            catch (Exception ex)
            {
                throw new Exception("解析图书失败", ex);
            }
        }

        public async Task<ElectricFee> GetElectricFeeAsync(string username, string password, string meterId, string factoryCode)
        {
            iasFetcher.SetUserInfo(username, password);
            var iasResult = await iasFetcher.FetchAsync();
            if (iasResult.Status is not FetchStatus.Success)
            {
                return null;
            }
            electricFeeFetcher.SetMeter(meterId, factoryCode);
            var result = await electricFeeFetcher.FetchAsync();
            var fee = JsonSerializer.Deserialize<ElectricFee>(result.Data);
            if (fee == null)
            {
                throw new RequestFailedException("获取电费失败");
            }
            return fee;
        }

        private void AddHttpHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Enconding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.27");
        }

        //private async Task<string> FetchUserNameAsync()
        //{
        //    if (!isLogin)
        //    {
        //        await LoginAsync();
        //    }
        //    var res = await _gluttony.LoginToJwc();
        //    var htmlDoc = new HtmlDocument();
        //    htmlDoc.Load(res);
        //    var name = htmlDoc.DocumentNode.SelectSingleNode("html/body/div/div[1]/div[1]/div[1]/div/div[2]/div[1]/p/b").InnerText;
        //    return name;
        //}
    }
}