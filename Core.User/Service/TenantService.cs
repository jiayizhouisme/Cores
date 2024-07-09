using Core.Cache;
using Core.Services;
using Furion.DatabaseAccessor;
using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public class TenantService
    {
        private readonly ICacheOperation _cache;
        public TenantService(ICacheOperation _cache)
        {
            this._cache = _cache;
        }

        public async Task<Tenant> GetTenant(string name)
        {
            Tenant tenant = null;
            var tenants = await _cache.GetList<Tenant>("Tenants",0);
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
            var tenants = await _cache.GetList<Tenant>("Tenants", 0);
            tenant = tenants.Where(a => a.Host == host).FirstOrDefault();
            if (tenant != null)
            {
                return tenant;
            }
            return null;
        }

    }
}
