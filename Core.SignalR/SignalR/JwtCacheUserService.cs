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
        public async void AddClient(string userId, RealOnlineClient client)
        {
            await _cache.Set(userId, client, 43200);
            await _cache.Set(client.ConnId, userId);
        }

        public RealOnlineClient isOnline(string userId)
        {
            var client = _cache.Get<RealOnlineClient>(userId);
            return client.Result;
        }

        public async void RemoveClientByConnId(string connId)
        {
            var userId = await ConnidToUserId(connId);
            RemoveClientByUserId(userId);
        }

        public async void RemoveClientByUserId(string userId)
        {
            var client = await _cache.Get<RealOnlineClient>(userId);
            if (client != null)
            {
                string connId = client.ConnId;
                await _cache.Del(userId);
                await _cache.Del(connId);
            }
            
        }
        private async Task<string> ConnidToUserId(string connId)
        {
            return await _cache.Get<string>(connId);
        }
    }
}
