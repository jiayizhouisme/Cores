using Core.MiddelWares.HttpTenantContextMiddleWare;
using Core.User.Service;
using Furion;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares
{

    public class SubSiteMiddleWare
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造 Http 请求中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="cacheService"></param>
        public SubSiteMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 1：将Post方法中Body中的数据进行AES解密
        /// 2：将返回数据进行AES加密
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {

            context.Request.EnableBuffering();

            var request = context.Request;

            
            var value = request.Path.Value;
            var subPath = App.Configuration["ServerConfig:SubSite:SubPath"];
            if (value.StartsWith(subPath))
            {
                
                request.Path = new PathString(request.Path.Value.Replace(subPath,""));
            }
            
            await _next.Invoke(context);
            // 响应完成时存入缓存
            context.Response.OnCompleted(() =>
            {
                return Task.CompletedTask;
            });
        }
    }
}
