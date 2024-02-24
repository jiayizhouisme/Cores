using BeetleX.Redis;
using BeetleX.Redis.Commands;
using Furion;
using System.Composition;
using static BeetleX.Redis.Commands.HSCAN;

namespace Core.Cache
{
    public class BettleX_Redis : RedisDB, ICacheOperation
    {
        private RedisHost redisHost;
        public BettleX_Redis()
        {
            redisHost = this.Host.AddWriteHost(App.Configuration["BeetleXConnectionString:Host"]);
            redisHost.Password = App.Configuration["BeetleXConnectionString:Password"];
            redisHost.MaxConnections = 500;
            this.KeyPrefix = App.Configuration["ServerConfig:CachePrefix"];
            this.DataFormater = new JsonFormater();
        }

        public async ValueTask<long> Lock(string key, string value, int expireTime = 10)
        {
            var result = await this.SetNX(key, value);
            while (result != 1)
            {
                await Task.Delay(200);
                result = await this.SetNX(key, value);
            }
            await this.Expire(key, expireTime);
            return result;
        }

        public async ValueTask<long> LockNoWait(string key, string value, int expireTime = 10)
        {
            var result = await this.SetNX(key, value);
            if (result != 1)
            {
                return 0;
            }
            await this.Expire(key, expireTime);
            return 1;
        }

        public async ValueTask<long> ReleaseLock(string key, string value)
        {
            if (value != null)
            {
                var result = await this.Get<string>(key);
                if (result != null && result.CompareTo(value) == 0)
                {
                    return await this.Del(key);
                }
                else
                {
                    return 0;
                }
            }
            return await this.Del(key);
        }

        public new async ValueTask<string> Set(string key, object value)
        {
            return await base.Set(key, value, null, null);
        }

        public new async ValueTask<string> Set(string key, object value, int? extime)
        {
            return await base.Set(key, value, extime, null);
        }

        public new async ValueTask<long> Del(params string[] key)
        {
            return await base.Del(key);
        }

        public new async ValueTask<T> Get<T>(string key)
        {
            return await base.Get<T>(key);
        }

        public async ValueTask<long> PushToList<T>(string key, T value)
        {
            var list = base.CreateList<T>(key);
            return await list.Push(value);
        }

        public async ValueTask<ICollection<T>> GetList<T>(string key, int start)
        {
            var list = base.CreateList<T>(key);
            var len = await list.Len();
            var tickets = (await list.Range(start, (int)len)).ToList();
            return tickets;
        }

        public ValueTask<long> Expire(string key, int extime)
        {
            return base.Expire(key, extime);
        }

        public async ValueTask<long> Incrby(string key, int num, int expire = 10)
        {
            var result = await base.Incrby(key, num);
            await base.Expire(key, expire);
            return result;
        }

        public async ValueTask<long> Decrby(string key, int num, int expire = 10)
        {
            var result = await base.Decrby(key, num);
            await base.Expire(key, expire);
            return result;
        }

        public async ValueTask<long> Incr(string key, int expire)
        {
            var result = await base.Incr(key);
            await base.Expire(key, expire);
            return result;
        }

        public async ValueTask<long> Decr(string key, int expire)
        {
            var result = await base.Decr(key);
            await base.Expire(key, expire);
            return result;
        }

        public ValueTask<long> RemoveFromList<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}
