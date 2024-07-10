using Core.Cache;
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
    public class Wechat : WechatBase, IWechat
    {
        private static readonly IDictionary<string, WechatApiClient> wechatApiClients = new Dictionary<string, WechatApiClient>();
        private readonly ICacheOperation _cache;
        public Wechat(ICacheOperation _cache)
        {
            this._cache = _cache;
        }
        public static void Add(string key, WechatApiClient wc)
        {
            wechatApiClients.Add(key, wc);
        }

        public static WechatApiClient Get(string key)
        {
            return wechatApiClients[key];
        }

        public bool IsTokenExpired(WechatToken token)
        {
            var now = DateTime.Now;
            if (now.CompareTo(token.ExpireDate) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<WechatToken?> GetToken(string key)
        {
            var response = await _cache.Get<WechatToken?>(wechatCachePrefix + key);
            return response;
        }

        public async Task<WechatUser?> GetUserOpenID(string key, string code,string grant_type = "authorization_code")
        {
            var client = Get(key);
            var response = await client.ExecuteSnsOAuth2AccessTokenAsync(new SnsOAuth2AccessTokenRequest() 
            { 
                Code = code,
                GrantType = grant_type }
            );
            if (response.IsSuccessful())
            {
                var wu = await GetUserInfoByOpenID(key,response.OpenId,response.AccessToken);
                return wu;
            }
            return null;
        }
        public async Task<WechatUser?> GetUserInfoByOpenID(string key, string openid, string user_access_token)
        {
            var client = Get(key);
            var response = await client.ExecuteCgibinUserInfoAsync(new CgibinUserInfoRequest()
            {
                AccessToken = user_access_token,
                OpenId = openid
            });
            if (response.IsSuccessful())
            {
                WechatUser wu = new WechatUser();
                wu.NickName = response.Nickname;
                wu.Sex = response.Sex;
                wu.HeadImg = response.HeadImageUrl;
                wu.OpenID = response.OpenId;
                wu.Province = response.Province;
                wu.Country = response.Country;
                wu.City = response.City;
                wu.UnionID = response.UnionId;
                return wu;
            }
            return null;
        }
        public async Task<WechatToken?> RegisteToken(string key)
        {
            var now = DateTime.Now;
            var client = Get(key);
            var response = await client.ExecuteCgibinTokenAsync(new CgibinTokenRequest { GrantType = "client_credential" });
            
            if (response.IsSuccessful())
            {
                WechatToken wt = new WechatToken();
                wt.AccessToken = response.AccessToken;
                wt.ExpireDate = now.AddSeconds(response.ExpiresIn);

                await _cache.Set(wechatCachePrefix + key,wt,7200);
                return wt;
            }
            return null;
            
        }

    }
}
