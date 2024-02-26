using Core.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares
{
    public class PermissionAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly Permissions[] permissions;
        private readonly IHttpContextUser httpContextUser;

        public PermissionAuthFilter(IHttpContextUser httpContextUser, Permissions[] permissions)
        {
            this.httpContextUser = httpContextUser;
            this.permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!permissions.Contains(httpContextUser.Permissions))
            {
                context.Result = new UnauthorizedResult();
            }
            await Task.CompletedTask;
        }
    }
}
