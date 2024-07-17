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
    public interface IGetTenantInHttpContext
    {
        public Task<Tenant> Get(HttpContext context);
    }
}
