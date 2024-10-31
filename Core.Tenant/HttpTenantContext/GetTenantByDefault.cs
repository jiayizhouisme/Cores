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
    public class GetTenantByDefault : IGetTenantInHttpContext
    {
        private readonly TenantService ts;
        public GetTenantByDefault(TenantService ts)
        {
            this.ts = ts;
        }
        public async Task<Tenant> Get(HttpContext context)
        {
            return await ts.GetTenant("anfeng");
        }

        public async Task<Tenant> Get(string key)
        {
            return await ts.GetTenant("anfeng");
        }

        public void Set(HttpContext context, string value)
        {

        }
    }
}
