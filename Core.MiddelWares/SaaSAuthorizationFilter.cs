using Core.Auth;
using Core.HttpTenant;
using Furion.LinqBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares
{
    public class SaaSAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextUser httpContextUser;
        private readonly ITenantGetSetor tenantGetSetor;
        public SaaSAuthorizationFilter(IHttpContextUser httpContextUser, ITenantGetSetor tenantGetSetor)
        {
            this.httpContextUser = httpContextUser;
            this.tenantGetSetor = tenantGetSetor;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var tenant_id = tenantGetSetor.Get();
            var realTenant = httpContextUser.RealTenantId;
            if (tenant_id == null ||
                realTenant.IsNullOrEmpty() ||
                tenant_id != realTenant)
            {
                context.Result = new UnauthorizedResult();
            }
            await Task.CompletedTask;
        }
    }
}
