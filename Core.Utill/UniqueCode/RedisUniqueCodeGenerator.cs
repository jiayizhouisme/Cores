using Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utill.UniqueCode
{
    public class RedisUniqueCodeGenerator : IUniqueCodeGenerater<long>
    {
        private readonly ICacheOperation _cache;
        public RedisUniqueCodeGenerator(ICacheOperation _cache)
        {
            this._cache = _cache;
        }

        public async Task<long> Generate(string key)
        {
            long? res = await _cache.Get<long>(key);
            if (res == null)
            {
                await _cache.Set(key, 0, 60);
            }
            return await _cache.Incr(key);
        }
    }
}
