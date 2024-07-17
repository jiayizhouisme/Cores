using Core.Cache;
using Core.Wechat.EventBus;
using Core.Wechat.Models;
using Core.Wechat.Rep;
using Core.Wechat.Safe;
using Furion.EventBus;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace Core.Wechat
{
    public class Wechat : WechatBase, IWechat
    {
        private readonly IDictionary<string, WechatApiClient> wechatApiClients;
        private readonly IWechatConfig wechatConfig;
        private readonly IWechatSafe safe;
        private readonly IEventPublisher eventPublisher;
        private readonly ICacheOperation _cache;
        public Wechat(IWechatSafe safe, IWechatConfig wechatConfig, IEventPublisher eventPublisher, ICacheOperation _cache)
        {
            this.wechatApiClients = new Dictionary<string, WechatApiClient>();
            this.safe = safe;
            this.wechatConfig = wechatConfig;
            this.eventPublisher = eventPublisher;
            this._cache = _cache;
        }

        public void Add(string key, WechatApiClient wc)
        {
            wechatApiClients.Add(key, wc);
        }

        public WechatApiClient Get(string key)
        {
            return wechatApiClients[key];
        }

        public bool IsTokenExpired(WechatToken token,int beforeTime = 0)
        {
            var now = DateTime.Now;
            if (now.AddSeconds(beforeTime).CompareTo(token.ExpireDate) >= 0)
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
                await this.safe.BeginGetWechatUser(response.OpenId);
                try
                {
                    //先从其他地方找
                    var WechatUser = await wechatConfig.GetWechatUserByOpenId(response.OpenId);
                    if (WechatUser != null)
                    {
                        var _u = WechatUser.Value;
                        _u.Key = key;
                        await eventPublisher.PublishAsync
                            (WechatUserCheckAndUpdateEvent.Event_OnGetWechatUserInfoFromCustom, _u);
                        return _u;
                    }
                    //没有找到就去微信查找
                    var wu = await GetUserInfoByOpenID(key, response.OpenId, response.AccessToken);
                    if (wu != null)
                    {
                        await eventPublisher.PublishAsync
                            (WechatUserCheckAndUpdateEvent.Event_OnGetWechatUserInfoFromWechatFirstTime, wu);
                    }
                    return wu;
                }
                catch(Exception e)
                {
                    await this.safe.EndGetWechatUser(response.OpenId);
                    throw new Exception("微信用户读取失败");
                }
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
                wu.Key = key;
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
                wt.key = key;
                await _cache.Set(wechatCachePrefix + key, wt, 7200);
                return wt;
            }
            return null;
        }
    }
}
