using Core.HttpTenant.Service;
using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant.HttpTenantContext
{
    public class GetTenantByHost : IGetTenantInHttpContext
    {
        private readonly TenantService ts;
        public GetTenantByHost(TenantService ts)
        {
            this.ts = ts;
        }
        public async Task<Tenant> Get(HttpContext context)
        {
            var host = context.Request.Host.ToString();
            return await Get(host);
        }

        public async Task<Tenant> Get(string key)
        {
            return await ts.GetTenantByHost(key);
        }
    }
}
