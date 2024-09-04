using Core.Auth;
using Core.HttpTenant;
using Core.HttpTenant.HttpTenantContext;
using Core.MiddelWares;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling.Internal;

namespace Core.EntityFrameWork
{
    public abstract class DefaultDbContext<T> : AppDbContext<T>, IMultiTenantOnDatabase where T : DbContext
    {
        protected string defaultConnectString;
        private readonly ITenantGetSetor tenantGetSetor;

        public DefaultDbContext(DbContextOptions<T> options, ITenantGetSetor tenantGetSetor) : base(options)
        {
            this.InsertOrUpdateIgnoreNullValues = true;
            this.tenantGetSetor = tenantGetSetor;
        }

        protected void SetConnectString(string connectString)
        {
            this.defaultConnectString = connectString;
        }

        public virtual string GetDatabaseConnectionString()
        {
            var tenant_name = tenantGetSetor.Get();
            var usetenant = App.Configuration["ConnectionStrings:UseTenant"];
            if (!string.IsNullOrEmpty(tenant_name) && (usetenant != null && usetenant == "yes"))
            {
                try
                {
                    IMemoryCache _memoryCache = App.GetService<IMemoryCache>();
                    var tenantCachedKey = $"MULTI_TENANT:{tenant_name}";
                    var cache_result = _memoryCache.Get<string>(tenantCachedKey);
                    string connstr = "";
                    if (cache_result == null)
                    {
                        var service = App.GetService<IGetTenantInHttpContext>();
                        var t = service.Get(tenant_name).Result;
                        if (t != null)
                        {
                            connstr = t.ConnectionString;
                            _memoryCache.Set(tenantCachedKey, connstr);
                        }
                        else
                        {
                            return defaultConnectString;
                        }
                    }
                    else
                    {
                        connstr = cache_result;
                    }

                    return connstr;
                }
                catch
                {
                    throw new System.Exception(tenant_name);
                }
            }
            else
            {
                return defaultConnectString;
            }
        }

    }
}