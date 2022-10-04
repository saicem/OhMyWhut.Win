using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OhMyWhut.Engine.Data;

namespace OhMyWhut.Engine.Fetchers
{
    internal class BookFetcher : IFetcher
    {
        private readonly HttpClient httpClient;

        internal BookFetcher(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<FetchResult> FetchAsync()
        {
            const string API_URL = "http://zhlgd.whut.edu.cn/tp_up/up/sysintegration/getlibraryRecordList";
            var body = new StringContent("{\"draw\":1,\"order\":[],\"pageNum\":1,\"pageSize\":100,\"start\":0,\"length\":100,\"appointTime\":\"\",\"dateSearch\":\"\",\"startDate\":\"\",\"endDate\":\"\"}", Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(API_URL, body);
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new FetchResult(FetchStatus.Success, content);
        }
    }
}
