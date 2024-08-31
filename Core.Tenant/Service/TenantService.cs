using Core.Cache;
using Furion.DatabaseAccessor;
using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant.Service
{
    public class TenantService
    {
        private readonly ICacheOperation _cache;
        public static string Tenent_Key = "Tenants";
        public TenantService(ICacheOperation _cache)
        {
            this._cache = _cache;
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
            return null;
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
            return null;
        }

    }
}
