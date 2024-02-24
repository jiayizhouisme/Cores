using BeetleX.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache
{
    public interface ICacheOperation
    {
        ValueTask<long> Lock(string key, string value, int expireTime = 10);

        ValueTask<long> LockNoWait(string key, string value, int expireTime = 10);

        ValueTask<long> ReleaseLock(string key, string value);

        ValueTask<string> Set(string key, object value);

        ValueTask<string> Set(string key, object value, int? extime);
        ValueTask<long> Del(params string[] key);
        ValueTask<T> Get<T>(string key);
        ValueTask<long> Expire(string key, int extime);
        ValueTask<long> PushToList<T>(string key, T value);
        ValueTask<long> RemoveFromList<T>(string key, T value);
        ValueTask<ICollection<T>> GetList<T>(string key, int start);
        ValueTask<long> Incrby(string key, int num, int extime = 10);
        ValueTask<long> Decrby(string key, int num, int extime = 10);
        ValueTask<long> Incr(string key, int extime = 10);
        ValueTask<long> Decr(string key, int extime = 10);
    }
}
