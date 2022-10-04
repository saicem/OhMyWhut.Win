using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OhMyWhut.Engine.Data;
using OhMyWhut.Engine.Exceptions;
using OhMyWhut.Engine.Services;

namespace OhMyWhut.Engine.Fetchers
{
    internal class IasFetcher : IFetcher
    {
        private readonly HttpClient httpClient;
        private string username = null!;
        private string password = null!;
        public bool HasSetUserInfo { get; private set; } = false;

        internal IasFetcher(in HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void SetUserInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
            HasSetUserInfo = true;
        }

        public async Task<FetchResult> FetchAsync()
        {
            if (!HasSetUserInfo)
            {
                throw new RequestFailedException("未指定用户");
            }
            var (ok, lt) = await GetLtAsync();
            if (ok)
            {
                return new FetchResult(FetchStatus.Success, null);
            }
            if (lt == null)
            {
                throw new RequestFailedException("无法请求智慧理工大");
            }
            const string LOGIN_URL = "http://zhlgd.whut.edu.cn/tpass/login?service=http%3A%2F%2Fzhlgd.whut.edu.cn%2Ftp_up%2F";
            var data = new KeyValuePair<string, string>[]
            {
                new ("ul", username.Length.ToString()),
                new ("pl", password.Length.ToString()),
                new ("lt", lt),
                new ("execution", "e1s1"),
                new ("_eventId", "submit"),
                new ("rsa", Encrypt(username + password + lt))
            };
            var res = await httpClient.PostAsync(LOGIN_URL, new FormUrlEncodedContent(data));
            //if (!res?.RequestMessage?.RequestUri?.AbsoluteUri.StartsWith("http://zhlgd.whut.edu.cn/tp_up/view") ?? true)
            //{
            //    throw new RequestFailedException("登录智慧理工大失败，检查账号密码是否正确");
            //}
            return new FetchResult(FetchStatus.Success, null);
        }

        /// <summary>
        /// 获取登录需要的 lt, 失败时返回 null
        /// </summary>
        /// <returns>(ok, lt) 已登录返回 ok，未登录返回 lt，lt 为 null 则请求失败</returns>
        private async Task<(bool, string?)> GetLtAsync()
        {
            var LT_URL = "http://zhlgd.whut.edu.cn/tpass/login";
            var res = await httpClient.GetAsync(LT_URL);
            if (!res.IsSuccessStatusCode)
            {
                return (false, null);
            }
            if (res!.RequestMessage!.RequestUri!.AbsoluteUri.StartsWith("http://zhlgd.whut.edu.cn/tp_up/"))
            {
                return (true, null);
            }
            var doc = new HtmlDocument();
            doc.Load(await res.Content.ReadAsStreamAsync());
            var lt = doc.DocumentNode.SelectSingleNode(
                "/html/body/div[2]/div/div[2]/form/input[4]").Attributes["Value"].Value;
            return (false, lt);
        }

        private static string Encrypt(string data)
        {
            return Encryptor.DesEncodeString(data, new string[] { "1", "2", "3" });
        }
    }
}
