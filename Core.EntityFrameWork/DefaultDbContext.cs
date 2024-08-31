using Core.Auth;
using Core.HttpTenant.HttpTenantContext;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
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
        protected readonly IHttpContextUser user;
        protected string defaultConnectString;

        public DefaultDbContext(DbContextOptions<T> options, IHttpContextUser user) : base(options)
        {
            this.user = user;
        }

        protected void SetConnectString(string connectString)
        {
            this.defaultConnectString = connectString;
        }

        public virtual string GetDatabaseConnectionString()
        {
            var usetenant = App.Configuration["ConnectionStrings:UseTenant"];
            if (user.TenantId != null && (usetenant != null && usetenant == "yes"))
            {
                try
                {
                    IMemoryCache _memoryCache = App.GetService<IMemoryCache>();
                    var tenantCachedKey = $"MULTI_TENANT:{user.TenantId}";
                    var cache_result = _memoryCache.Get<string>(tenantCachedKey);
                    string connstr = "";
                    if (cache_result == null)
                    {
                        var service = App.GetService<IGetTenantInHttpContext>();
                        var t = service.Get(App.HttpContext).Result;
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
                    throw new System.Exception(user.TenantId);
                }
            }
            else
            {
                return defaultConnectString;
            }
        }

    }
}