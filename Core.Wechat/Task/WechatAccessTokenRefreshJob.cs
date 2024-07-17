using Core.Cache;
using Core.Wechat.Rep;
using Core.Wechat.Safe;
using Furion.Schedule;
using SKIT.FlurlHttpClient.Wechat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Task
{
    public class WechatAccessTokenRefreshJob : IJob
    {
        private readonly IWechatConfig config;
        private readonly IWechat wechat;
        private readonly IWechatSafe safe;
        public WechatAccessTokenRefreshJob(IWechatConfig config, IWechat wechat, IWechatSafe safe)
        {
            this.config = config;
            this.wechat = wechat;
            this.safe = safe;
        }
        public async System.Threading.Tasks.Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
        {
            var r = await this.config.GetConfigs();
            foreach (var _r in r)
            {
                await safe.BeginCheckAccessToken(_r.key);
                var token = await wechat.GetToken(_r.key);
                if (token == null || wechat.IsTokenExpired(token.Value,300))
                {
                    await wechat.RegisteToken(_r.key);
                }
                await safe.EndCheckAccessToken(_r.key);
            }
        }
    }
}
