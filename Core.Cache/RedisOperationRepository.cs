using Core.Config;
using Furion;
using Furion.JsonSerialization;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Core.Cache
{
    public class RedisOperationRepository : ICacheOperation
    {
        private readonly IJsonSerializerProvider _serializerProvider;
        private readonly ILogger<RedisOperationRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly string keyprefix;

        public RedisOperationRepository(
            ILogger<RedisOperationRepository> logger,
            ConnectionMultiplexer redis,
            IJsonSerializerProvider _serializerProvider)
        {
            this._serializerProvider = _serializerProvider;
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
            this.keyprefix = Configration.CachePrefix;
        }
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }

        public async Task Clear()
        {
            foreach (var endPoint in _redis.GetEndPoints())
            {
                var server = GetServer();
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }
        public async ValueTask<long> Lock(string key, string value, int expireTime = 10)
        {
            key = keyprefix + key;
            var _lock = await this._database.LockTakeAsync(key, value, TimeSpan.FromSeconds(expireTime));
            while (_lock != true)
            {
                await Task.Delay(200);
                _lock = await this._database.LockTakeAsync(key, value, TimeSpan.FromSeconds(expireTime));
            }

            return 1;
        }

        public async ValueTask<long> LockNoWait(string key, string value, int expireTime = 10)
        {
            key = keyprefix + key;
            var ret = await this._database.LockTakeAsync(key, value, TimeSpan.FromSeconds(expireTime));
            if (ret == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async ValueTask<long> ReleaseLock(string key, string value)
        {
            key = keyprefix + key;
            var _lock = await this._database.LockReleaseAsync(key, value);
            if (_lock == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async ValueTask<string> Set(string key, object value)
        {
            key = keyprefix + key;
            string ret = null;
            if (value != null)
            {
                ret = _serializerProvider.Serialize(value);
                await _database.StringSetAsync(key, _serializerProvider.Serialize(value));
            }
            return ret;
        }

        public async ValueTask<string> Set(string key, object value, int? extime)
        {
            key = keyprefix + key;
            string ret = null;
            if (value != null)
            {
                ret = _serializerProvider.Serialize(value);
                await _database.StringSetAsync(key, ret, TimeSpan.FromSeconds(extime.Value));
            }
            return ret;
        }

        public async ValueTask<long> Del(params string[] key)
        {
            key[0] = keyprefix + key[0];
            var _lock = await _database.KeyDeleteAsync(key[0]);
            if (_lock == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async ValueTask<T> Get<T>(string key)
        {
            key = keyprefix + key;
            var value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return _serializerProvider.Deserialize<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public async ValueTask<long> Expire(string key, int extime)
        {
            key = keyprefix + key;
            var ret = await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(extime));
            if (ret == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async ValueTask<long> PushToList<T>(string key, T value)
        {
            key = keyprefix + key;
            return await _database.ListRightPushAsync(key, _serializerProvider.Serialize(value));
        }

        public async ValueTask<ICollection<T>> GetList<T>(string key, int start)
        {
            key = keyprefix + key;
            var length = await _database.ListLengthAsync(key);
            var result = await _database.ListRangeAsync(key, start, length);
            var list = result.Select(o => _serializerProvider.Deserialize<T>(o)).ToList();
            return list;
        }

        public async ValueTask<long> Incrby(string key, int num, int extime = 10)
        {
            key = keyprefix + key;
            var result = await _database.StringIncrementAsync(key, num);
            await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(extime));

            return result;
        }

        public async ValueTask<long> Decrby(string key, int num, int extime = 10)
        {
            key = keyprefix + key;
            var result = await _database.StringDecrementAsync(key, num);
            await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(extime));

            return result;
        }

        public async ValueTask<long> Incr(string key, int extime = 10)
        {
            key = keyprefix + key;
            var result = await _database.StringIncrementAsync(key);
            await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(extime));

            return result;
        }

        public async ValueTask<long> Decr(string key, int extime = 10)
        {
            key = keyprefix + key;
            var result = await _database.StringDecrementAsync(key);
            await _database.KeyExpireAsync(key, TimeSpan.FromSeconds(extime));

            return result;
        }

        public async ValueTask<long> RemoveFromList<T>(string key, T value)
        {
            key = keyprefix + key;
            return await _database.ListRemoveAsync(key, _serializerProvider.Serialize(value));
        }
    }
}
