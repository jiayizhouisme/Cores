using Core.Auth;
using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace Core.MiddelWares
{
    public static class RateLimitPolicy
    {
        /// <summary>
        /// 请求上下文中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IServiceCollection AddMyRateLimiter(this IServiceCollection services,
            string policyName = "my_policy",
            int PermitLimit = 3,int _window = 60,
            QueueProcessingOrder _QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            int _queueLimit = 0)
        {
            return services.AddRateLimiter(options =>
            {
                options.AddPolicy(policyName: policyName, httpcontext =>
                {
                    var userId = httpcontext.GetRemoteIpAddressToIPv4();

                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: userId, _ => new
                        FixedWindowRateLimiterOptions
                    {
                        PermitLimit = PermitLimit,
                        Window = TimeSpan.FromSeconds(_window),
                        QueueProcessingOrder = _QueueProcessingOrder,
                        QueueLimit = _queueLimit
                    });
                });


            });
        }

        /// <summary>
        /// 请求上下文中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserRateLimiter(this IServiceCollection services,
            string policyName = "user_rate_policy",
            int PermitLimit = 3, int _window = 60,
            QueueProcessingOrder _QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            int _queueLimit = 0)
        {
            return services.AddRateLimiter(options =>
            {
                options.AddPolicy(policyName: policyName, httpcontext =>
                {
                    string userId = App.GetService<IHttpContextUser>().ID;
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: userId, _ => new
                        FixedWindowRateLimiterOptions
                    {
                        PermitLimit = PermitLimit,
                        Window = TimeSpan.FromSeconds(_window),
                        QueueProcessingOrder = _QueueProcessingOrder,
                        QueueLimit = _queueLimit
                    });
                });


            });
        }

        
    }
    
}
