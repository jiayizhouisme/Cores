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
        private readonly IHttpContextUser user;
        private string defaultConnectString;

        public DefaultDbContext(DbContextOptions<T> options, IHttpContextUser user) : base(options)
        {
            this.user = user;
        }

        protected void SetConnectString(string connectString)
        {
            this.defaultConnectString = connectString;
        }

        public string GetDatabaseConnectionString()
        {

            if (user.TenantId != null)
            {
                try
                {
                    var t = base.Tenant;
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