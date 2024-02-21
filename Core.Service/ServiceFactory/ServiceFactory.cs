using Core.Services;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.ServiceFactory
{
    public class ServiceFactory
    {
        public static T? GetSaasService<T, T1>(IServiceProvider provider, string id)
            where T1 : class, IPrivateEntity, new()
        {
            string connstr = GetConnstr(provider, id);

            var ret = provider.GetService<T>();
            if (!string.IsNullOrEmpty(connstr))
            {
                var _ret = (IBaseService<T1>)ret;
                _ret.SetDbConnectString(connstr);
            }
            return ret;
        }
        public static T? GetNamedSaasService<T, T1>(IServiceProvider provider, IBaseService<T1> service, string id)
            where T1 : class, IPrivateEntity, new()
        {
            var connstr = GetConnstr(provider, id);
            if (!string.IsNullOrEmpty(connstr))
            {
                service.SetDbConnectString(connstr);
            }
            return (T)service;
        }

        private static string GetConnstr(IServiceProvider provider, string id)
        {
            IMemoryCache _memoryCache = provider.GetService<IMemoryCache>();
            var tenantCachedKey = $"MULTI_TENANT:{id}";
            var cache_result = _memoryCache.Get<string>(tenantCachedKey);
            string connstr = "";
            if (cache_result == null)
            {
                var respository = Db.GetRepository<Tenant, MultiTenantDbContextLocator>(provider);
                connstr = respository.Where(a => a.Host == id).Select(a => a.ConnectionString).FirstOrDefault();
                _memoryCache.Set(tenantCachedKey, connstr);
            }
            else
            {
                connstr = cache_result;
            }
            return connstr;
        }
    }
}
