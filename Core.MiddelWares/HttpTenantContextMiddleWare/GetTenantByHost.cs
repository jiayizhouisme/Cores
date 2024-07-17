using Core.User.Service;
using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares.HttpTenantContextMiddleWare
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
            return await ts.GetTenantByHost(host);
        }
    }
}
