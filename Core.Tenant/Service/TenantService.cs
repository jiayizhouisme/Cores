using Core.Cache;
using Furion.DatabaseAccessor;
using Furion.RemoteRequest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.HttpTenant.Service
{
    public class TenantService
    {
        private readonly ICacheOperation _cache;
        private readonly IServiceProvider _serviceProvider;
        public static string Tenent_Key = "Tenants";
        public TenantService(ICacheOperation _cache, IServiceProvider serviceProvider)
        {
            this._cache = _cache;
            _serviceProvider = serviceProvider;
        }

        public async Task<Tenant> GetTenant(string name)
        {
            Tenant tenant = null;
            var tenants = await _cache.GetList<Tenant>(Tenent_Key, 0);
            tenant = tenants.Where(a => a.Name == name).FirstOrDefault();
            if (tenant != null)
            {
                
                return tenant;
            }
            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var te = Db.GetRepository<Tenant, MultiTenantDbContextLocator>(scope.ServiceProvider);
                    tenant = te.AsQueryable(a => a.Name == name).FirstOrDefault();
                }
                return tenant;
            }
        }

        public async Task<Tenant> GetTenantByHost(string host)
        {
            Tenant tenant = null;
            var tenants = await _cache.GetList<Tenant>(Tenent_Key, 0);
            tenant = tenants.Where(a => a.Host == host).FirstOrDefault();
            if (tenant != null)
            {
                return tenant;
            }
            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var te = Db.GetRepository<Tenant, MultiTenantDbContextLocator>(scope.ServiceProvider);
                    tenant = te.AsQueryable(a => a.Host == host).FirstOrDefault();
                }
                return tenant;
            }
        }

    }
}
