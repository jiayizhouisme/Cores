using InitQ.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Queue.IQueue
{
    public class InitQRedisPushMessage : IQueuePushInfo
    {
        private readonly ICacheService _initQ;

        public InitQRedisPushMessage(ICacheService _initQ)
        {
            this._initQ = _initQ;
        }
        public async Task PushMessage(IQueueEntity eq)
        {
            await _initQ.ListLeftPushAsync(eq.name, JsonConvert.SerializeObject(eq.body));
        }

        public async Task PushMessageDelay(IQueueEntity eq, int sec)
        {
            await _initQ.SortedSetAddAsync(eq.name, JsonConvert.SerializeObject(eq.body), sec);
        }

        public async Task PushMessageDelay(IQueueEntity eq, DateTime time)
        {
            await _initQ.SortedSetAddAsync(eq.name, JsonConvert.SerializeObject(eq.body), time);
        }
    }
}
