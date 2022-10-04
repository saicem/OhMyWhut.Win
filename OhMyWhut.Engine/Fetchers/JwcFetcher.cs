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
    internal class JwcFetcher : IFetcher
    {
        private readonly HttpClient httpClient;

        internal JwcFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<FetchResult> FetchAsync()
        {
            var res = await httpClient.GetAsync("http://zhlgd.whut.edu.cn/tpass/login?service=http%3A%2F%2Fsso.jwc.whut.edu.cn%2FCertification%2Findex2.jsp");
            var req = new HttpRequestMessage
            {
                RequestUri = new Uri("http://sso.jwc.whut.edu.cn/Certification/casindex.do"),
                Method = HttpMethod.Get,
                Headers =
                {
                   Referrer = res.RequestMessage!.RequestUri
                }
            };
            res = await httpClient.SendAsync(req);
            var uri = res?.RequestMessage?.RequestUri?.AbsoluteUri;
            if (uri == null || !uri.StartsWith("http://sso.jwc.whut.edu.cn/Certification/toIndex.do"))
            {
                throw new RequestFailedException("登录教务处失败");
            }
            return new FetchResult(FetchStatus.Success, 
                await res!.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        //public async Task<Stream> LoginToJwc()
        //{
        //    var res = await httpClient.GetAsync("http://zhlgd.whut.edu.cn/tpass/login?service=http%3A%2F%2Fsso.jwc.whut.edu.cn%2FCertification%2Findex2.jsp");
        //    var req = new HttpRequestMessage
        //    {
        //        RequestUri = new Uri("http://sso.jwc.whut.edu.cn/Certification/casindex.do"),
        //        Method = HttpMethod.Get,
        //        Headers =
        //        {
        //           Referrer = res.RequestMessage!.RequestUri
        //        }
        //    };
        //    res = await httpClient.SendAsync(req);
        //    var uri = res?.RequestMessage?.RequestUri?.AbsoluteUri;
        //    if (uri == null || !uri.StartsWith("http://sso.jwc.whut.edu.cn/Certification/toIndex.do"))
        //    {
        //        throw new RequestFailedException("登录教务处失败");
        //    }
        //    return await res!.Content.ReadAsStreamAsync().ConfigureAwait(false);
        //}
    }
}
