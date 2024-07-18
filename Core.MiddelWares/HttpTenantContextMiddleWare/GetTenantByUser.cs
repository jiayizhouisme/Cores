using Core.Auth;
using Core.User.Service;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares.HttpTenantContextMiddleWare
{
    public class GetTenantByUser : IGetTenantInHttpContext
    {
        private readonly TenantService ts;
        private readonly IServiceProvider serviceProvider;
        public GetTenantByUser(TenantService ts, IServiceProvider serviceProvider)
        {
            this.ts = ts;
            this.serviceProvider = serviceProvider;
        }
        public async Task<Tenant> Get(HttpContext context)
        {
            using var scope = serviceProvider.CreateScope();
            var user = App.GetService<IHttpContextUser>(scope.ServiceProvider);
            return await ts.GetTenant(user.RealTenantId);
        }
    }
}
