﻿using Core.Auth;
using Core.MiddelWares.HttpTenantContextMiddleWare;
using Core.Services.ServiceFactory;
using Core.User.Entity;
using Core.User.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares
{
    public class WebRouteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory serviceFactory;
       
        /// <summary>
        /// 构造 Http 请求中间件
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="cacheService"></param>
        public WebRouteMiddleware(RequestDelegate next, IServiceScopeFactory serviceFactory)
        {
            _next = next;
            this.serviceFactory = serviceFactory;

        }

        /// <summary>
        /// 1：将Post方法中Body中的数据进行AES解密
        /// 2：将返回数据进行AES加密
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var path1 = request.Path.Value;
            if (!path1.StartsWith("/api"))
            {
                var tenant_name = request.Headers[HttpContextMiddleware.Key_TenantName].ToString();
                if (tenant_name != null && !string.IsNullOrEmpty(tenant_name))
                {
                    using (var scope = serviceFactory.CreateScope())
                    {
                        var user = scope.ServiceProvider.GetService<IHttpContextUser>();
                        var service = ServiceFactory.GetSaasService<IWebRouteConfigService, WebRouteConfig>(scope.ServiceProvider
                            , user.TenantId);

                        var red_path = await service.GetConfig(path1);
                        if (red_path != null)
                        {
                            request.Path = new PathString(red_path.routePath);
                        }
                    }
                }
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
