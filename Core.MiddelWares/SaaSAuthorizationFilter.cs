using Core.Auth;
using Core.Config;
using Core.HttpTenant;
using Core.HttpTenant.HttpTenantContext;
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
        private readonly IGetTenantInHttpContext getTenantInHttpContext;
        public SaaSAuthorizationFilter(IHttpContextUser httpContextUser, ITenantGetSetor tenantGetSetor, IGetTenantInHttpContext getTenantInHttpContext)
        {
            this.httpContextUser = httpContextUser;
            this.tenantGetSetor = tenantGetSetor;
            this.getTenantInHttpContext = getTenantInHttpContext;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string realTenant;
            if (Configration.tenantConfigType == TenantConfigTypes.ByHost)
            {
                var tenant = await getTenantInHttpContext.Get(tenantGetSetor.Get());
                realTenant = httpContextUser.RealTenantId;
                if (tenant.Name != realTenant)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                var tenant_id = tenantGetSetor.Get();
                realTenant = httpContextUser.TenantId;
                if (tenant_id == null ||
                    realTenant.IsNullOrEmpty() ||
                    tenant_id != realTenant)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            
            await Task.CompletedTask;
        }
    }
}
