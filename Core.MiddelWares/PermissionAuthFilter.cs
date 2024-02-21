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
        private readonly IHttpContextUser httpContextUser;
        public PermissionAuthFilter(IHttpContextUser httpContextUser)
        {
            this.httpContextUser = httpContextUser;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (httpContextUser.Permissions != Permissions.Administrator)
            {
                context.Result = new UnauthorizedResult();
            }
            await Task.CompletedTask;
        }
    }
}
