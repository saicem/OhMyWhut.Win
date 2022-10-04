using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OhMyWhut.Engine.Data;
using OhMyWhut.Engine.Exceptions;

namespace OhMyWhut.Engine.Fetchers
{
    internal class UserCourseFetcher : IFetcher
    {
        private readonly HttpClient httpClient;

        internal UserCourseFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<FetchResult> FetchAsync()
        {
            await LoginToCourseSelectSystem();
            var res = await httpClient.GetAsync("http://218.197.102.183/Course/grkbList.do");
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new FetchResult(FetchStatus.Success, content);
        }

        private async Task LoginToCourseSelectSystem()
        {
            var res = await httpClient.GetAsync("http://218.197.102.183/Course/");
            if (!res?.RequestMessage?.RequestUri?.AbsoluteUri.StartsWith("http://218.197.102.183/Course/login.do") ?? true)
            {
                throw new RequestFailedException("登录选课系统失败");
            }
        }
    }
}
