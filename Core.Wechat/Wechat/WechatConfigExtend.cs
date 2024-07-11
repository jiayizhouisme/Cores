using Core.Wechat.Entity;
using Core.Wechat.Rep;
using Core.Wechat.Task;
using Furion.DatabaseAccessor;
using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;
using SKIT.FlurlHttpClient.Wechat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeChat<T>(this IServiceCollection services) where T : class,IWechatConfig
        {
            services.AddSingleton<IWechat,Wechat>();
            services.AddTransient<IWechatConfig,T>();
            
            services.AddSchedule(a => {
                a.AddJob<LoadWechatConfigJob>(Triggers.Daily().SetRunOnStart(true));
            });
            return services;
        }
    }
}
