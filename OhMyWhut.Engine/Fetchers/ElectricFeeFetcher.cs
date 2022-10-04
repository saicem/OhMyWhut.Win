using System.Text;
using OhMyWhut.Engine.Data;
using OhMyWhut.Engine.Exceptions;

namespace OhMyWhut.Engine.Fetchers
{
    internal class ElectricFeeFetcher : IFetcher
    {
        private readonly HttpClient httpClient;
        private string meterId = null!;
        private string factoryCode = null!;

        public bool HasSetElectricMeter { get; private set; } = false;

        internal ElectricFeeFetcher(in HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<FetchResult> FetchAsync()
        {
            if (!HasSetElectricMeter)
            {
                throw new RequestFailedException("未设置电表信息");
            }
            // 登录至财务收费
            var res = await httpClient.GetAsync("http://zhlgd.whut.edu.cn/tpass/login?service=http%3A%2F%2Fcwsf.whut.edu.cn%2FcasLogin");
            if (!res?.RequestMessage?.RequestUri?.AbsoluteUri.StartsWith("http://cwsf.whut.edu.cn/showPublic") ?? true)
            {
                return new FetchResult(FetchStatus.MissLoginStatus, null);
                //throw new RequestFailedException("获取电费失败");
            }
            // 查询电表接口
            res = await httpClient.PostAsync("http://cwsf.whut.edu.cn/queryReserve", new StringContent($"meterId={meterId}&factorycode={factoryCode}", Encoding.UTF8, "application/x-www-form-urlencoded"));
            // TODO 未能成功获取数据时，fee 会被解析为一堆 null 值，这在操作时会产生隐患
            return new FetchResult(FetchStatus.Success, await res.Content.ReadAsStringAsync());
        }

        public void SetMeter(string meterId, string factoryCode)
        {
            this.meterId = meterId;
            this.factoryCode = factoryCode;
            HasSetElectricMeter = true;
        }
    }
}
