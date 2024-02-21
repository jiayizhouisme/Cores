using Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SignalR
{
    public class JwtCacheUserService : ISignalRUserService
    {
        private readonly ICacheOperation _cache;

        public JwtCacheUserService(ICacheOperation _cache)
        {
            this._cache = _cache;
        }
        public async void AddClient(string connId, RealOnlineClient client)
        {
            await _cache.Set(connId, client, 43200);
        }

        public RealOnlineClient isOnline(string userId)
        {
            var client = _cache.Get<RealOnlineClient>(userId);
            return client.Result;
        }

        public async void RemoveClient(string connId)
        {
            await _cache.Del(connId);
        }
    }
}
