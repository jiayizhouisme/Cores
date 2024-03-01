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
        public void AddClient(string userId, RealOnlineClient client)
        {
            _cache.Set(userId, client);
            _cache.Set(client.ConnId, userId);
        }

        public RealOnlineClient isOnline(string userId)
        {
            var client = _cache.Get<RealOnlineClient>(userId);
            return client.Result;
        }

        public void RemoveClientByConnId(string connId)
        {
            var userId = ConnidToUserId(connId);
            if (userId != null)
            {
                RemoveClientByUserId(userId);
            }
            
        }

        public void RemoveClientByUserId(string userId)
        {
            var client = _cache.Get<RealOnlineClient>(userId);
            if (client != null)
            {
                string connId = client.Result.ConnId;
                _cache.Del(userId);
                _cache.Del(connId);
            }
            
        }
        private string ConnidToUserId(string connId)
        {
            return _cache.Get<string>(connId).Result;
        }
    }
}
