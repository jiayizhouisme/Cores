using Core.Wechat.Rep;
using Furion.DataValidation;
using Furion.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Task
{
    public class LoadWechatConfigJob : IJob
    {
        private readonly IWechatConfig config;
        private readonly IWechat wechat;
        public LoadWechatConfigJob(IWechatConfig config, IWechat wechat)
        {
            this.config = config;
            this.wechat = wechat;
        }
        public async System.Threading.Tasks.Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
        {
            var r = await this.config.GetConfigs();
            foreach (var _r in r)
            {
                var token = await wechat.GetToken(_r.key);
                if (token == null || wechat.IsTokenExpired(token.Value))
                {
                    await wechat.RegisteToken(_r.key);
                }
            }
        }
    }
}
