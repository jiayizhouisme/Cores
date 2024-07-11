using Core.Wechat.Models;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat
{
    public interface IWechat
    {
        public Task<WechatToken?> GetToken(string key);
        public void Add(string key, WechatApiClient wc);
        public WechatApiClient Get(string key);


        public Task<WechatToken?> RegisteToken(string key);
        public bool IsTokenExpired(WechatToken token);
        public Task<WechatUser?> GetUserOpenID(string key,string code, string grant_type = "authorization_code");
        public Task<WechatUser?> GetUserInfoByOpenID(string key, string openid, string user_access_token);
    }
}
