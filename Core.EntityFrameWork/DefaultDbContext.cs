using Core.Auth;
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

            if (user.TenantId != null && App.Configuration["ConnectionStrings:UseTenant"] == "yes")
            {
                try
                {
                    var t = this.Tenant;
                    return t.ConnectionString;
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