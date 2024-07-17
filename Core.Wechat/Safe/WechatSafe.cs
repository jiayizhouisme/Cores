using Core.Cache;
using Furion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Safe
{
    public class WechatSafe : IWechatSafe
    {
        private readonly ICacheOperation _cache;
        public WechatSafe(ICacheOperation _cache)
        {
            this._cache = _cache;
        }

        public async System.Threading.Tasks.Task BeginCheckAccessToken(string Appid)
        {
            await _cache.Lock("CheckAccessTokenLock:" + Appid, Appid);
        }

        public async System.Threading.Tasks.Task EndCheckAccessToken(string Appid)
        {
            await _cache.ReleaseLock("CheckAccessTokenLock:" + Appid, Appid);
        }

        public async System.Threading.Tasks.Task BeginGetWechatUser(string OpenId)
        {
            await _cache.Lock("WechatGetUserInfoLock:" + OpenId, OpenId);
        }

        public async System.Threading.Tasks.Task EndGetWechatUser(string OpenId)
        {
            await _cache.ReleaseLock("WechatGetUserInfoLock:" + OpenId, OpenId);
        }

    }
}
