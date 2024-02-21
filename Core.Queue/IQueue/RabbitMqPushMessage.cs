using Microsoft.AspNetCore.DataProtection;
using Quick.RabbitMQPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Queue.IQueue
{
    public class RabbitMqPushMessage : IQueuePushInfo
    {
        private readonly IQuickRabbitMQPlus _queuePushInfo;
        public RabbitMqPushMessage(IQuickRabbitMQPlus _queuePushInfo)
        {
            this._queuePushInfo = _queuePushInfo;
        }
        public async Task PushMessage(IQueueEntity eq)
        {
            var ret = await _queuePushInfo.Send(eq.body, eq.name);
            if (ret.Item1)
            {

            }
            else
            {
                var errMsg = $"失败原因：{ret.Item2}";
            }
            _queuePushInfo.Close();
        }

        public Task PushMessageDelay(IQueueEntity eq, int sec)
        {
            throw new NotImplementedException();
        }

        public Task PushMessageDelay(IQueueEntity eq, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
