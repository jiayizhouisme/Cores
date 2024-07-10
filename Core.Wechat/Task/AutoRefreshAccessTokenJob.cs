using Furion.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Task
{
    public class AutoRefreshAccessTokenJob : IJob
    {
        private readonly IWechat wechat;
        public AutoRefreshAccessTokenJob(IWechat wechat)
        {
            this.wechat = wechat;
        }
        public async System.Threading.Tasks.Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
        {
            
        }
    }
}
