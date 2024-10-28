using Core.Auth;
using Core.HttpTenant.HttpTenantContext;
using Core.HttpTenant.Service;
using Core.HttpTenant;
using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Config
{
    public static class TenantConfig
    {
        public static IServiceCollection AddTenantService(this IServiceCollection services, TenantConfigTypes type)
        {
            services.AddSingleton<TenantService>();

            if (type == TenantConfigTypes.ByUrl)
            {
                services.AddScoped<IHttpContextUser, JwtUserContext_Real>();
                services.AddSingleton<ITenantGetSetor, TenantHttpContextGS>();
                services.AddSingleton<IGetTenantInHttpContext, GetTenantByUrl>();
            }
            else if (type == TenantConfigTypes.ByUser)
            {
                services.AddScoped<IHttpContextUser, JwtUserContext_Real>();
                services.AddSingleton<ITenantGetSetor, TenantHttpContextGS>();
                services.AddSingleton<IGetTenantInHttpContext, GetTenantByUser>();
            }
            else if (type == TenantConfigTypes.ByHost)
            {
                services.AddScoped<IHttpContextUser, JwtUserContext>();
                services.AddSingleton<ITenantGetSetor, TenantHttpContextHost>();
                services.AddSingleton<IGetTenantInHttpContext, GetTenantByHost>();
            }
            return services;
        }
    }
    public enum TenantConfigTypes
    {
        ByUrl,
        ByUser,
        ByHost
    }

}
