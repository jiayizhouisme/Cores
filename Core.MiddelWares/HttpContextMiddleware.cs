﻿using Core.MiddelWares.HttpTenantContextMiddleWare;
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

namespace Core.MiddelWares
{

    public class HttpContextMiddleware
    {
        private readonly RequestDelegate _next;
        private IGetTenantInHttpContext _tenantInHttpContext;
        public static string Key_TenantName = "Tenant_Name";
        /// <summary>
        /// 构造 Http 请求中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="cacheService"></param>
        public HttpContextMiddleware(RequestDelegate next, IGetTenantInHttpContext _tenantInHttpContext)
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

            var tenant = await _tenantInHttpContext.Get(context);

            if (tenant != null)
            {
                request.Headers.Append("Origin_Host", request.Host.ToString());
                if (_tenantInHttpContext is GetTenantByUrl) {
                    if (request.Path.Value.EndsWith("/" + tenant.Name))
                    {
                        request.Path = new PathString(request.Path.Value.Replace("/" + tenant.Name, "/"));
                    }
                    else
                    {
                        request.Path = new PathString(request.Path.Value.Replace("/" + tenant.Name + "/", "/"));
                    }
                }
                request.Host = new HostString(tenant.Host);
                request.Headers.Append(HttpContextMiddleware.Key_TenantName, tenant.Name);
            }

            await _next.Invoke(context);
            // 响应完成时存入缓存
            context.Response.OnCompleted(() =>
            {
                StringValues str;
                request.Headers.TryGetValue("Origin_Host", out str);
                if (!string.IsNullOrEmpty(str))
                {
                    request.Host = new HostString(str.ToString());
                }
                
                return Task.CompletedTask;
            });
        }
    }
}
