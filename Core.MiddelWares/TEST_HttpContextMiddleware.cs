using Core.HttpTenant.HttpTenantContext;
using Core.User.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant.Service
{

    public class TEST_HttpContextMiddleware
    {
        private readonly RequestDelegate _next;
        private IGetTenantInHttpContext _tenantInHttpContext;
        /// <summary>
        /// 构造 Http 请求中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="cacheService"></param>
        public TEST_HttpContextMiddleware(RequestDelegate next, IGetTenantInHttpContext _tenantInHttpContext)
        {
            _next = next;
            this._tenantInHttpContext = _tenantInHttpContext;
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


            await _next.Invoke(context);
            // 响应完成时存入缓存
            context.Response.OnCompleted(() =>
            {
                return Task.CompletedTask;
            });
        }
    }
}
