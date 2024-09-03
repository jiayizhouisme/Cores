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
            return await Get(name);
        }

        public async Task<Tenant> Get(string key)
        {
            return await ts.GetTenant(key);
        }

        public void Set(HttpContext context,string value)
        {
            var request = context.Request;
            if (request.Path.Value.EndsWith("/" + value))
            {
                request.Path = new PathString(request.Path.Value.Replace("/" + value, "/"));
            }
            else
            {
                request.Path = new PathString(request.Path.Value.Replace("/" + value + "/", "/"));
            }
        }
    }
}
