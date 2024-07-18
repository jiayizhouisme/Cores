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
    public class GetTenantByUrl : IGetTenantInHttpContext
    {
        private readonly TenantService ts;
        public GetTenantByUrl(TenantService ts)
        {
            this.ts = ts;
        }
        public async Task<Tenant> Get(HttpContext context)
        {
            var name = context.Request.Path.Value.Split('/')[1];
            return await ts.GetTenant(name);
        }
    }
}
